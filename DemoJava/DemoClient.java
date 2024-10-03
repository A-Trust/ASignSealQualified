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
import org.apache.http.StatusLine;
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
		String baseurl = "https://test.seal.a-trust.at/SealQualified/v1/";  // test system (HTTPS)
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



		System.out.println("================="); 
		System.out.println("get seal certificate"); 
		GetSealCertificate(baseurl, serial);
		System.out.println("================="); 		
		System.out.println("sign with seal private key"); 
		Sign(baseurl, key, serial);
		System.out.println("================="); 		
		System.out.println("batch sign with seal private key"); 
		BatchSign(baseurl, key, serial);		
	}
	
	
	private static void BatchSign(String baseurl, PrivateKey key, String serial)
	{
		try {
			// Hash to Sign
			byte[] dataToSign1 = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas turpis metus, pharetra sit amet dui et, vestibulum egestas nisl. Morbi non commodo orci. Nulla eget commodo risus, a egestas tortor. Pellentesque nunc sapien, vulputate et ipsum sagittis, blandit elementum leo. Vivamus nec facilisis lectus, non sollicitudin velit. Vestibulum a lacinia justo. Aenean nisi quam, pulvinar vel faucibus a, scelerisque id neque. In in rutrum massa. Mauris quam mauris, lobortis sed commodo vitae, viverra non tortor. Maecenas sit amet neque purus.".getBytes();
			byte[] dataToSign2 = "Vestibulum interdum ac nunc eget placerat. Sed interdum vestibulum feugiat. Donec in turpis tristique, efficitur lectus quis, ornare dolor. Proin volutpat nulla in facilisis suscipit. Sed convallis nisl at iaculis volutpat. Nulla fringilla, felis vel lobortis egestas, nibh ex porta erat, eget luctus elit metus at erat. Duis quis sapien euismod, mattis orci eu, iaculis odio. Curabitur elementum pharetra dui quis laoreet. Mauris in sollicitudin justo. Nulla facilisi.".getBytes();
			byte[] dataToSign3 = "Phasellus fermentum at risus et pharetra. Suspendisse vel posuere sapien. Integer faucibus nec dolor ac dignissim. Aliquam dapibus, magna sed gravida varius, enim lectus dictum justo, sit amet feugiat odio velit sit amet massa. Duis dignissim felis vitae urna tempor, eget facilisis risus sollicitudin. Mauris dignissim velit non libero mattis mollis. Mauris ut nibh metus. Donec mollis euismod est, dictum vestibulum eros dignissim tincidunt.".getBytes();
			byte[] dataToSign4 = "Curabitur ac quam ac odio tempor hendrerit. Sed sed lacus velit. Aliquam finibus tortor tellus, vel maximus erat sodales ut. Cras aliquam justo sed rutrum volutpat. Curabitur et convallis ipsum. Vestibulum tempus, diam vel efficitur faucibus, diam magna dignissim orci, sit amet tempus leo magna sollicitudin orci. Morbi tellus lorem, eleifend eu ligula quis, lacinia ornare diam. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Integer elementum ac sapien vel bibendum. Pellentesque mattis orci vel facilisis convallis. Aliquam sit amet pellentesque neque, ut interdum tortor. Nunc est tellus, rhoncus in tincidunt at, porta eu massa.".getBytes();
			byte[] dataToSign5 = "Pellentesque quis arcu sed quam suscipit convallis. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Suspendisse turpis justo, venenatis a turpis ac, accumsan ultrices nisl. Nunc leo leo, accumsan id metus sed, eleifend blandit neque. Mauris lobortis urna eu massa scelerisque egestas. Proin pulvinar, elit sit amet aliquam faucibus, risus lacus luctus massa, sit amet semper ante metus a tellus. Sed hendrerit velit quis egestas tristique. Praesent placerat iaculis tortor eget dignissim. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Aliquam erat volutpat.".getBytes();
			
			MessageDigest hashAlgo = MessageDigest.getInstance("SHA-256");
			byte[] hashToSign1 = hashAlgo.digest(dataToSign1);
			byte[] hashToSign2 = hashAlgo.digest(dataToSign2);
			byte[] hashToSign3 = hashAlgo.digest(dataToSign3);
			byte[] hashToSign4 = hashAlgo.digest(dataToSign4);
			byte[] hashToSign5 = hashAlgo.digest(dataToSign5);
			

			String data = "[{\"Id\":0,\"Hash\":\"" + new String(Base64.getEncoder().encode(hashToSign1)) + "\"},";
			data += "{\"Id\":1,\"Hash\":\"" + new String(Base64.getEncoder().encode(hashToSign2)) + "\"},";
			data += "{\"Id\":2,\"Hash\":\"" + new String(Base64.getEncoder().encode(hashToSign3)) + "\"},";
			data += "{\"Id\":3,\"Hash\":\"" + new String(Base64.getEncoder().encode(hashToSign4)) + "\"},";
			data += "{\"Id\":4,\"Hash\":\"" + new String(Base64.getEncoder().encode(hashToSign5)) + "\"}]";
			byte[] dataBytes = data.getBytes("UTF-8");
			
			// prepare Request
			Signature sig = Signature.getInstance("SHA256WithRSA");
			sig.initSign(key);
			sig.update(dataBytes);
			byte[] HashSignature = sig.sign();
			
			
			String request = "{\"AuthCert\": null, \"AuthSerial\": \"" + serial + "\", \"ListOfHashes\": \"";
			request += new String(Base64.getEncoder().encode(dataBytes));
			request +="\", \"HashSignature\": \"";
			request += new String(Base64.getEncoder().encode(HashSignature));
			request +="\", \"HashSignatureMechanism\": \"SHA256withRSA\" }";
						
			//System.out.println("request: " + request); 
			
			String postUrl = baseurl + "/Batch/Sign/nosessionid";
			String result = Post(postUrl,request);
			System.out.println("result: " + result); 	
		}
		catch(Exception e) {
			System.out.println("exception: " + e.toString()); 
		}	
	}
	
	private static void Sign(String baseurl, PrivateKey key, String serial)
	{
		try {
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
		catch(Exception e) {
			System.out.println("exception: " + e.toString()); 
		}	
	}
	
	private static void GetSealCertificate(String baseurl, String serial)
	{
		try {		
			// get seal certificate		
			String getUrl = baseurl + "/Certificate/" + serial + "/nosessionid";
			byte[] seal_certificate_raw = Get(getUrl); 
			
			String seal_certificate_base64 = new String(Base64.getEncoder().encode(seal_certificate_raw));
			System.out.println("seal certificate: " + seal_certificate_base64); 
		}
		catch(Exception e) {
			System.out.println("exception: " + e.toString()); 
		}				
	}
	
	
	
	private static String Post(String postUrl, String requestJson) throws Exception
	{
		CloseableHttpClient httpClient = HttpClients.createDefault();
		HttpPost httpPost = new HttpPost(postUrl);	
		httpPost.setHeader("Accept", "application/json");
		httpPost.setHeader("Content-type", "application/json");
		httpPost.setEntity(new ByteArrayEntity(requestJson.getBytes("UTF8")));
	
		CloseableHttpResponse httpResponse = httpClient.execute(httpPost);
		
		//StatusLine sl = httpResponse.getStatusLine();
		//System.out.println("status code= " + Integer.toString(sl.getStatusCode())); 		
		
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




























