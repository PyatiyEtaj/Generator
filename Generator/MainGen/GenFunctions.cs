using System;

namespace Generator
{
    public class GenFunctions
    {
        public enum FuncName
        {
            Rnd,
            GenAE,
            GetAECode
        }

        public static readonly string[] Funcs = {"_rnd", "_genAE", "_getAEcode"};

        public static string Rnd(string a, string b, string type, Random r, string count = "1")
        {
            string str;
                
            if (type == "int")
            {
                Int32 A = Int32.Parse(a), B = Int32.Parse(b);
                str = RndWrapper.NextIMass(A, B, Int32.Parse(count), r);
            }
            else
            {
                Double A = Double.Parse(a.Replace('.', ',')), B = Double.Parse(b.Replace('.', ','));
                str = RndWrapper.NextDMass(A, B, Int32.Parse(count), r);
            }

            return str;
        }
    }
}