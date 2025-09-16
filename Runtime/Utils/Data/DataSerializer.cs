using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Utils.Data
{
    public static class DataSerializer
    {
        private const char CharKey = 'F';
        
        public static char[] Serialize(string jsonObject)
        {
            var charArray = jsonObject.ToCharArray();
            
            Obfuscate(charArray);
            
            return charArray;
        }
        
        public static string Deserialize(char[] chars)
        {
            var charArray = chars;
            
            Obfuscate(charArray);
          
            return new string(charArray);
        }
        
        private static void Obfuscate(char[] data)
        {
            for (int i = 0; i < data.Length; i++)
                data[i] ^= CharKey; 
        }
    }
}