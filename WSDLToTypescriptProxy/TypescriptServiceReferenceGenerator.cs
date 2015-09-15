using System.ServiceModel;
using WSDLToTypescriptProxy.CodeGenerator;
using WSDLToTypescriptProxy.ContractGenerator;
using WSDLToTypescriptProxy.Metadata;

namespace WSDLToTypescriptProxy
{
    public class TypescriptServiceReferenceGenerator
    {
        private readonly IServiceMetadataImporter _serviceMetadataImporter;
        private readonly IServiceContractCreator _soapServiceContractCreator;
        private readonly ITypescriptCodeCreator _typescriptCodeCreator;

        public TypescriptServiceReferenceGenerator()
        {
            _serviceMetadataImporter = new ServiceMetadataImporter();
            _soapServiceContractCreator = new SoapServiceContractCreator();
            _typescriptCodeCreator = new TypescriptCodeCreator();
        }

        public void GenerateServiceReferenceCode(string remoteUri, string outputFile)
        {
            var endpoint = new EndpointAddress(remoteUri);
            var importer = _serviceMetadataImporter.GetImporterFromServiceEndpoint(endpoint);
            var codeCompileUnit = _soapServiceContractCreator.GenerateCodeFromImportedContracts(importer);
            _typescriptCodeCreator.GenerateCode(codeCompileUnit, outputFile);            
        }
    }
}