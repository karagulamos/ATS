using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace Library.Core.Bootstrapper
{
    public static class MefExtensions
    {
        public static object GetExportedValueByType(this CompositionContainer container, Type type)
        {
            if (!container.Catalog.Parts.Any()) return null;

            var firstOrDefault = GetMefExports(container, type).FirstOrDefault();

            return firstOrDefault != null ? firstOrDefault.Value : null;
        }

        public static IEnumerable<object> GetExportedValuesByType(this CompositionContainer container, Type type)
        {
            return container.Catalog.Parts.Any() ? GetMefExports(container, type) : new List<object>().AsEnumerable();
        }


        private static IEnumerable<Export> GetMefExports(CompositionContainer container, Type type)
        {
            if (Enumerable.Any(container.Catalog.Parts, partDef => partDef.ExportDefinitions.Any(exportDef => exportDef.ContractName == type.FullName)))
            {
                var contract = AttributedModelServices.GetContractName(type);
                var definition = new ContractBasedImportDefinition(contract, contract, null, ImportCardinality.ExactlyOne, false, false, CreationPolicy.Any);
                return container.GetExports(definition);
            }

            return new List<Export>();
        }
    }
}
