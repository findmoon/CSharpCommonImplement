// C#10 支持 文件范围的命名空间声明
//namespace MiscellaneousTest.PowerShellExecCSharp;

public class Calculator
    {
        private int _base;

        public Calculator() { }

        public Calculator(int _base)
        {
            this._base = _base;
        }

        public static int Add(int a, int b)
        {
            return a + b;
        }

        public int Multiply(int a, int b)
        {
            return a * b;
        }

        public int BasePlusMultiply(int a, int b)
        {
            return _base + a * b;
        }
    }

