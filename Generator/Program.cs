using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator
{
    class Program
    {
        static void Out(Tree tree)
        {
            if (tree == null) return;
            Console.WriteLine(tree.Value + "|" + tree.State.ToString());
            if (tree.Left != null)
            {
                Console.Write("L-");
                Out(tree.Left);
            }

            if (tree.Right != null)
            {
                Console.Write("R-");
                Out(tree.Right);   
            }
        }
        
        static void Main(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("Должно быть 3 аргумента {имя входного файла-шаблона} {имя выходного файла с заданием} {имя выходного файла с кодом} {имя выходного файла с тестовыми данными}");
                return;
            }

            try
            {
                Gen g = new Gen();
                g.Run(args[0]);
    
                using (StreamWriter sw = new StreamWriter(args[1], false, Encoding.UTF8))
                {
                    sw.WriteLine(g.Template);
                }
                
                if (args.Length < 3) return;
                
                using (StreamWriter sw = new StreamWriter(args[2], false, Encoding.UTF8))
                {
                    sw.WriteLine(g.Code);
                }
                
                using (StreamWriter sw = new StreamWriter(args[3], false, Encoding.UTF8))
                {
                    foreach (var t in g.Tests)
                    {
                        sw.WriteLine(t.First + " : " + t.Second);
                    }
                }
                Console.WriteLine("Успешное завершение работы!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Входной файл-шаблон содержит ошибки! Завершение работы...");
                using (StreamWriter sw = new StreamWriter("ERROR", false, Encoding.UTF8))
                {
                    sw.WriteLine(e);
                }
            }
            /*

            Gen g = new Gen();
            g.Run("doc");
            Console.WriteLine();
            Console.WriteLine(g.Template);
            Console.WriteLine("---------------");
            Console.WriteLine(g.Code);
            Console.WriteLine("---------------");
            foreach (var t in g.Tests)
            {
                Console.WriteLine(t.First + " : " + t.Second);
            }*/
        }
    }
}