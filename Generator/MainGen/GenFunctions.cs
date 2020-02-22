using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Generator.MainGen.Parametr;
using Generator.Parsing;
using Newtonsoft.Json;
using System.Text;
using NLua;
using Generator.MainGen.ForGenFunc;

namespace Generator.MainGen
{
    public class GenFunctions
    {
        private List<AFunc> _f = new List<AFunc>(); 

        public GenFunctions()
        {
            _f.Add(new Rnd());
            _f.Add(new GenExpr());
            _f.Add(new LuaFunc());
            _f.Add(new ParentChecker());
        }

        public string WhatToDoWithParam(FuncsEnum funcs, Param param, List<Param> parametrs)
        {
            switch (funcs)
            {
                case FuncsEnum.rnd:
                    return _f[0].Run(param, parametrs);

                case FuncsEnum.genAE:
                    return _f[1].Run(param, parametrs);

                case FuncsEnum.lua:
                    return _f[2].Run(param, parametrs);

                case FuncsEnum.parent:
                    return _f[3].Run(param, parametrs);

                case FuncsEnum.getAEcode:
                    return ((GenExpr)_f[1]).ExpressionCodeOnC();

                default:
                    return param.RawData;

            }

            //return param.RawData;
        }

        public bool CheckTests(List<DataContainer> lDc)
        {
            foreach (DataContainer dc in lDc)
            {
                if (dc.Data.Count == 1) continue;

                foreach (string str in dc.Data)
                {
                    if (str.Contains($"#{FuncsEnum.rnd}"))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public List<List<string>> GetTestsFromJson(string json)
        {
            List<List<string>> result = new List<List<string>>();

            var tests = JsonConvert.DeserializeObject<List<DataContainer>>(json);

            foreach (var dc in tests)
            {
                if (dc.Data.Count > 1)
                {
                    result.Add(dc.Data);
                }
                else
                {
                    if (dc.Data[0].Contains($"#{FuncsEnum.rnd}"))
                    {
                        Param p = new Param(dc.Data[0], -1, "TEMP");
                        result.Add(new List<string>(_f[0].Run(p).Split(',')));
                    }
                    else
                    {
                        result.Add(dc.Data);
                    }
                }
            }

            return result;
        }
    }
}