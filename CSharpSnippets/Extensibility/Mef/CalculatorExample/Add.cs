using System.ComponentModel.Composition;

namespace CSharpSnippets.Extensibility.Mef.CalculatorExample
{
    [Export(typeof(IOperation))]
    [ExportMetadata("Symbol", '+')]
    class Add : IOperation
    {
        public int Operate(int left, int right)
        {
            return left + right;
        }
    }
}
