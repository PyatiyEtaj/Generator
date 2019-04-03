using System.Collections.Generic;

namespace Generator
{
    public class GenData
    {
        public List<DataContainer> Sd { get; set; }
        public List<DataContainer> TestsD { get; set; }
        public string Template { get; set; }
        public string Code { get; set; }

        public GenData(List<DataContainer> sd, string template, string code, List<DataContainer> testsD)
        {
            Sd = sd;
            Template = template;
            Code = code;
            TestsD = testsD;
        }
    }
}