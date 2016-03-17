using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System;	

public class Crypt : MonoBehaviour {
	
	public static string password1="AndyKillsChicken";
	public static string password2="SecretPassword";
	static int lC=1;
	static string str2;
	
	
	public static string Encrypt(string plainText, string password,
             string salt = "Kosher", string hashAlgorithm = "SHA1",
           int passwordIterations = 2, string initialVector = "OFRna73m*aze01xY",
            int keySize = 256)
        {
            if (string.IsNullOrEmpty(plainText))
                return "";

            byte[] initialVectorBytes = Encoding.ASCII.GetBytes(initialVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(salt);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            PasswordDeriveBytes derivedPassword = new PasswordDeriveBytes (password, saltValueBytes, hashAlgorithm, passwordIterations);
            byte[] keyBytes = derivedPassword.GetBytes(keySize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;

            byte[] cipherTextBytes = null;

            using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initialVectorBytes))
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        cipherTextBytes = memStream.ToArray();
                        memStream.Close();
                        cryptoStream.Close();
                    }
                }
            }

            symmetricKey.Clear();
            return Convert.ToBase64String(cipherTextBytes);
        }
	
	public static string Decrypt(string cipherText, string password,
   							string salt = "Kosher", string hashAlgorithm = "SHA1",
   								int passwordIterations = 2, string initialVector = "OFRna73m*aze01xY",
    																					int keySize = 256)
	{
		    if (string.IsNullOrEmpty(cipherText))
		        return "";
		
		    byte[] initialVectorBytes = Encoding.ASCII.GetBytes(initialVector);
		    byte[] saltValueBytes = Encoding.ASCII.GetBytes(salt);
		    byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
		
		    PasswordDeriveBytes derivedPassword = new PasswordDeriveBytes(password, saltValueBytes, hashAlgorithm, passwordIterations);
		    byte[] keyBytes = derivedPassword.GetBytes(keySize / 8);
		
		    RijndaelManaged symmetricKey = new RijndaelManaged();
		    symmetricKey.Mode = CipherMode.CBC;
		
		    byte[] plainTextBytes = new byte[cipherTextBytes.Length];
		    int byteCount = 0;
		
		    using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initialVectorBytes))
		    {
		        using (MemoryStream memStream = new MemoryStream(cipherTextBytes))
		        {
		            using (CryptoStream cryptoStream = new CryptoStream(memStream, decryptor, CryptoStreamMode.Read))
		            {
		                byteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
		                memStream.Close();
		                cryptoStream.Close();
		            }
		        }
		    }
		
		    symmetricKey.Clear();
		    return Encoding.UTF8.GetString(plainTextBytes, 0, byteCount);
	}
	
	public static int lineCount(string p){
		
		System.IO.StreamReader file = new System.IO.StreamReader(p);
			str2 = Crypt.Decrypt(file.ReadToEnd(),password1,password2,"SHA1",2,"16CHARSLONG12345",256);
			foreach(char c in str2){
				if(c=='\n'){
					lC++;	
				}
			}
			if(lC==0){
				lC=1;	
			}
			file.Close();
		
		return lC;
	}
}
