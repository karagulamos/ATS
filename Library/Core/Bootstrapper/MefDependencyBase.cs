using System.ComponentModel.Composition.Hosting;

namespace Library.Core.Bootstrapper
{
    public class MefDependencyBase
    {
        private readonly CompositionContainer _container;

        private static readonly MefDependencyBase Dependency;

        private MefDependencyBase()
        {
            _container = MefLoader.Init();
        }

        static MefDependencyBase()
        {
            Dependency = new MefDependencyBase();
        }

        public static CompositionContainer Container
        {
            get { return Dependency._container; }
        }
    }
}
