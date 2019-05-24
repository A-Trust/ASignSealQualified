import java.io.*;
import java.security.KeyStore;
import java.security.Security;
import java.security.PrivateKey;
import java.security.cert.CertificateEncodingException;
import java.security.cert.CertificateException;
import java.security.cert.X509Certificate;
import java.security.MessageDigest;
import java.security.KeyPair;
import java.security.KeyPairGenerator;
import java.security.NoSuchAlgorithmException;
import java.security.Signature;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Iterator;
import java.util.List;
import java.util.Enumeration;
import java.util.Base64;

import org.apache.http.HttpResponse;
import org.apache.http.client.HttpClient;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.mime.MultipartEntityBuilder;
import org.apache.http.entity.ContentType;
import org.apache.http.util.EntityUtils;
import org.apache.http.HttpEntity;
import org.apache.http.client.methods.CloseableHttpResponse;
import org.apache.http.impl.client.CloseableHttpClient;
import org.apache.http.impl.client.HttpClients;
import org.apache.http.entity.ByteArrayEntity;

public class DemoClient
{
	public static void main(String[] args) throws Exception{		
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


		// Hash to Sign
		byte[] dataToSign = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.".getBytes();
		
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




























