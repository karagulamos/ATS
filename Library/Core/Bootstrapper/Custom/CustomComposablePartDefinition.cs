using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.ReflectionModel;

namespace Library.Core.Bootstrapper.Custom
{
    public class CustomComposablePartDefinition : ComposablePartDefinition
    {
        private readonly ComposablePartDefinition _innerPartDefinition;

        public CustomComposablePartDefinition(ComposablePartDefinition innerPartDefinition)
        {
            _innerPartDefinition = innerPartDefinition;
        }

        public override ComposablePart CreatePart()
        {
            var innerPart = _innerPartDefinition.CreatePart();
            if (ReflectionModelServices.IsDisposalRequired(_innerPartDefinition) &&
                _innerPartDefinition.Metadata.ContainsKey(CompositionConstants.PartCreationPolicyMetadataName) &&
                (CreationPolicy)_innerPartDefinition.Metadata[CompositionConstants.PartCreationPolicyMetadataName] == CreationPolicy.NonShared)
            {
                return new NonDisposableComposablePart(innerPart);
            }
            return innerPart;
        }

        public override IEnumerable<ExportDefinition> ExportDefinitions
        {
            get { return _innerPartDefinition.ExportDefinitions; }
        }

        public override IEnumerable<ImportDefinition> ImportDefinitions
        {
            get { return _innerPartDefinition.ImportDefinitions; }
        }

        public override IDictionary<string, object> Metadata
        {
            get { return _innerPartDefinition.Metadata; }
        }

        public override string ToString()
        {
            return _innerPartDefinition.ToString();
        }
    }
}
