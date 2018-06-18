using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MadMilkman.Ini;
using AkiraServer.Helper;

namespace AkiraServer
{
    class Config
    {
        private static string iniPath = string.Format("{0}\\{1}",Directory.GetCurrentDirectory(), "AkiraSettings.ini");
        //
        public static string Root;
        public static int Port;
        public static bool ListDirectories;
        public static bool LocalhostOnly;
        public static bool LogToConsole;
        //
        public static bool DebugToConsole;

        public static void Conf()
        {
            if (!LoadIni())
            {
                if (CreateIni())
                {
                    if (!LoadIni())
                    {
                        Console.WriteLine("Critical Error!\nCould not load config\nPress any key to exit...");
                        Console.ReadKey(false);
                    }
                }
            }
        }

        private static IniOptions iniOptions()
        {
            IniOptions options = new IniOptions();
            options.CommentStarter = IniCommentStarter.Hash;
            options.Compression = false;
            options.Encoding = Encoding.UTF8;
            options.EncryptionPassword = null;
            return options;
        }

        private static bool LoadIni()
        {
            try
            {
                IniFile file = new IniFile(iniOptions());
                file.Load(iniPath);

                // Map 'yes' value as 'true' boolean.
                file.ValueMappings.Add("yes", true);
                file.ValueMappings.Add("true", true);
                file.ValueMappings.Add("1", true);
                // Map 'no' value as 'false' boolean.
                file.ValueMappings.Add("no", false);
                file.ValueMappings.Add("false", false);
                file.ValueMappings.Add("0", false);

                IniSection Settings = file.Sections["Settings"];
                //
                Settings.Keys["Root"].TryParseValue(out Root);
                Settings.Keys["Port"].TryParseValue(out Port);
                Settings.Keys["ListDirectories"].TryParseValue(out ListDirectories);
                Settings.Keys["LocalhostOnly"].TryParseValue(out LocalhostOnly);
                Settings.Keys["LogToConsole"].TryParseValue(out LogToConsole);
                //
                IniSection Debug = file.Sections["Debug"];
                //
                Debug.Keys["DebugToConsole"].TryParseValue(out DebugToConsole);
            }
            catch(System.IO.FileNotFoundException e)
            {
                Helper.Helper.Debug(e.Message);
                return false;
            }
            Helper.Helper.Debug(
                "Debug: " + Environment.NewLine +
                "   Root: " + Root + Environment.NewLine +
                "   Port: " + Port + Environment.NewLine +
                "   ListDirectories: " + ListDirectories + Environment.NewLine +
                "   LocalhostOnly: " + LocalhostOnly + Environment.NewLine +
                "   LogToConsole: " + LogToConsole + Environment.NewLine +
                "   DebugToConsole: " + DebugToConsole + Environment.NewLine + Environment.NewLine
                );
            return true;
        }

        private static bool CreateIni()
        {
            int Indentation = 4;
            // Create new file with a default formatting.
            IniFile file = new IniFile(iniOptions());

            // Add new section.
            IniSection Settings = file.Sections.Add("Settings");
            // Add trailing comment.
            Settings.TrailingComment.Text = "Convars";

            // Add new key and its value.
            IniKey root = Settings.Keys.Add("Root", "./www");
            //Add leading comment.
            root.TrailingComment.Text = "Which port will the server use";
            root.LeftIndentation = Indentation;


            IniKey port = Settings.Keys.Add("Port", "8080");
            port.TrailingComment.Text = "Which port will the server use";
            port.LeftIndentation = Indentation;

            IniKey listDirectories = Settings.Keys.Add("ListDirectories", "true");
            listDirectories.TrailingComment.Text = "Defines if the directories without files will show it's content";
            listDirectories.LeftIndentation = Indentation;

            IniKey localhostOnly = Settings.Keys.Add("LocalhostOnly", "true");
            localhostOnly.TrailingComment.Text = "If true, server will be reachable from localhost only";
            localhostOnly.LeftIndentation = Indentation;

            IniKey logToConsole = Settings.Keys.Add("LogToConsole", "true");
            logToConsole.TrailingComment.Text = "If true, server will print activity in the console";
            logToConsole.LeftIndentation = Indentation;

            IniSection Debug = file.Sections.Add("Debug");
            Debug.TrailingComment.Text = "Debug Info";

            IniKey debugToConsole = Debug.Keys.Add("DebugToConsole", "true");
            debugToConsole.TrailingComment.Text = "If true, server will print debuggin info in the console";
            debugToConsole.LeftIndentation = Indentation;

            // Save file.
            try
            {
                file.Save(iniPath);
            }
            catch(System.IO.IOException e)
            {
                Console.WriteLine(e.Message);
            }
            return true;
        }
    }
}
