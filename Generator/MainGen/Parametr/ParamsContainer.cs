using Generator.Parsing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Generator.MainGen.Parametr
{
    public class ParamsContainer : IParamsContainer
    {
        public List<Param> Parametrs { get; set; } = new List<Param>();
        public GenFunctions Gf = new GenFunctions();

        
        public async Task<List<Param>> GenNewParametrs(List<DataContainer> d)
        {
            Parametrs.Clear();
            foreach (var sd in d)
            {
                bool flag = false;
                Dictionary<string, int> map = new Dictionary<string, int>();
                for (int i = 0; i < sd.Data.Count; i++)
                {
                    map.Add(sd.Data[i], i+1);
                }
                Param param = new Param("INIT", 0, sd.Name);
                while (!flag && sd.Data.Count > 0)
                {
                    int pos = Gf.Random.Next(0, sd.Data.Count);
                    string rawData = sd.Data[pos];
                    sd.Data.RemoveAt(pos);
                    param = new Param(rawData, map[rawData], sd.Name);
                    if ( param.FindParent() == FuncsEnum.parent)
                    {
                        if (!await Gf.FindParent(param, Parametrs)) continue;
                    }
                    param.Value = Gf.WhatToDoWithParam(param.WhatIsIt(), param.RawData, Parametrs);
                    flag = true;
                }

                if (!flag && sd.Data.Count == 0)
                {
                    param = new Param($"--< ERROR var({sd.Name}) - cannot gen a value for that variable >--", 0, sd.Name);
                }
                Parametrs.Add(param);
            }

            return Parametrs;
        }
    }
}
