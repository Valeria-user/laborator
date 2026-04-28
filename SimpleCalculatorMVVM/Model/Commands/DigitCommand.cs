namespace SimpleCalculatorMVVM.Models.Commands
{
    public class DigitCommand : ICalculatorCommand
    {
        private readonly CalculatorEngine _engine;
        private readonly string _digit;
        private string _previousState;

        public DigitCommand(CalculatorEngine engine, string digit)
        {
            _engine = engine;
            _digit = digit;
        }

        public void Execute()
        {
            _previousState = _engine.GetCurrentInput();
            _engine.ProcessDigitOrPoint(_digit);
        }

        public void Undo()
        {
            _engine.RestoreCurrentInput(_previousState);
        }
    }
}