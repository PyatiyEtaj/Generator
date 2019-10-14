using System;
using System.Collections.Generic;
using System.Text;
using Generator.MainGen.Parametr;

namespace Generator.MainGen.ForGenFunc
{
    public class GenExpr : AFunc
    {
        private ArithmExpr _arithmExpr = new ArithmExpr();
        // выражения
        public override string Run(Param param, List<Param> parametrs = null)
        {
            var args = GetArgs(param.RawData, parametrs);

            if (args.Length != 1) throw new Exception($"Func #{FuncsEnum.genAE} take 1 parametr ( hardnessOfArithmeticExpression )");

            _arithmExpr.Run(Int32.Parse(args[0]));
            return _arithmExpr.Expression;
        }

        public string ExpressionCodeOnC()
        {
            return _arithmExpr.CodeOnC;
        }
    }
}
