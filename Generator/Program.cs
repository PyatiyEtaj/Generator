using System;
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

        static void Main(string[] args)
        {
            External(args);
            //NonExternal();
        }
    }
}