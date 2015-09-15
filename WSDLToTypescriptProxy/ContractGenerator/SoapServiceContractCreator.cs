using System;
using System.CodeDom;
using System.ServiceModel.Description;

namespace WSDLToTypescriptProxy.ContractGenerator
{
    public class SoapServiceContractCreator : IServiceContractCreator
    {
        public CodeCompileUnit GenerateCodeFromImportedContracts(WsdlImporter importer)
        {
            var generator = new ServiceContractGenerator();

            var contracts = importer.ImportAllContracts();
            foreach (var contract in contracts)
            {
                generator.GenerateServiceContractType(contract);
            }
            if (generator.Errors.Count != 0)
                throw new Exception("There were errors during code compilation.");
            var targetCompileUnit = generator.TargetCompileUnit;
            return targetCompileUnit;
        }
    }
}
