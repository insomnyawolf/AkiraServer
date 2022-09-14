using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using MimeTypeMap;

namespace AkiraServer.Helper
{
    class Helper
    {
        public enum Enc
        {
            UTF8,
            ASCII
        }
        public static byte[] GetBytes(string str, Enc e = Enc.UTF8)
        {
            if(e == Enc.ASCII)
            {
                return Encoding.ASCII.GetBytes(str);
            }
            if (e == Enc.UTF8)
            {
                return Encoding.UTF8.GetBytes(str);
            }
            throw new Exception("Encoding not implemented");
        }

        public static void Debug(string debugInfo)
        {
            if (Config.DebugToConsole)
            {
                Console.WriteLine(debugInfo);
            }
        }

        public static void Log(string logInfo)
        {
            if (Config.LogToConsole)
            {
                Console.WriteLine(logInfo);
            }
        }

        

        public static HelperEnum.IdentifyPath Identifypath(string path)
        {
            if (File.Exists(path))
            {
                return HelperEnum.IdentifyPath.File;
            }

            if (Directory.Exists(path))
            {
                return HelperEnum.IdentifyPath.Directory;
            }

            return HelperEnum.IdentifyPath.NonExists;
        }

        public static byte[] FileToByteArray(string fileName)
        {
            return File.ReadAllBytes(fileName);
        }

        public static string GetMime(string fileExt)
        {
            return MimeTypeMap.List.MimeTypeMap.GetMimeType(Path.GetExtension("."+fileExt))[0];
        }

        internal static HelperEnum.IdentifyFile Identifyfile(string text)
        {
            if (text.EndsWith(".cs"))
            {
                return HelperEnum.IdentifyFile.Cs;   
            }

            if (text.EndsWith(".php"))
            {
                return HelperEnum.IdentifyFile.PHP;
            }
            return HelperEnum.IdentifyFile.Other;
        }
    }

    class HelperEnum
    {
        public enum IdentifyPath
        {
            File,
            Directory,
            NonExists
        }

        public enum IdentifyFile
        {
            Cs,
            PHP,
            Other
        }
    }
}
