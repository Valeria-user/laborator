using System;

namespace SimpleCalculatorMVVM.Models.Commands
{
    public class ScientificCommand : ICalculatorCommand
    {
        private readonly CalculatorEngine _engine;
        private readonly string _function;
        private string _previousState;

        public ScientificCommand(CalculatorEngine engine, string function)
        {
            _engine = engine;
            _function = function;
        }

        public void Execute()
        {
            _previousState = _engine.GetCurrentInput();
            _engine.ExecuteScientificFunction(_function);
        }

        public void Undo()
        {
            _engine.RestoreCurrentInput(_previousState);
            _engine.SetNewInputFlag(true);
        }
    }
}