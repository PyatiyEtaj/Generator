using System;
using System.Collections.Generic;
using System.Text;
using Generator.MainGen.Parametr;
using NLua;

namespace Generator.MainGen.ForGenFunc
{
    public class ParentChecker : AFunc
    {
        private Lua _lua;

        public ParentChecker()
        {
            _lua = new Lua();
            _lua.State.Encoding = Encoding.UTF8;
        }

        public override string Run(Param param, List<Param> parametrs = null)
        {
            string raw = param.RawData;
            var args = GetArgs(param.RawData, parametrs);
            if (args.Length < 2) throw new Exception($"func #{FuncsEnum.parent} takes 2+ parametrs (value | nameOfParent| posOfParam(| posOfParam...))");
            param.RawData = param.Value = args[0];
            StringBuilder code = new StringBuilder($"return {args[1]}");
            if (args.Length > 2)
            {
                for (int i = 2; i < args.Length; i++)
                {
                    var s = args[i].Split('=', 2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (Param p in parametrs)
                    {
                        if (p.Name == s[0] && p.Position == Int32.Parse(s[1]))
                        {
                            code = code.Replace(p.Name, "true");
                        }
                    }
                }
            }

            bool res = false;
            try
            {
                res = (bool)_lua.DoString(code.ToString())[0];
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in #parent string = |{raw}|   | message = {ex.Message} |");
            }
            return res.ToString();
        }
    }
}
