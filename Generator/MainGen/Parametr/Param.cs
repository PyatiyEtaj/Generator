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
        
        public FuncsEnum WhatIsIt()
        {
            FuncsEnum type = FuncsEnum.justString;

            if (RawData.Contains($"#{FuncsEnum.rnd}"))
            {
                type = FuncsEnum.rnd;
            }
            else if (RawData.Contains($"#{FuncsEnum.genAE}"))
            {
                type = FuncsEnum.genAE;
            }
            else if (RawData.Contains($"#{FuncsEnum.getAEcode}"))
            {
                type = FuncsEnum.getAEcode;
            }

            return type;
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
