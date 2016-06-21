using System.ComponentModel.Composition;
using Library.Core.Bootstrapper;

namespace Library.Services.Tasks
{
    [Export(typeof(ITaskRunnerFactory))]
    [PartCreationPolicy(CreationPolicy.Any)]
    class TaskRunnerFactory : ITaskRunnerFactory
    {
        T ITaskRunnerFactory.Create<T>(string contractName)
        {
            return MefDependencyBase.Container.GetExportedValue<T>(contractName);
        }
    }
}
