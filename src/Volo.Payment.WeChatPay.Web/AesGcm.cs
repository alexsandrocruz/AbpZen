using System;
using System.Text;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace Volo.Payment.WeChatPay;

public class AesGcm
{
    private static string ALGORITHM = "AES/GCM/NoPadding";
    private static int TAG_LENGTH_BIT = 128;
    private static int NONCE_LENGTH_BYTE = 12;

    public static string AesGcmDecrypt(string associatedData, string nonce, string ciphertext, string aesKey)
    {
        var gcmBlockCipher = new GcmBlockCipher(new AesEngine());
        var aeadParameters = new AeadParameters(
            new KeyParameter(Encoding.UTF8.GetBytes(aesKey)), 
            128, 
            Encoding.UTF8.GetBytes(nonce), 
            Encoding.UTF8.GetBytes(associatedData));
        gcmBlockCipher.Init(false, aeadParameters);

        var data = Convert.FromBase64String(ciphertext);
        var plaintext = new byte[gcmBlockCipher.GetOutputSize(data.Length)];
        var length = gcmBlockCipher.ProcessBytes(data, 0, data.Length, plaintext, 0);
        gcmBlockCipher.DoFinal(plaintext, length);
        return Encoding.UTF8.GetString(plaintext);
    }
}