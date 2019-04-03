using System.Collections.Generic;

namespace Generator
{
    class Elems
    {
        /*public enum FuncName
        {
            Sin, Cos, Sqrt, Tg, Log, Ln
        }
        public enum SignName
        {
            Sub, Add, Div, Mul, Deg
        }*/
        public static string[] Funcs = {"sin", "cos", "sqrt", "tg", "log", "ln" };
        public static string[] Signs = {"-", "+", "/", "*", "^" };

        public static void SetFuncs(List<string> funcs)
        {
            Funcs = new string[funcs.Count]; 
            for (int i = 0 ; i < funcs.Count; i++)
            {
                Funcs[i] = funcs[i];
            }
        }
        
        public static void SetSigns(List<string> signs)
        {
            Signs = new string[signs.Count]; 
            for (int i = 0 ; i < signs.Count; i++)
            {
                Signs[i] = signs[i];
            }
        }
        
    }
}