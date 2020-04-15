﻿using Generator.MainGen.Parametr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Generator.MainGen.StdGenFunc
{
    class LuaExtension : LuaFunc
    {
        public override string Run(Param param)
        {
            var args = GetArgs(param.RawData);
            int pos = param.FunctionName.IndexOf('.');
            string moduleName = pos == -1 ? args[0] : param.FunctionName.Substring(0, pos);
            string funcName = pos == -1 ? args[1] : param.FunctionName.Substring(pos+1, param.FunctionName.Length-pos-1);
            StringBuilder funcArgs = new StringBuilder();
            for (int i = pos == -1 ? 2 : 0; i < args.Length; i++) funcArgs.Append($",{args[i]}");funcArgs[0] = ' ';
            string cmd = $"#lua(\"local lib = require('{moduleName}');return lib.{funcName}({funcArgs.ToString()});\")";
            return base.Run(new Param(cmd, default, default));
        }
    }
}