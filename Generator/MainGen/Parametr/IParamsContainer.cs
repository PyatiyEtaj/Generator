using Generator.Parsing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generator.MainGen.Parametr
{
    public interface IParamsContainer
    {
        Task<List<Param>> GenNewParametrs(List<DataContainer> d);
    }
}
