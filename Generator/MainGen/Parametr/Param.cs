using System;
using System.Collections.Generic;
using System.Text;

namespace Generator.MainGen.Parametr
{
    public class Param
    {
        //public Param Parent { get; set; } = null;
        //public bool AreParentsFound { get; set; } = false;
        public string Value { get; set; }
        public string Name { get; set; }
        public string RawData { get; set; }
        public int Position { get; set; }

        public Param(string raw, int pos, string name)
        {
            RawData = raw;
            Value = raw;
            Position = pos;
            Name = name;
        }

        public string GetFuncName(string str)
        {
            int p = str.IndexOf('(');
            if (p < 0) return "NULL";
            return str.Substring(0, p);
        }

        public FuncsEnum CheckParamType(string str)
        {
            string tmp = GetFuncName(str);
            FuncsEnum type = FuncsEnum.justString;
            if (tmp.Contains($"#{FuncsEnum.rnd}"))
            {
                type = FuncsEnum.rnd;
            }
            else if (tmp.Contains($"#{FuncsEnum.genAE}"))
            {
                type = FuncsEnum.genAE;
            }
            else if (tmp.Contains($"#{FuncsEnum.getAEcode}"))
            {
                type = FuncsEnum.getAEcode;
            }
            else if (tmp.Contains($"#{FuncsEnum.lua}"))
            {
                type = FuncsEnum.lua;
            }

            return type;
        }

        public FuncsEnum WhatIsIt()
        {
            return CheckParamType(RawData);
        }

        public FuncsEnum FindParent()
        {
            if (RawData.Contains($"#{FuncsEnum.parent}"))
            {
                return FuncsEnum.parent;
            }

            return FuncsEnum.justString;
        }
    }
}
