using System.Collections.Generic;

namespace Generator
{
    public class DataContainer
    {
        public string Name { get; set; } 
        public List<string> Data { get; set; }

        public DataContainer(string name, List<string> data)
        {
            Name = name;
            Data = data;
        }
        
    }
}