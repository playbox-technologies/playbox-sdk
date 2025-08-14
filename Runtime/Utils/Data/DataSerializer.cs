using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Utils.Data
{
    public static class DataSerializer
    {
        private const byte Key = 0x11;

        public static byte[] Serialize(object obj)
        {
            using var memoryStream = new MemoryStream();
#pragma warning disable SYSLIB0011
            new BinaryFormatter().Serialize(memoryStream, obj);
#pragma warning restore SYSLIB0011
            byte[] data = memoryStream.ToArray();
            Obfuscate(data);
            return data;
        }

        public static T Deserialize<T>(byte[] bytes)
        {
            Obfuscate(bytes);
            using var memoryStream = new MemoryStream(bytes);
#pragma warning disable SYSLIB0011
            return (T)new BinaryFormatter().Deserialize(memoryStream);
#pragma warning restore SYSLIB0011
        }

        private static void Obfuscate(byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
                data[i] ^= Key; 
        }
    }
}