using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generator.MainGen;
using Generator.MainGen.Structs;
using Generator.Parsing;
using NLua;
namespace Generator
{
    class Program
    {
        
        static void External(string[] args)
        {
            if (args.Length > 1)
            {
                var pathToOut = args[0];
                var path = args[1];
                try
                {
                    Gen g = new Gen();
                    var r = g.RunAsync(path);

                    Console.WriteLine($"\n\n# ШАБЛОННЫЙ_ВИД\n{r.Result.Item1}");
                    Console.WriteLine($"# РЕШЕНИЕ\n```\n{r.Result.Item2}\n```");
                    Console.WriteLine($"# ТЕСТОВЫЕ_ДАННЫЕ\n```\n{r.Result.Item3}\n```");
                    StringBuilder str = new StringBuilder("# ТЕСТОВЫЕ_ДАННЫЕ_INTERPRET\n```\n");
                    foreach (var p in g.GetTestsFromJson(r.Result.Item3))
                    {
                        str.Append(p);
                        str.Append("\n");
                    }
                    Console.WriteLine($"{str}```");
                }
                catch (Exception e)
                {
                    Console.WriteLine("\n\n## Что-то пошло не так, возможно файл-шаблон содержит ошибки! Завершение работы...\nError = " + e.Message);
                }
            }
        }

        static void NonExternal(string[] args)
        {
            try
            {
                //string arg = $"tasks/tempalate.gentemp";
                string arg = $"tasks/test_table.gentemp";

                Gen g = new Gen();
                var r = g.RunAsync(arg);

                var tests = g.GetTestsFromJson(r.Result.Item3);
                Console.WriteLine($"\n\n# ШАБЛОННЫЙ_ВИД\n{r.Result.Item1}");
                Console.WriteLine($"# РЕШЕНИЕ\n```\n{r.Result.Item2}\n```");
                Console.WriteLine($"# ТЕСТОВЫЕ_ДАННЫЕ\n```\n{r.Result.Item3}\n```");
                StringBuilder str = new StringBuilder("# ТЕСТОВЫЕ_ДАННЫЕ_INTERPRET\n```\n");
                //Console.WriteLine($"# ТЕСТОВЫЕ_ДАННЫЕ_INTERPRET\n```\n{g.GetTestsFromJson(r.Result.Item3)}\n```");
                foreach (var p in g.GetTestsFromJson(r.Result.Item3))
                {
                    str.Append(p);
                    str.Append("\n");
                }
                Console.WriteLine($"{str}```");
            }
            catch (Exception e)
            {
                Console.WriteLine("\n\n## Что-то пошло не так, возможно файл-шаблон содержит ошибки! Завершение работы...\nError = " + e.Message);
            }
        }

        static void Main(string[] args)
        {
            //temp();
            External(args);
            //NonExternal(args);
            //Console.WriteLine((char)254);
            //Console.ReadKey();
        }
    }
}