using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApplication
{
    public class MyEncrypt
    {
        private static byte[] iv = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
        public static string SHA1(string content)
        {
            return SHA1(content, Encoding.UTF8);
        }

        public static string SHA1(string content, Encoding encode)
        {
            try
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                byte[] bytes_in = encode.GetBytes(content);
                byte[] bytes_out = sha1.ComputeHash(bytes_in);
                sha1.Dispose();
                string result = BitConverter.ToString(bytes_out);
                result = result.Replace("-", "");
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("SHA1加密出错：" + ex.Message);
            }
        }

        public static string EncryptDES(string encryptString, string encryptKey)
        {
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey);
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, iv), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();

            return Convert.ToBase64String(mStream.ToArray());
        }

        public static string DecryptDES(string decryptString, string decryptKey)
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, iv), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }

        public static string[] GenerateKey()
        {
            string[] result = new string[2];
            if (!File.Exists(MyKeys.MYDIRECTORY + "publicKeyString.txt"))
            {
                //生成密钥对  
                Console.WriteLine("HaHa");
                RsaKeyPairGenerator rsaKeyPairGenerator = new RsaKeyPairGenerator();
                RsaKeyGenerationParameters rsaKeyGenerationParameters = new RsaKeyGenerationParameters(BigInteger.ValueOf(3), new Org.BouncyCastle.Security.SecureRandom(), 1024, 25);
                rsaKeyPairGenerator.Init(rsaKeyGenerationParameters);//初始化参数  
                AsymmetricCipherKeyPair keyPair = rsaKeyPairGenerator.GenerateKeyPair();
                AsymmetricKeyParameter publicKey = keyPair.Public;//公钥  
                AsymmetricKeyParameter privateKey = keyPair.Private;//私钥  

                SubjectPublicKeyInfo subjectPublicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey);
                PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey);

                Asn1Object asn1ObjectPublic = subjectPublicKeyInfo.ToAsn1Object();
                byte[] publicInfoByte = asn1ObjectPublic.GetEncoded();
                Asn1Object asn1ObjectPrivate = privateKeyInfo.ToAsn1Object();
                byte[] privateInfoByte = asn1ObjectPrivate.GetEncoded();

                string publicKeyString = Convert.ToBase64String(publicInfoByte);
                string privateKeyString = Convert.ToBase64String(privateInfoByte);

                File.WriteAllText(MyKeys.MYDIRECTORY + "publicKeyString.txt", publicKeyString, Encoding.UTF8);
                File.WriteAllText(MyKeys.MYDIRECTORY + "privateKeyString.txt", privateKeyString, Encoding.UTF8);

                result[0] = publicKeyString;
                result[1] = privateKeyString;
            }
            else
            {
                string publicKeyString = File.ReadAllText(MyKeys.MYDIRECTORY + "publicKeyString.txt");
                string privateKeyString = File.ReadAllText(MyKeys.MYDIRECTORY + "privateKeyString.txt");

                result[0] = publicKeyString;
                result[1] = privateKeyString;
            }
            return result;
        }

        public static string EncryptRSA(string encryptString, string encryptKey, int model)
        {
            AsymmetricKeyParameter encKey;
            if (model == 1)
                encKey = PublicKeyFactory.CreateKey(Convert.FromBase64String(encryptKey));
            else
                encKey = PrivateKeyFactory.CreateKey(Convert.FromBase64String(encryptKey));
            IAsymmetricBlockCipher cipher = new RsaEngine();
            cipher.Init(true, encKey);//true表示加密  

            byte[] encryptData = cipher.ProcessBlock(Encoding.UTF8.GetBytes(encryptString), 0, Encoding.UTF8.GetBytes(encryptString).Length);
            return Convert.ToBase64String(encryptData);
        }

        public static string DecryptRSA(string decryptString, string decryptKey, int model)
        {
            AsymmetricKeyParameter decKey;
            if (model == 1)
                decKey = PublicKeyFactory.CreateKey(Convert.FromBase64String(decryptKey));
            else
                decKey = PrivateKeyFactory.CreateKey(Convert.FromBase64String(decryptKey));

            byte[] encryptData = Convert.FromBase64String(decryptString);
            IAsymmetricBlockCipher cipher = new RsaEngine();

            cipher.Init(false, decKey);//false表示解密  
            string decryptData = Encoding.UTF8.GetString(cipher.ProcessBlock(encryptData, 0, encryptData.Length));
            return decryptData;
        }
    }
}
