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

        static void Output((string, string, string) r)
        {
            Gen g = new Gen();
            Console.WriteLine($"\n\n# ШАБЛОННЫЙ_ВИД\n{r.Item1}");
            Console.WriteLine($"# РЕШЕНИЕ\n```\n{r.Item2}\n```");
            Console.WriteLine($"# ТЕСТОВЫЕ_ДАННЫЕ\n__JSON__\n```\n{r.Item3}\n```");
            string json = r.Item3;
            var tests = g.GetTestsFromJson(json);
            StringBuilder str = new StringBuilder("__Готовые данные__\n```\n");
            foreach (var p in tests)
            {
                str.Append($"[\n\tимя теста = {p.Name} | вес = {p.Weight}\n\tтесты = ");
                foreach (var elem in p.Data)
                {
                    str.Append($"{{{elem}}}, ");
                }
                str.Remove(str.Length - 2, 2);
                str.Append("\n]\n");
            }
            Console.WriteLine($"{str}```");
        }
        
        static void External(string[] args)
        {
            if (args.Length > 1)
            {
                var pathToExt = args[0];
                var path = args[1];
                try
                {
                    Gen g = new Gen();
                    var r = g.RunAsync(path);
                    Output(r.Result);
                    
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
                Output(r.Result);
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