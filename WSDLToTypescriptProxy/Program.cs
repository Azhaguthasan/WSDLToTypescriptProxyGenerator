using System;
using System.Collections.Generic;
using System.IO;
using WSDLToTypescriptProxy.Infras;

namespace WSDLToTypescriptProxy
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var argsParser = new ArgsParser();
            var argsHelper = new ArgsHelper();
            Dictionary<ArgType, string> argsList;
            var hasAllRequiredArgs = argsParser.TryParseArgs(args, out argsList);

            if (!hasAllRequiredArgs)
            {
                Console.WriteLine(argsHelper.GetArgsHelp());
                return;
            }

            try
            {
                var typescriptServiceReferenceGenerator = new TypescriptServiceReferenceGenerator();
                typescriptServiceReferenceGenerator.GenerateServiceReferenceCode(argsList[ArgType.RemoteUri], argsList[ArgType.OutputFileName]);
            }
            catch (IOException)
            {
                Console.WriteLine("The generator failed to generate typescript code. Please check you have access to the file path");
            }
            catch (Exception)
            {
                Console.WriteLine("The generator failed to generate typescript code. Please check the arguments and ensure the Soap service is up and running.");
            }
        }
    }
}