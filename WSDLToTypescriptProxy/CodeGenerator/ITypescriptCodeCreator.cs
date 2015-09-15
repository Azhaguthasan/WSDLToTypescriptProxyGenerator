using System.CodeDom;

namespace WSDLToTypescriptProxy.CodeGenerator
{
    public interface ITypescriptCodeCreator
    {
        void GenerateCode(CodeCompileUnit codeCompileUnit, string outputFile);
    }
}