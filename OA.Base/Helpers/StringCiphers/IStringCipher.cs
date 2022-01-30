using System;
using System.Collections.Generic;
using System.Text;

namespace OA.Base.Helpers.StringCiphers
{
    public interface IStringCipher
    {
        string Encrypt(string inputText);
        string Decrypt(string encryptText);
    }
}
