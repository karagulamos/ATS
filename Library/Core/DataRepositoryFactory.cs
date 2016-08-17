using System.ComponentModel.Composition;
using Library.Core.Bootstrapper;
using Library.Core.Persistence.Repositories;

namespace Library.Core
{
    [Export(typeof(IDataRepositoryFactory))]
    [PartCreationPolicy(CreationPolicy.Any)]
    public class DataRepositoryFactory : IDataRepositoryFactory // An Abstract Factory
    {
        T IDataRepositoryFactory.Create<T>()
        {
            return MefDependencyBase.Container.GetExportedValue<T>(); // Dependencies are lazily loaded (on-demand)
        }
    }
}
