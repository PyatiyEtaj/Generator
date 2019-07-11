using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Generator.Parsing;
using Generator.MainGen.Structs;
using System.Threading.Tasks;
using ALS.CheckModule.Processes;
using Newtonsoft.Json;
using Generator.MainGen;
using Generator.MainGen.Parametr;

namespace Generator.MainGen
{
    public class Gen
    {
        private IParser _pr;
        private IParamsContainer _paramsContainer;
        private List<Param> _parametrs;
        private GenFunctions _genFunctions = new GenFunctions();

        public Gen(IParser pr, IParamsContainer paramsContainer)
        {
            _pr = pr;
            _paramsContainer = paramsContainer;
        }
        /*
        // Выполнение необходимой функции (указаны в файле GenFunctions)
        private string CheckF(string str)
        {
            if (str.Contains($"#{FuncsEnum.rnd}"))
            {
                str = _genFunctions.Rnd(str, _parametrs);
            }
            else if (str.Contains($"#{FuncsEnum.genAE}"))
            {
                str = _genFunctions.Expression(str, _parametrs);
            }
            else if (str.Contains($"#{FuncsEnum.getAEcode}"))
            {
                str = _genFunctions.ExpressionCodeOnC();
            }
            return str;
        }
        private List<Param> ProcessData(List<DataContainer> d)
        {
            List<Param> ls = new List<Param>();
            foreach (var sd in d)
            {
                var pos = _genFunctions.Random.Next(0, sd.Data.Count);
                string rawData = sd.Data[pos];
                Param param = new Param(rawData, pos);
                //param.DefValue(_genFunctions);
                ls.Add(param);
            }
            return ls;
        }
        */

        private async Task<bool> Compile(int lr,int var)
        {
            string lrPath = ProcessCompiler.CreatePath(lr, var);
            ProcessCompiler pc = new ProcessCompiler(Path.Combine("sourceCodeModel", $"{lrPath}.cpp"), Path.Combine("executeModel", $"{lrPath}.exe"));
            return await Task.Run (() => pc.Execute(60000));
        }

        public async Task<ResultData> Run(string fileName,int lr = 1, int var = 1)
        {     
            // тупа парсинг
            var d = await Task.Run( () => _pr.Read(fileName));
            if (d == null) return null;

            // тупа генерация
            _parametrs = await _paramsContainer.GenNewParametrs(d.Sd);

            foreach (var elem in _parametrs)
            {
                var pattern = $"({elem.Name})";
                d.Template = d.Template.Replace(pattern, elem.Value);
                d.Code = d.Code.Replace(pattern, elem.Value);
                // кансер шо пипес
                for (int i = 0; i < d.TestsD.Count; i++)
                {
                    for (int j = 0; j < d.TestsD[i].Data.Count; j++)
                    {
                        d.TestsD[i].Data[j] = d.TestsD[i].Data[j].Replace(pattern, elem.Value);
                    }
                }
            }

            if (!_genFunctions.CheckTests(d.TestsD))
            {
                throw new Exception("Тестовые данные содержат ошибку!");
            }
            
            string lrPath = ProcessCompiler.CreatePath(lr, var);
            
            using (StreamWriter sw = new StreamWriter(Path.Combine("sourceCodeModel",$"{lrPath}.cpp"), false, Encoding.UTF8))
            {
                await sw.WriteLineAsync(d.Code);
            }
            
            // тупа компиляция
            if (!await Compile(lr, var))
            {
                throw new Exception("Ошибка во время компиляции!");
            }

            return new ResultData() {
                Template = d.Template, /* шаблон задания */
                Code = new System.Uri(Path.Combine(Environment.CurrentDirectory, "executeModel", $"{lrPath}.exe")).AbsoluteUri, /* путь до бинарника */
                //Code = d.Code,
                Tests = JsonConvert.SerializeObject(d.TestsD) /* тестовые данные */
            };
        }
    }
}