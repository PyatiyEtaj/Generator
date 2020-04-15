using Generator.MainGen.StdGenFunc;
using Generator.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generator.MainGen.Parametr
{
    public class ParamsContainer : IParamsContainer
    {
        public List<Param> Parametrs { get; set; } = new List<Param>();
        public GenFunctions Gf = new GenFunctions();
        private Dictionary<int, FuncsEnum> _funcs = new Dictionary<int, FuncsEnum>();
        
        public ParamsContainer()
        {
            foreach (int i in Enum.GetValues(typeof(FuncsEnum)))
            {
                FuncsEnum f = (FuncsEnum)i;
                _funcs.Add(AFunc.GetHashOfFunc(f.ToString()), f);
            }
        }

        private FuncsEnum WhatParamIsIt(string name)
        {
            if (name == FuncsEnum.justString.ToString()) return FuncsEnum.justString;

            int h = AFunc.GetHashOfFunc(name);
            if (_funcs.ContainsKey(h))
            {
                if (name == $"{_funcs[h]}")
                    return _funcs[h];
            }
            return FuncsEnum.luaExtension;
        }

        public List<Param> GenNewParametrs(List<DataContainer> dataContainer)
        {
            Random r = new Random();
            Parametrs.Clear();
            foreach (DataContainer d in dataContainer)
            {
                //DataContainer d = dataContainer[counter];
                //bool flag = false;
                Dictionary<string, int> map = new Dictionary<string, int>();
                for (int i = 0; i < d.Data.Count; i++)
                {
                    map.Add(d.Data[i], i+1);
                }
                Param param = new Param(default, default, d.Name);
                while (/*!flag &&*/ d.Data.Count > 0)
                {
                    //Console.WriteLine(d.Data);
                    int pos = r.Next(0, d.Data.Count);
                    string rawData = d.Data[pos];
                    d.Data.RemoveAt(pos);
                    param = new Param(rawData, map[rawData], d.Name, Parametrs);
                    var ftype = WhatParamIsIt(param.GetFuncName());
                    /* TEMP*/
                    /*if (ftype == FuncsEnum.parent)
                    {
                        if (!bool.Parse(Gf.WhatToDoWithParam(FuncsEnum.parent, param, Parametrs))) continue;
                    }*/
                    param.Value = Gf.WhatToDoWithParam(ftype, param, Parametrs);
                    //flag = true;
                }

                /* TEMP*/
                /*if (!flag && d.Data.Count == 0)
                {
                    //param = new Param($"--< ERROR var({d.Name}) - cannot gen a value for that variable >--", 0, d.Name);
                    param = new Param("", 1, d.Name);
                }*/
                // здесь добавить чтоб изменял параметры с одинаковыми именами
                Parametrs.Add(param);
            }

            return Parametrs;
        }
    }
}
