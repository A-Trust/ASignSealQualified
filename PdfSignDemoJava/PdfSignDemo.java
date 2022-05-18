import com.fasterxml.jackson.databind.ObjectMapper;
import eu.europa.esig.dss.enumerations.SignatureAlgorithm;
import eu.europa.esig.dss.enumerations.SignatureLevel;
import eu.europa.esig.dss.model.*;
import eu.europa.esig.dss.model.x509.CertificateToken;
import eu.europa.esig.dss.pades.PAdESSignatureParameters;
import eu.europa.esig.dss.pades.SignatureFieldParameters;
import eu.europa.esig.dss.pades.SignatureImageParameters;
import eu.europa.esig.dss.pades.signature.PAdESService;
import eu.europa.esig.dss.pdf.pdfbox.PdfBoxNativeObjectFactory;
import eu.europa.esig.dss.validation.CommonCertificateVerifier;
import org.apache.http.HttpEntity;
import org.apache.http.client.methods.CloseableHttpResponse;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.ByteArrayEntity;
import org.apache.http.impl.client.CloseableHttpClient;
import org.apache.http.impl.client.HttpClients;
import org.apache.http.util.EntityUtils;
import org.bouncycastle.asn1.ASN1Integer;
import org.bouncycastle.asn1.DERSequenceGenerator;
import org.bouncycastle.asn1.x500.RDN;
import org.bouncycastle.asn1.x500.X500Name;
import org.bouncycastle.asn1.x500.style.BCStyle;
import org.bouncycastle.asn1.x500.style.IETFUtils;
import org.bouncycastle.cert.jcajce.JcaX509CertificateHolder;

import java.io.*;
import java.math.BigInteger;
import java.nio.charset.StandardCharsets;
import java.security.*;
import java.security.cert.CertificateFactory;
import java.security.cert.X509Certificate;
import java.util.Base64;
import java.util.Enumeration;
import java.util.List;
import java.util.Map;

/**
 * Standalone DSS A-Trust qualified signature test for the MCWE for A-Trust
 */
public class PdfSignDemo {
    public static void main(String[] args) throws Exception {
        char[] pfxPassword = "testpwd".toCharArray();
        String pfxFile = "./../test_credentials/authentication_certificate.p12";
        String baseurl = "http://hs-abnahme.a-trust.at/SealQualified/v1/";  // test system (HTTP)
        //String baseurl = "https://hs-abnahme.a-trust.at/SealQualified/v1/";  // test system (HTTPS)
        //String baseurl = "https://www.a-trust.at/SealQualified/v1/";  // live system (only HTTPS)

        // load pkcs12
        Security.setProperty("crypto.policy", "unlimited");

        KeyStore keystore = KeyStore.getInstance("PKCS12");
        keystore.load(new FileInputStream(pfxFile), pfxPassword);

        String keyAlias = "";
        for (Enumeration en = keystore.aliases(); en.hasMoreElements();) {
            String alias = (String)en.nextElement();
            if(keystore.isKeyEntry(alias)) {
                keyAlias = alias;
                break;
            }
        }

        PrivateKey key = (PrivateKey) keystore.getKey(keyAlias, pfxPassword);
        X509Certificate cert = (X509Certificate) keystore.getCertificate(keyAlias);
        String serial = cert.getSerialNumber().toString();

        // get seal certificate
        System.out.println("=================");
        System.out.println("get seal certificate");
        String getUrl = baseurl + "/Certificate/" + serial + "/nosessionid";
        byte[] seal_certificate_raw = Get(getUrl);

        String seal_certificate_base64 = new String(Base64.getEncoder().encode(seal_certificate_raw));
        System.out.println("seal certificate: " + seal_certificate_base64);


        System.out.println("=================");
        System.out.println("sign with seal private key");

        // START PART 1 - Until here this demo application is the same as DemoJava

        X509Certificate seal_certificate_x509 = (X509Certificate) CertificateFactory.getInstance("X.509")
                .generateCertificates(new ByteArrayInputStream(seal_certificate_raw)).stream().findFirst().get();

        // Downloaded sample from https://svn.apache.org/repos/asf/tika/trunk/tika-parsers/src/test/resources/test-documents/testPDF_Version.8.x.pdf
        DSSDocument doc = new FileDocument("testPDF_Version.8.x.pdf");

        PAdESSignatureParameters parameters = prepareSignParameters(seal_certificate_x509);

        // We need this because we are testing with an expired certificate
        parameters.setSignWithExpiredCertificate(true);

        // Create common certificate verifier
        CommonCertificateVerifier commonCertificateVerifier = new CommonCertificateVerifier();
        // Create PAdESService for signature
        PAdESService service = new PAdESService(commonCertificateVerifier);
        service.setPdfObjFactory(new PdfBoxNativeObjectFactory());
        // Get the SignedInfo segment that need to be signed.
        ToBeSigned dataToSignDSS = service.getDataToSign(doc, parameters);
        byte[] dataToSign = dataToSignDSS.getBytes();

        // END PART 2 - We now have data from the prepared PDF with certificate information and PDF structure for signature
        //              and sign it with the A-Trust qualified signature test service

        MessageDigest hashAlgo = MessageDigest.getInstance("SHA-256");
        byte[] hashToSign = hashAlgo.digest(dataToSign);

        // prepare Request
        Signature sig = Signature.getInstance("SHA256WithRSA");
        sig.initSign(key);
        sig.update(hashToSign);
        byte[] HashSignature = sig.sign();


        String request = "{\"AuthSerial\": \"" + serial + "\", \"Hash\": \"";
        request += new String(Base64.getEncoder().encode(hashToSign));
        request +="\", \"HashSignature\": \"";
        request += new String(Base64.getEncoder().encode(HashSignature));
        request +="\", \"HashSignatureMechanism\": \"SHA256withRSA\" }";

        String postUrl = baseurl + "/Sign/nosessionid";
        String result = Post(postUrl,request);
        System.out.println("result: " + result);

        // START PART 2 - The returned signature is now integrated in the prepared PDF structure and written to file
        Map resultMap = new ObjectMapper().readValue(result.getBytes(StandardCharsets.UTF_8), Map.class);

        byte[] jwsSignature = Base64.getDecoder().decode((String) resultMap.get("Signature"));
        byte[] derSignature = convertJWSConcatenatedToDEREncodedSignature(jwsSignature);

        SignatureValue signatureValue = new SignatureValue(SignatureAlgorithm.ECDSA_SHA256, derSignature);

        // We invoke the PadesService to sign the document with the signature value obtained in
        // the previous step.
        DSSDocument signedDocument = service.signDocument(doc, parameters, signatureValue);

        signedDocument.writeTo(new FileOutputStream("output.pdf"));
    }

