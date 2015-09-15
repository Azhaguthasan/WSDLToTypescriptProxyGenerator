using System.CodeDom;
using System.ServiceModel.Description;

namespace WSDLToTypescriptProxy.ContractGenerator
{
    public interface IServiceContractCreator
    {
        CodeCompileUnit GenerateCodeFromImportedContracts(WsdlImporter importer);
    }
}