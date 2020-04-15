using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Generator.Parsing;
using Generator.MainGen.Structs;
using System.Threading.Tasks;
using ALS.CheckModule.Processes;
using Newtonsoft.Json;
using Generator.MainGen.Parametr;

namespace Generator.MainGen
{
    public class Gen
    {
        private IParser _pr; // анализ файла-шаблона
        private IParamsContainer _paramsContainer; // объекты-параметры
        private GenFunctions _genFunctions = new GenFunctions(); // обертка для удобного использования функций генератора

        public Gen(IParser pr, IParamsContainer paramsContainer)
        {
            _pr = pr;
            _paramsContainer = paramsContainer;
        }

        private string PathSourceModelCode(string name, string fextension) => Path.Combine("sourceCodeModel", $"{name}.{fextension}");
        private string PathExecuteModel(string name, string fextension) => Path.Combine("executeModel", $"{name}.{fextension}");

        private string PathToSoulution(string subpath)
        {
            if (!Directory.Exists($"sourceCodeModel\\{subpath}"))
            {
                Directory.CreateDirectory($"sourceCodeModel\\{subpath}");
            }

            return $"sourceCodeModel\\{subpath}";
        }

        // использование объектов-параметров
        private void UseParams(GenData data, List<Param> parametrs)
        {
            foreach (var elem in parametrs)
            {
                var pattern = $"@{elem.Name}@";
                data.Template = data.Template.Replace(pattern, elem.Value);
                data.Code = data.Code.Replace(pattern, elem.Value);
                if (data.TestsD == null) continue;
                for (int i = 0; i < data.TestsD.Count; i++)
                {
                    for (int j = 0; j < data.TestsD[i].Data.Count; j++)
                    {
                        data.TestsD[i].Data[j] = data.TestsD[i].Data[j].Replace(pattern, elem.Value);
                    }
                }
            }
        }

        // обращение к модулю компиляции решений
        private async Task<bool> CompileSolution(int lr, int var)
        {
            string name = ProcessCompiler.CreatePath(lr, var);
            //ProcessCompiler pc = new ProcessCompiler(Path.Combine("sourceCodeModel", $"{lrPath}.cpp"), Path.Combine("executeModel", $"{lrPath}.exe"));
            ProcessCompiler pc = new ProcessCompiler(PathToSoulution(name), PathExecuteModel(name, "exe"));
            return await Task.Run(() => pc.Execute(60000));
        }

        // компиляция эталонного решения
        private async Task<string> Compile(GenData data, int lr, int var)
        {   
            string name = ProcessCompiler.CreatePath(lr, var); // имя исходного кода эталонного решения
            string pathtocpp = PathToSoulution(name); // путь до исходного кода
            // создание файла с исходным кодом в ПЗУ
            using (StreamWriter sw = new StreamWriter(Path.Combine(pathtocpp, $"{name}.cpp"), false, Encoding.UTF8))
            {
                await sw.WriteLineAsync(data.Code);
            }
            // компиляция решения
            if (!await CompileSolution(lr, var))
            {
                throw new Exception("Ошибка во время компиляции!");
            }

            return name;
        }

        public async Task<ResultData> Run(string fileName, int lr = 1, int var = 1, bool needCompile = false, bool returnRawCode = false)
        {
            var data = await Task.Run(() => _pr.Read(fileName));
            if (data == null) return null;

            var parametrs = _paramsContainer.GenNewParametrs(data.Sd);

            UseParams(data, parametrs);

            if (data.TestsD != null && !_genFunctions.CheckTests(data.TestsD))
            {
                throw new Exception("Тестовые данные содержат ошибку!");
            }

            string name = "std_name";
            if (needCompile) name = await Compile(data, lr, var);

            return new ResultData()
            {
                Template = data.Template, /* шаблон задания */
                Code = (returnRawCode) ? data.Code : new System.Uri(Path.Combine(Environment.CurrentDirectory, PathExecuteModel(name, "exe"))).AbsoluteUri, /*путь до бинарника / или сырой код*/
                Tests = JsonConvert.SerializeObject(data.TestsD) /* тестовые данные */
            };
        }
    }
}