    private static PAdESSignatureParameters prepareSignParameters(X509Certificate cert) throws Exception {
        // Preparing parameters for the PAdES signature
        PAdESSignatureParameters parameters = new PAdESSignatureParameters();
        // We choose the level of the signature (-B, -T, -LT, -LTA).
        parameters.setSignatureLevel(SignatureLevel.PAdES_BASELINE_B);

        parameters.setReason("Signature verification at: http://www.signature-verification.gv.at");
        parameters.setContactInfo("Amtssignatur contact info");

        // get certificates holder name
        X500Name x500name = new JcaX509CertificateHolder(cert).getSubject();
        RDN cn = x500name.getRDNs(BCStyle.CN)[0];
        parameters.setSignerName(IETFUtils.valueToString(cn.getFirst().getValue()));

        CertificateToken certToken = new CertificateToken(cert);

        // We set the signing certificate
        parameters.setSigningCertificate(certToken);

        // We set the certificate chain
        parameters.setCertificateChain(List.of(certToken));

        // Initialize visual signature and configure
        SignatureImageParameters imageParameters = new SignatureImageParameters();
        // set an image
        imageParameters.setImage(new InMemoryDocument(new FileInputStream("signature-pen.png")));

        // initialize signature field parameters
        SignatureFieldParameters fieldParameters = new SignatureFieldParameters();
        imageParameters.setFieldParameters(fieldParameters);
        // the origin is the left and top corner of the page
        fieldParameters.setOriginX(20);
        fieldParameters.setOriginY(40);
        fieldParameters.setWidth(300);
        fieldParameters.setHeight(200);

        parameters.setImageParameters(imageParameters);

        return parameters;
    }

    /**
     * Helper method to convert concatenated signature values (as used by the JWS-standard) to
     * DER-encoded signature values (e.g. used by Java)
     *
     * Copied from: https://github.com/BMF-RKSV-Technik/at-registrierkassen-mustercode/blob/master/regkassen-common/src/main/java/at/asitplus/regkassen/common/util/CryptoUtil.java
     *
     * @param concatenatedSignatureValue
     *          concatenated signature value (as used by JWS standard)
     * @return DER-encoded signature value
     * @throws IOException
     */
    private static byte[] convertJWSConcatenatedToDEREncodedSignature(final byte[] concatenatedSignatureValue) throws IOException {

        final byte[] r = new byte[33];
        final byte[] s = new byte[33];
        System.arraycopy(concatenatedSignatureValue, 0, r, 1, 32);
        System.arraycopy(concatenatedSignatureValue, 32, s, 1, 32);
        final BigInteger rBigInteger = new BigInteger(r);
        final BigInteger sBigInteger = new BigInteger(s);

        final ByteArrayOutputStream bos = new ByteArrayOutputStream();
        final DERSequenceGenerator seqGen = new DERSequenceGenerator(bos);

        seqGen.addObject(new ASN1Integer(rBigInteger.toByteArray()));
        seqGen.addObject(new ASN1Integer(sBigInteger.toByteArray()));
        seqGen.close();
        bos.close();

        final byte[] derEncodedSignatureValue = bos.toByteArray();

        return derEncodedSignatureValue;
    }

    private static String Post(String postUrl, String requestJson) throws Exception
    {
        CloseableHttpClient httpClient = HttpClients.createDefault();
        HttpPost httpPost = new HttpPost(postUrl);
        httpPost.setHeader("Accept", "application/json");
        httpPost.setHeader("Content-type", "application/json");
        httpPost.setEntity(new ByteArrayEntity(requestJson.getBytes("UTF8")));

        CloseableHttpResponse httpResponse = httpClient.execute(httpPost);

        HttpEntity resEntity = httpResponse.getEntity();
        String content = EntityUtils.toString(resEntity);
        httpClient.close();
        return content;
    }

    private static byte[] Get(String getUrl) throws Exception
    {
        CloseableHttpClient httpClient = HttpClients.createDefault();
        HttpGet httpGet = new HttpGet(getUrl);
        CloseableHttpResponse httpResponse = httpClient.execute(httpGet);

        byte[] content = EntityUtils.toByteArray(httpResponse.getEntity());
        httpClient.close();
        return content;
    }
}