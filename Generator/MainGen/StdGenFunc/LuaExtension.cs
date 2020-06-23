﻿using Generator.MainGen.Structs;
using System.Collections.Generic;

namespace Generator.MainGen.StdGenFunc
{
    class LuaExtension : LuaFunc
    {
        public LuaExtension(bool multypleReturnDatas = false) : base(multypleReturnDatas) { }
        public override dynamic Run(FunctionStruct fs)
        {
            // анализ - какой метод необходимо вызвать и из какого модуля
            List<(string, string)> ls = new List<(string, string)>();
            int pos = fs.FullFunctionName.IndexOf('.');
            if (pos == -1) { ls.Add(("0", fs.Raw)); return ls; }
            string moduleName = fs.FullFunctionName.Substring(0, pos);
            string funcName = fs.FullFunctionName.Substring(pos + 1, fs.FullFunctionName.Length - pos - 1);
            // создание кода для вызова метода
            fs.Args = $"\"local lib = require('{moduleName}');return lib.{funcName}({fs.Args});\"";
            // вызов метода
            return base.Run(fs);
        }
    }
}
