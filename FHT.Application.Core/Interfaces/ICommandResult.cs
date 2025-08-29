namespace FHT.Application.Core.Interfaces
{
    public interface ICommandResult
    {
        bool Success
        {
            get;
        }

        string Message
        {
            get;
        }

        object Data
        {
            get;
        }
    }
}
