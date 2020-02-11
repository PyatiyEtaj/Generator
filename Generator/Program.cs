using System;
using System.IO;
using System.Text;
using Generator.MainGen;
using Generator.MainGen.Parametr;
using Generator.Parsing;

namespace Generator
{
    class Program
    {
        static void RunTest()
        {
            string arg = "Exp.txt";
            System.Diagnostics.Stopwatch myStopwatch = new System.Diagnostics.Stopwatch();
            myStopwatch.Start();

            for (int i = 0; i < 100; i++)
            {
                Gen g = new Gen(new Parser(), new ParamsContainer());
                GenFunctions gf = new GenFunctions();
                var result = g.Run(arg);
            }

            myStopwatch.Stop();
            Console.WriteLine($"время работы: { myStopwatch.ElapsedMilliseconds}");
        }


        static void Main(string[] args)
        {
            /*RunTest();
            Console.ReadLine();
            return;*/

            var path = @"C:\Users\kampukter\source\repos\Generator\Generator\Docs\Exp.txt";
            //for (int i = 0; i < 100000; i++)
            try
            {
                int lr = 2;
                //string[] arg = { @"C:\Users\kampukter\source\repos\Generator\Generator\bin\Debug\netcoreapp2.1\Exp.txt", "1.txt", "2.txt", "3.txt" };
                string arg = $"tasks/t{lr}.txt";
                Gen g = new Gen(new Parser(), new ParamsContainer());
                GenFunctions gf = new GenFunctions();
                var result = g.Run(arg, lr, 1, true);
                //Console.WriteLine(result.Result.Code + "\n----------------------\n");
                Console.WriteLine(result.Result.Template + "\n----------------------\n");
                //Console.WriteLine(result.Result.Tests + "\n----------------------\n");
                Console.WriteLine();
                /*var t = gf.GetTestsFromJson(result.Result.Tests);
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
                }*/
            }
            catch (Exception e)
            {
                Console.WriteLine("\n\nЧто-то пошло не так, возможно файл-шаблон содержит ошибки! Завершение работы...\nError = " + e.Message);
                using (StreamWriter sw = new StreamWriter("ERROR.txt", false, Encoding.UTF8))
                {
                    sw.WriteLine(e.Message);
                }
            }
            Console.ReadKey();
        }
    }
}