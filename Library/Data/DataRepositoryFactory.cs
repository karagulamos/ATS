using System.ComponentModel.Composition;
using Library.Core.Bootstrapper;

namespace Library.Data
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
