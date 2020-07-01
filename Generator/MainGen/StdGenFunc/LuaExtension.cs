using Generator.MainGen.Structs;
using System;
using System.Collections.Generic;
using System.IO;

namespace Generator.MainGen.StdGenFunc
{
    class LuaExtension : LuaFunc
    {
        private string _tempDir = default;
        public LuaExtension(string tempDir = default,bool multypleReturnDatas = false) : base(multypleReturnDatas) 
        {
            _tempDir = tempDir;
        }

        private string GetPackage(string moduleName)
        {
            var dir = Path.GetDirectoryName(moduleName);
            var package =
                _tempDir == default ? "" : $"package.path = package.path .. ';{Path.Combine(_tempDir, dir, "?.lua").Replace("\\", "/")}';";
            return package;
        }

        public override dynamic Run(FunctionStruct fs)
        {
            // анализ - какой метод необходимо вызвать и из какого модуля
            List<(string, string)> ls = new List<(string, string)>();
            int pos = fs.FullFunctionName.IndexOf('.');
           
            if (pos == -1) { ls.Add(("0", fs.Raw)); return ls; }
            string moduleName =  fs.FullFunctionName.Substring(0, pos).Replace("\\", "/");
            string funcName = fs.FullFunctionName.Substring(pos + 1, fs.FullFunctionName.Length - pos - 1);
            // создание кода для вызова метода
            var package = GetPackage(moduleName);
            fs.Args = $"\"{package} local lib = require('{moduleName}');return lib.{funcName}({fs.Args});\"";
            // вызов метода
            return base.Run(fs);
        }
    }
}
