using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;
using Library.Core.Bootstrapper.Custom;

namespace Library.Core.Bootstrapper
{
    public class MefLoader
    {
        private static readonly AggregateCatalog AggregateCatalog;

        private MefLoader() { }

        static MefLoader()
        {
            AggregateCatalog = new AggregateCatalog();
        }

        public static CompositionContainer Init()
        {
            var assemblyCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            return Init(new Collection<ComposablePartCatalog> { assemblyCatalog });
        }

        public static CompositionContainer Init(ICollection<ComposablePartCatalog> catalogParts)
        {
            if (catalogParts != null)
            {
                foreach (var part in catalogParts.Where(part => !AggregateCatalog.Catalogs.Contains(part)))
                {
                    AggregateCatalog.Catalogs.Add(new CustomCatalog(part));
                }
            }

            var container = new CompositionContainer(AggregateCatalog, CompositionOptions.IsThreadSafe);

            return container;
        }
    }
}
