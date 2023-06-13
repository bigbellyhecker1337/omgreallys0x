using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Threading;

namespace Gayauth.Patcher
{

    internal class Patch
    {

        [HarmonyPatch(typeof(Encoding), nameof(Encoding.Default.GetBytes), new[] { typeof(string) })]

       public class patch
        {

         #region utils

            public static bool Format(string inputString)
            {
                byte[] hashBytes;
                bool correctformat = false;
                try
                {
                    hashBytes = HexStringToByteArray(inputString);
                }
                catch (FormatException)
                { 
                    return false;
                }
                if (hashBytes.Length == 32)
                {
                    correctformat = true;
                }

                return correctformat;
            }

            private static byte[] HexStringToByteArray(string input)
            {
                int numberChars = input.Length;
                byte[] bytes = new byte[numberChars / 2];
                for (int i = 0; i < numberChars; i += 2)
                {
                    bytes[i / 2] = Convert.ToByte(input.Substring(i, 2), 16);
                }
                return bytes;
            }

            #endregion


            static List<string> all = new List<string>();
            static int nameindex = 0;
            static int ownerindex = 0;
            static bool secret_finded = false;
            static bool name_finded = false;
            static bool owner_finded = false;


            public static bool Prefix(ref string s)
            {
                all.Add(s);
             

                for (int i = 0; i < all.Count; i++)
                {


                    if (all[i].Length == 64 && Format(all[i]) == true && secret_finded == false)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Their Secret Key: " + all[i] + " Replaced to: " + Program.secret);
                        Console.ResetColor();
                        Program.findedsecret = all[i];

                        secret_finded = true;
                    }





                    if (name_finded == false)
                    {
                            if (all[i] == "name")
                            {
                                nameindex = i;
                            }


                            if (i == nameindex + 1 && nameindex != 0)
                            {
                                if (Format(all[i]) == false)
                                {
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    Console.WriteLine("Their Name: " + all[i] + " Replaced to: " + Program.appname);
                                    Console.ResetColor();
                                    Program.findedname = all[i];
                                    name_finded = true;
                                }

                            }
                    }






                    if (owner_finded == false)
                    {
                        if (all[i] == "ownerid")
                        {
                            ownerindex = i;
                        }


                        if (i == ownerindex + 1 && ownerindex != 0)
                        {
                            if (Format(all[i]) == false)
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine("Their Owner ID: " + all[i] + " Replaced to: " + Program.ownerid);
                                Console.ResetColor();
                                Program.findedownerid = all[i];
                                owner_finded = true;
                            }
                        }
                    }



                }

                if (s == Program.findedname)
                {
                    s = Program.appname;
                }
                if (s == Program.findedownerid)
                {
                    s = Program.ownerid;
                }
                if (s == Program.findedsecret)
                {
                    s = Program.secret;
                }

                return true;
            }

        }
    }

}
