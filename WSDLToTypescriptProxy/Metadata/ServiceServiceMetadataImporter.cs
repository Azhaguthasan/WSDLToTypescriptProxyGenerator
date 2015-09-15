using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace WSDLToTypescriptProxy.Metadata
{
    public class ServiceMetadataImporter : IServiceMetadataImporter
    {

        public WsdlImporter GetImporterFromServiceEndpoint(EndpointAddress metadataAddress)
        {
            var metadataSet = GetMetaDataDocs(metadataAddress);
            return CreateImporterFromMetadata(metadataSet);
        }


        private MetadataSet GetMetaDataDocs(EndpointAddress metadataAddress)
        {
            var binding = new WSHttpBinding(SecurityMode.Transport);
            binding.MaxReceivedMessageSize = Int32.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = Int32.MaxValue;
            var mexClient = new MetadataExchangeClient(binding)
            {
                ResolveMetadataReferences = true,
                MaximumResolvedReferences = 1000
            };

            var metaDocs = mexClient.GetMetadata(metadataAddress.Uri, MetadataExchangeClientMode.HttpGet);
            return metaDocs;
        }

        private WsdlImporter CreateImporterFromMetadata(MetadataSet metaDocs)
        {
            var importer = new WsdlImporter(metaDocs);
            object dataContractImporter;
            XsdDataContractImporter xsdDcImporter;
            if (!importer.State.TryGetValue(typeof(XsdDataContractImporter), out dataContractImporter))
            {
                Console.WriteLine("Couldn't find the XsdDataContractImporter! Adding custom importer.");
                xsdDcImporter = new XsdDataContractImporter { Options = new ImportOptions() };
                importer.State.Add(typeof(XsdDataContractImporter), xsdDcImporter);
            }
            else
            {
                xsdDcImporter = (XsdDataContractImporter)dataContractImporter;
                if (xsdDcImporter.Options == null)
                {
                    Console.WriteLine("There were no ImportOptions on the importer.");
                    xsdDcImporter.Options = new ImportOptions();
                }
            }
            return importer;
        }
    }
}