using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace Library.Core.Bootstrapper.Custom
{
    public class CustomCatalog : ComposablePartCatalog
    {
        private readonly ComposablePartCatalog _innerCatalog;
        private readonly IDictionary<ComposablePartDefinition, CustomComposablePartDefinition> _partDefinitions;
        public CustomCatalog(ComposablePartCatalog catalog)
        {
            _innerCatalog = catalog;
            _partDefinitions = new Dictionary<ComposablePartDefinition, CustomComposablePartDefinition>();
        }

        public override IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(ImportDefinition definition)
        {
            var exports = _innerCatalog.GetExports(definition);
            return exports.Select(e => 
                new Tuple<ComposablePartDefinition, ExportDefinition>(GetCustomPart(e.Item1), e.Item2)
            );
        }

        public override IQueryable<ComposablePartDefinition> Parts
        {
            get
            {
                return _innerCatalog.Parts.Select(p => GetCustomPart(p)).AsQueryable();
            }
        }

        private CustomComposablePartDefinition GetCustomPart(ComposablePartDefinition part)
        {
            if (_partDefinitions.ContainsKey(part))
            {
                return _partDefinitions[part];
            }

            var customPart = new CustomComposablePartDefinition(part);
            _partDefinitions[part] = customPart;
            return customPart;
        }
    }
}
