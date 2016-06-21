namespace Library.Services.Tasks
{
    public interface ITaskRunnerFactory
    {
        T Create<T>(string contractName) where T : ITaskRunner;
    }
}