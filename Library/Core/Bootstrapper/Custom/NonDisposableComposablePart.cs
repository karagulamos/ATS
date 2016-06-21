using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;

namespace Library.Core.Bootstrapper.Custom
{
    public class NonDisposableComposablePart : ComposablePart
    {

        private readonly ComposablePart _composablePart;

        public NonDisposableComposablePart(ComposablePart composablePart)
        {
            _composablePart = composablePart;
        }

        public override IEnumerable<ExportDefinition> ExportDefinitions
        {
            get { return _composablePart.ExportDefinitions; }
        }

        public override object GetExportedValue(ExportDefinition definition)
        {
            return _composablePart.GetExportedValue(definition);
        }

        public override IEnumerable<ImportDefinition> ImportDefinitions
        {
            get { return _composablePart.ImportDefinitions; }
        }

        public override void SetImport(ImportDefinition definition, IEnumerable<Export> exports)
        {
            _composablePart.SetImport(definition, exports);
        }
    }
}
