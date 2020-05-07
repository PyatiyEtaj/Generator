using System;
using System.Linq;
using Generator.MainGen;
using Generator.MainGen.Parametr;
using Generator.Parsing;

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
                    int lr = 1;
                    string arg = $"tasks/test.gentemp";
                    Gen g = new Gen(new Parser(), new ParamsContainer());
                    GenFunctions gf = new GenFunctions();
                    var result = g.Run(path, lr, 1, false, true);

                    Console.WriteLine($"\n\n# ШАБЛОННЫЙ_ВИД\n{result.Result.Template}");
                    Console.WriteLine($"# РЕШЕНИЕ\n```\n{result.Result.Code}\n```");
                    Console.WriteLine($"# ТЕСТОВЫЕ_ДАННЫЕ\n```\n{result.Result.Tests}\n```");
                }
                catch (Exception e)
                {
                    Console.WriteLine("\n\n## Что-то пошло не так, возможно файл-шаблон содержит ошибки! Завершение работы...\nError = " + e.Message);
                }
            }
        }

        static void NonExternal()
        {
            int lr = 1;
            string arg = $"tasks/test.gentemp";
            Gen g = new Gen(new Parser(), new ParamsContainer());
            GenFunctions gf = new GenFunctions();
            var result = g.Run(arg, lr, 1, false, true);

            Console.WriteLine($"\n\n# ШАБЛОННЫЙ_ВИД\n{result.Result.Template}");
            Console.WriteLine($"# РЕШЕНИЕ\n```\n{result.Result.Code}\n```");
            Console.WriteLine($"# ТЕСТОВЫЕ_ДАННЫЕ\n```\n{result.Result.Tests}\n```");
            Console.ReadKey();
        }

        static void temp()
        {
            var r = new GenFunctions().GetTestsFromJsonNewVersion("[{\"Name\":\"Тест1\",\"Data\":[\"#tasks/rnd_arr.rnd_array(10, 10, 100)\"]},{\"Name\":\"Тест2\",\"Data\":[\"1\",\"2\",\"3\",\"4\",\"5\",\"6\",\"7\",\"8\",\"9\",\"10\"]}]");
            foreach (var item in r)
            {
                Console.WriteLine(item);
            }
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            //temp();
            External(args);
            //NonExternal();
            
        }
    }
}