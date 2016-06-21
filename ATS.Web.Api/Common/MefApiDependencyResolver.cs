using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Web.Http.Dependencies;
using Library.Core.Bootstrapper;

namespace ATS.Web.Api.Common
{
    public class MefApiDependencyResolver : IDependencyResolver
    {
        public MefApiDependencyResolver(CompositionContainer container)
        {
            _Container = container;
        }

        private CompositionContainer _Container;

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            return _Container.GetExportedValueByType(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _Container.GetExportedValuesByType(serviceType);
        }

        public void Dispose()
        {
        }
    }
}