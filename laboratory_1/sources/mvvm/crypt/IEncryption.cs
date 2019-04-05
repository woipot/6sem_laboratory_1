using System;
using System.Security.Cryptography;

namespace laboratory_1.sources.mvvm.crypt
{
    public interface IEncryption
    {
        void Decrypt(string fromFile, string toFile);

        void Encrypt(string fromFile, string toFile);

        long GetCurrentProgressBytes();

        long GetMaximum(string fileName);

        event EventHandler<int> ProgreeUpdated;
    }
}
