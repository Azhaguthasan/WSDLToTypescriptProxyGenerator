using System.ServiceModel;
using System.ServiceModel.Description;

namespace WSDLToTypescriptProxy.Metadata
{
    public interface IServiceMetadataImporter
    {
        WsdlImporter GetImporterFromServiceEndpoint(EndpointAddress metadataAddress);
    }
}