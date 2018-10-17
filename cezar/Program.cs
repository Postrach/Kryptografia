using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace cezar
{
    /* 
        a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z,
        A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z
    */

    class Program
    {
        #region auxiliary functions
        public static void FileWrite(string fName, string data) {
            StreamWriter sw = File.CreateText(fName);

            sw.Write(data);
            sw.Close();
        }

        public static string FileRead(string fName) {
            string data = "";
            FileStream fs = new FileStream(fName,
                FileMode.Open, FileAccess.Read);
            try
            {
                StreamReader sr = new StreamReader(fs);

                while (!sr.EndOfStream)
                {
                    data += sr.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.Write("exception");
            }
            fs.Close();

            return data;
        }

        public static int ReadKey() {
            string temp = FileRead("key.txt");
            string[] tmp = temp.Split(' ');
            int key = Int32.Parse(tmp[0]);

            return key;
        }

        public static int ReadFactor() {
            string temp = FileRead("key.txt");
            string[] tmp = temp.Split(' ');
            int factor = Int32.Parse(tmp[1]);

            return factor;
        }

        public static int GreatestCommonDivisor(int a, int b) {
            while (a != b) {
                if (a > b)
                {
                    a -= b;
                }
                else {
                    b -= a;
                }
            }
            return a;
        }

        #endregion

        public static void CesarEncryption(string text, int key) {
            // in: plain.txt, key.txt
            // out: crypto.txt

            char[] textChars = text.ToCharArray();
            string encrypted = "";
            char letter;

            foreach (char c in textChars) {
                if (char.IsLetter(c))
                {
                    if (char.IsUpper(c))
                    {
                        letter = 'A';
                    }
                    else
                    {
                        letter = 'a';
                    }
                    encrypted += (char)((((c + key) - letter) % 26) + letter);
                }
                else {
                    encrypted += c;
                }
            }
            FileWrite("crypto.txt", encrypted);
            Console.WriteLine("File encrypted");
        }

        public static string CesarDecryption(string text, int key) {
            // in: crypto.txt, key.txt
            // out: decrypt.txt

            key = 26 - key;
            char[] textChars = text.ToCharArray();
            string decrypted = "";
            char letter;

            foreach (char c in textChars)
            {
                if (char.IsLetter(c))
                {
                    if (char.IsUpper(c))
                    {
                        letter = 'A';
                    }
                    else
                    {
                        letter = 'a';
                    }
                    decrypted += (char)((((c + key) - letter) % 26) + letter);
                }
                else
                {
                    decrypted += c;
                }
            }
            FileWrite("decrypt.txt", decrypted);
            Console.WriteLine("File decrypted");

            return decrypted;
        }

        public static void CesarCryptoanalisisWithExtra(string text, string extra) {
            //in: crypto.txt, extra.txt
            //out: key-new.txt, decrypt.txt

            char[] textChars = text.ToCharArray();
            int key = 0;
            int index = 0;

            foreach (char c in textChars)
            {
                if (char.IsLetter(c))
                {
                    key = ((int)c - extra[index] + 26) % 26;
                    break;
                }
                index++;
            }
            CesarDecryption(text, key);
            FileWrite("key-new.txt", key.ToString());
        }

        public static void CesarCryptoanalisisWithTextOnly(string text) {
            //in: crypto.txt
            //out: decrypt.txt

            string decrypted = "";
            for (int i = 0; i < 26; i++)
            {
                decrypted += "\r\n\r\n" + CesarDecryption(text, i);
            }

            FileWrite("decrypt.txt", decrypted);
        }

        public static void AffineEncryption(string text, int key, int factor) {
            // in: plain.txt, key.txt
            // out: crypto.txt

            char[] textChars = text.ToCharArray();
            string encrypted = "";
            char letter;

            if (GreatestCommonDivisor(key, 26) == 1)
            {
                foreach (char c in textChars)
                {
                    if (char.IsLetter(c))
                    {
                        if (char.IsUpper(c))
                        {
                            letter = 'A';
                        }
                        else
                        {
                            letter = 'a';
                        }
                        encrypted += (char)((((c * key + factor) - letter) % 26) + letter);
                    }
                    else
                    {
                        encrypted += c;
                    }
                }
                FileWrite("crypto.txt", encrypted);
                Console.WriteLine("File encrypted");
            }
        }

        public static void AffineDecryption(string text, int key, int factor) {
            // in: crypto.txt, key.txt
            // out: decrypt.txt
            
            char[] textChars = text.ToCharArray();
            string decrypted = "";
            char letter;
            int newKey = -1;

            for (int i = 1; i < 26; i++)
            {
                if ((key * i) % 26 == 1)
                {
                    newKey = i;
                }
            }

            if (newKey == -1) {
                Console.WriteLine("Błędny klucz.");
            }

            foreach (char c in textChars)
            {
                if (char.IsLetter(c))
                {
                    if (char.IsUpper(c))
                    {
                        letter = 'A';
                    }
                    else
                    {
                        letter = 'a';
                    }
                    decrypted += (char)((((newKey * (c-factor)) - letter) % 26) + letter);
                }
                else
                {
                    decrypted += c;
                }
            }
            FileWrite("decrypt.txt", decrypted);
        }

        static void Main(string[] args)
        {
            try
            {
                switch (args[0])
                {
                    case "-c":
                        Console.Write("szyfr Cezara, ");
                        switch (args[1])
                        {
                            case "-e":
                                Console.Write("szyfrowanie\n");
                                CesarEncryption(FileRead("plain.txt"), ReadKey());
                                break;
                            case "-d":
                                Console.Write("odszyfrowywanie\n");
                                CesarDecryption(FileRead("crypto.txt"), ReadKey());
                                break;
                            case "-j":
                                Console.Write("kryptoanaliza z tekstem jawnym\n");
                                CesarCryptoanalisisWithExtra(FileRead("crypto.txt"), FileRead("extra.txt"));
                                break;
                            case "-k":
                                Console.Write("kryptoanaliza wyłącznie w oparciu o kryptogram\n");
                                CesarCryptoanalisisWithTextOnly(FileRead("crypto.txt"));
                                break;
                            default:
                                break;
                        }
                        break;
                    case "-a":
                        Console.Write("szyfr afiniczny, ");
                        switch (args[1])
                        {
                            case "-e":
                                Console.Write("szyfrowanie\n");
                                AffineEncryption(FileRead("plain.txt"), ReadKey(), ReadFactor());
                                break;
                            case "-d":
                                Console.Write("odszyfrowywanie\n");
                                AffineDecryption(FileRead("crypto.txt"), ReadKey(), ReadFactor());
                                break;
                            case "-j":
                                Console.Write("kryptoanaliza z tekstem jawnym\n");
                                break;
                            case "-k":
                                Console.Write("kryptoanaliza wyłącznie w oparciu o kryptogram\n");
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (System.IndexOutOfRangeException e)
            {

                Console.Write("Invalid options.");
            }
            Console.ReadKey();
        }
        
    }
}
