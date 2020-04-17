using System;
using System.Security.Cryptography;
using System.Text;

namespace Logic {

    /*
        Autor: Jhonattan Alejandro Pulido Arenas
        Fecha creación: 11/03/2020
        Descripción: Clase que sirve para encriptar claves y tokens
    */
    public class LEncriptar {

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 11/03/2020
            Descripción: Método que sirve para encriptar una cadena de texto
            Recibe: String input - Puede ser una clave, un token, etc.
            Retorna: La cadena encriptada
        */
        public string Encriptar(string input) {

            SHA256CryptoServiceProvider provider = new SHA256CryptoServiceProvider();

            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashedBytes = provider.ComputeHash(inputBytes);

            StringBuilder output = new StringBuilder();

            for (int i = 0; i < hashedBytes.Length; i++)
                output.Append(hashedBytes[i].ToString("x2").ToLower());

            return output.ToString();
        }

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 11/03/2020
            Descripción: Método que sirve para encriptar una cadena de texto
            Recibe: 
            Retorna: Código de verificación
        */
        public string CodigoVerificacion() {

            Guid guid = Guid.NewGuid();

            string codigoVerificacion = guid.ToString();
            codigoVerificacion = codigoVerificacion.Substring(0, 6);

            return codigoVerificacion;
        }
    }
}
