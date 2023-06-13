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
using System.Collections.Specialized;
using System.Diagnostics;

namespace Gayauth.Patcher
{

    internal class NVC
    {
        public static bool signature = false;
        static int rate = 0;


        [HarmonyPatch(typeof(NameValueCollection), nameof(NameValueCollection.Get), new[] { typeof(int) })]
        public class nvcget
        {

            static bool Prefix(int index)
            {
                if (index == 0)
                {
                    signature = false;
                    rate = 0;
                    signature = true;
                }
             
                return true;
            }

        }



        [HarmonyPatch(typeof(ProcessStartInfo), MethodType.Constructor, new[] { typeof(string), typeof(string) })]
        public class psi
        {

            static bool Prefix(ref string fileName, ref string arguments)
            {
                if (fileName == "cmd.exe" && arguments.Contains("Signature checksum failed"))
                {
                    arguments = $"/c start cmd /C \"color a && title big belly hecker && echo patched checks! && timeout /t 3\"";
                    return true;
                }
                else
                {
                    return true;
                }
             
            }

        }




        [HarmonyPatch(typeof(Environment), nameof(Environment.Exit), new[] { typeof(int) })]
        public class index
        {

           static bool Prefix(int exitCode)
            {
                if (exitCode == 0 && signature == true)
                {
                    if (rate == 2)
                    {
                        signature = false;
                        rate = 0;
                        return true;
                    }
                    else
                    {
                        if (rate < 2)
                        {
                            rate++;
                        }

                        return false;
                    }
               
                }
                else
                {
                    return true;
                }
             
            }

        }
    }

}