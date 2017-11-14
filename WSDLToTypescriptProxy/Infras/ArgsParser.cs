using System.Collections.Generic;

namespace WSDLToTypescriptProxy.Infras
{
    public class ArgsParser
    {
        public bool TryParseArgs(string[] args, out Dictionary<ArgType, string> parsedArgs)
        {
            parsedArgs = new Dictionary<ArgType, string>();

            if (args.Length == 0 || (args.Length != 1 && args.Length != 3 && args[1] != "/o"))
                return false;

            var remoteUri = string.Empty;
            var outputFileName = "ServiceReference.ts";

            if (args.Length == 1)
            {
                remoteUri = args[0];
            }
            else if (args.Length == 3)
            {
                outputFileName = args[1];
                remoteUri = args[2];
            }

            parsedArgs.Add(ArgType.OutputFileName, outputFileName);
            parsedArgs.Add(ArgType.RemoteUri, remoteUri);

            return true;
        }

    }
}
