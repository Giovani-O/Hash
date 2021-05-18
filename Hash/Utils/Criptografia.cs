using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Hash.Utils
{
    class Criptografia
    {
        public static string GerarMD5(string entrada)
        {
            MD5 md5 = MD5.Create();

            // Calcula o Hash da entrada, dentro de um array de bytes
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(entrada));

            // Construção da string a partir do array
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(data[i].ToString("x2"));
            }

            return builder.ToString();
        }

    }
}
