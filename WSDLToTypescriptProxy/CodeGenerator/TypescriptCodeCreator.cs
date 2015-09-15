using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TypescriptCodeDom;

namespace WSDLToTypescriptProxy.CodeGenerator
{
    public class TypescriptCodeCreator : ITypescriptCodeCreator
    {

        public void GenerateCode(CodeCompileUnit codeCompileUnit, string outputFile)
        {
            CleanCode(codeCompileUnit);
            GenerateCodeUsingCodeDOM(codeCompileUnit, outputFile);
        }

        private void GenerateCodeUsingCodeDOM(CodeCompileUnit codeCompileUnit, string outputFile)
        {
            var options = new CodeGeneratorOptions
            {
                BracingStyle = "JS"
            };
            var codeDomProvider = new TypescriptCodeProvider();
            using (var textWriter = new IndentedTextWriter(new StreamWriter(outputFile)))
            {
                codeDomProvider.GenerateCodeFromCompileUnit(codeCompileUnit, textWriter, options);
                textWriter.Close();
            }
        }

        private void CleanCode(CodeCompileUnit targetCompileUnit)
        {
            targetCompileUnit.Namespaces
                .OfType<CodeNamespace>()
                .ForEach(ns =>
            {
                CleanupImports(ns);
                CleanupTypes(ns);
            });
        }

        private void CleanupTypes(CodeNamespace ns)
        {
            if (string.IsNullOrWhiteSpace(ns.Name))
            {
                CleanupBaseNamespace(ns);
            }

            ns.Types
                .OfType<CodeTypeDeclaration>()
                .ForEach(CleanupType);
        }

        private void CleanupBaseNamespace(CodeNamespace ns)
        {
            var allTypesInNamespace = ns.Types.OfType<CodeTypeDeclaration>().ToList();
            var client = allTypesInNamespace
                .FirstOrDefault(declaration => declaration.IsClass && declaration.Name.EndsWith("Client"));

            if (client != null)
                ns.Types.Remove(client);

            var channelInterface =
                allTypesInNamespace.FirstOrDefault(
                    declaration => declaration.IsInterface && declaration.Name.EndsWith("Channel"));

            if (channelInterface != null)
                ns.Types.Remove(channelInterface);
        }

        private void CleanupType(CodeTypeDeclaration declaration)
        {
            if (declaration.IsClass)
            {
                declaration.IsInterface = true;
                declaration.IsClass = false;
            }

            var systemBaseTypes =
                declaration.BaseTypes
                .OfType<CodeTypeReference>()
                .Where(reference => reference.BaseType.Contains("System"))
                .ToList();

            systemBaseTypes.ForEach(declaration.BaseTypes.Remove);


            var systemMembers = declaration.Members
                .OfType<CodeTypeMember>()
                .Where(IsASystemMember)
                .ToList();

            systemMembers.ForEach(declaration.Members.Remove);
        }

        private bool IsASystemMember(CodeTypeMember member)
        {
            if (!(member is CodeMemberField || member is CodeMemberProperty))
                return false;

            return member is CodeMemberField
                ? ((CodeMemberField) member).Type.BaseType.Contains("System.Runtime")
                : ((CodeMemberProperty) member).Type.BaseType.Contains("System.Runtime");
        }

        private static void CleanupImports(CodeNamespace ns)
        {
            var nonSystemImports = ns.Imports
                .OfType<CodeNamespaceImport>()
                .Where(import => !import.Namespace.Contains("System"))
                .ToList();

            ns.Imports.Clear();
            nonSystemImports.ForEach(ns.Imports.Add);
        }
    }
}
