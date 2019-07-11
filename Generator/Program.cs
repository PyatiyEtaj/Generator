using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Generator.MainGen;
using Generator.MainGen.Parametr;
using Generator.Parsing;

namespace Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"C:\Users\kampukter\source\repos\\Generator\Generator\Docs\Exp.txt";
            //Console.WriteLine(new System.Uri(path).AbsoluteUri);
            Console.WriteLine(@"file:///C:/Users/kampukter/source/repos/ALS/ALS.GeneratorModule/Docs/Exp.txt");
            //for (int i = 0; i < 100000; i++)
            try
            {
                //string[] arg = { @"C:\Users\kampukter\source\repos\Generator\Generator\bin\Debug\netcoreapp2.1\Exp.txt", "1.txt", "2.txt", "3.txt" };
                string arg = "Exp.txt";
                Gen g = new Gen(new Parser(), new ParamsContainer());
                GenFunctions gf = new GenFunctions();
                var result = g.Run(arg);
                Console.WriteLine(result.Result.Code);
                Console.WriteLine(result.Result.Template + "\n");
                Console.WriteLine(result.Result.Tests + "\n");
                Console.WriteLine();
                var t = gf.GetTestsFromJson(result.Result.Tests);
                foreach(var ls in t)
                {
                    foreach(var s in ls)
                    {
                        Console.Write(s + " | ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("Успешное завершение работы!");
                Console.WriteLine(new Uri(result.Result.Code).AbsolutePath);
                if (System.IO.File.Exists(new Uri(result.Result.Code).AbsolutePath))
                {
                    Console.WriteLine("NICE");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\n\nЧто-то пошло не так, возможно файл-шаблон содержит ошибки! Завершение работы...\nError = " + e);
                using (StreamWriter sw = new StreamWriter("ERROR.txt", false, Encoding.UTF8))
                {
                    sw.WriteLine(e);
                }
            }
            Console.ReadKey();
        }
    }
}