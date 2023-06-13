using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Gayauth.Patcher;

namespace Gayauth.Patcher
{
    internal class Program
    {
        public static string appname = "";
        public static string ownerid = "";
        public static string secret = "";

        public static Harmony patch = new Harmony("big belly hacker 1337");

        public static string findedname = "";
        public static string findedownerid = "";
        public static string findedsecret = "";

        static void Main(string[] args)
        {
            Console.Title = "big belly hacker 1337";
            Console.ForegroundColor = ConsoleColor.Green;

            bool yesSex = false;

            Console.WriteLine("[1] Use saved Keyauth credentials");
            Console.WriteLine("[2] Enter new Keyauth credentials");
            Console.Write("Make your choice: ");

            string choice = Console.ReadLine();
            Console.WriteLine();

            if (choice == "1")
            {
                if (File.Exists("gayauth.txt"))
                {
                    string[] noSex = File.ReadAllLines("gayauth.txt");  
                    if (noSex.Length == 3)
                    {
                        appname = noSex[0];
                        ownerid = noSex[1];
                        secret = noSex[2];
                        yesSex = true;
                    }
                }

                if (!yesSex)
                {
                    Console.WriteLine("No saved data found.");
                    Console.ReadLine();
                    return;
                }
            }
            else if (choice == "2")
            {
                Console.Write("App Name: ");
                appname = Console.ReadLine();
                Console.Write("Owner ID: ");
                ownerid = Console.ReadLine();
                Console.Write("Secret: ");
                secret = Console.ReadLine();

                File.WriteAllText("gayauth.txt", $"{appname}\n{ownerid}\n{secret}");
            }
            else
            {
                Console.WriteLine("Invalid choice.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Loaded Keyauth credentials");
            Console.WriteLine();

            var assembly = Assembly.LoadFile(Path.GetFullPath(args[0]));
            var paraminfo = assembly.EntryPoint.GetParameters();
            object[] parameters = new object[paraminfo.Length];
            patch.PatchAll(Assembly.GetExecutingAssembly());
            assembly.EntryPoint.Invoke(null, parameters);
        }
    }
}
