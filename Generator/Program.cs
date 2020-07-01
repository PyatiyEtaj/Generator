using System;
using System.Collections.Generic;
using System.IO;
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

        static void Output(Gen g, (string, string, string) r)
        {
            Console.WriteLine($"\n\n# ШАБЛОННЫЙ_ВИД\n{r.Item1}");
            Console.WriteLine($"# РЕШЕНИЕ\n```\n{r.Item2}\n```");
            Console.WriteLine($"# ТЕСТОВЫЕ_ДАННЫЕ\n__JSON__\n```\n{r.Item3}\n```");
            string json = r.Item3;
            var tests = g.GetTestsFromJson(json);
            StringBuilder str = new StringBuilder("__Готовые данные__\n```\n");
            foreach (var p in tests)
            {
                str.Append($"[\n\tимя теста = {p.Name} | вес = {p.Weight}\n\tтесты = ");
                var length = p.Data.Count > 100 ? 100 : p.Data.Count;
                for (int i = 0; i < p.Data.Count && i < 100; i++)
                {
                    var elem = p.Data.ElementAt(i);
                    str.Append($"{{{elem}}}, ");
                }
                if (p.Data.Count > 100)
                    str.Append("{...}  ");
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
                var templateDir = Path.GetDirectoryName(path);
                try
                {
                    Gen g = new Gen(templateDir);
                    var r = g.RunAsync(path);
                    Output(g ,r.Result);
                    
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
                string path = @"C:/Users/kampukter/source/repos/Generator/Generator/bin/Debug/netcoreapp3.0/tasks/inst.gentemp";
                var templateDir = Path.GetDirectoryName(path);//$"tasks/test_table.gentemp";
                Gen g = new Gen(templateDir);
                var r = g.RunAsync(path);
                Output(g, r.Result);
            }
            catch (Exception e)
            {
                Console.WriteLine("\n\n## Что-то пошло не так, возможно файл-шаблон содержит ошибки! Завершение работы...\nError = " + e.Message);
            }
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            External(args);
            //NonExternal(args);
        }
    }
}