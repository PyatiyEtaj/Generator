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

            var path = "NULL_PATH";
            if (args.Length > 0)
                path = args[0];
            try
            {
                int lr = 5;
                //string[] arg = { @"C:\Users\kampukter\source\repos\Generator\Generator\bin\Debug\netcoreapp2.1\Exp.txt", "1.txt", "2.txt", "3.txt" };
                string arg = $"tasks/test.gentemp";
                Gen g = new Gen(new Parser(), new ParamsContainer());
                GenFunctions gf = new GenFunctions();
                var result = g.Run(path, lr, 1, false, false);

                Console.WriteLine($"\n\n# ШАБЛОННЫЙ_ВИД\n{result.Result.Template}");
                Console.WriteLine($"# РЕШЕНИЕ\n```\n{result.Result.Code}\n```");
                Console.WriteLine($"# ТЕСТОВЫЕ_ДАННЫЕ\n```\n{result.Result.Tests}\n```");

                /*Console.WriteLine($"\n----------------------\nШАБЛОННЫЙ_ВИД\n\n{result.Result.Template}\n----------------------");
                Console.WriteLine($"РЕШЕНИЕ\n\n```{result.Result.Code}```\n----------------------");
                Console.WriteLine($"ТЕСТОВЫЕ_ДАННЫЕ\n\n```{result.Result.Tests}```\n----------------------");*/

                /*using (StreamWriter sw = new StreamWriter(path + ".result_gen_template.txt", false, Encoding.UTF8))
                {
                    sw.WriteLine($"----------------------\nШАБЛОННЫЙ_ВИД\n\n{result.Result.Template}\n----------------------");
                    sw.WriteLine($"РЕШЕНИЕ\n\n{result.Result.Code}\n----------------------");
                    sw.WriteLine($"ТЕСТОВЫЕ_ДАННЫЕ\n\n{result.Result.Tests}\n----------------------");
                }*/
                //Console.WriteLine();
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
                Console.WriteLine("\n\n## Что-то пошло не так, возможно файл-шаблон содержит ошибки! Завершение работы...\nError = " + e.Message);
                /*using (StreamWriter sw = new StreamWriter(path + ".result_gen_template.txt", false, Encoding.UTF8))
                {
                    sw.WriteLine("Что-то пошло не так, возможно файл-шаблон содержит ошибки! Завершение работы...\nError = " + e.Message);
                }*/
            }
            //Console.ReadKey();
        }
    }
}