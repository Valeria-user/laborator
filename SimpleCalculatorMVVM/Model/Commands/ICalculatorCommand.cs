namespace SimpleCalculatorMVVM.Models.Commands
{
    public interface ICalculatorCommand
    {
        void Execute();
        void Undo();
    }
}