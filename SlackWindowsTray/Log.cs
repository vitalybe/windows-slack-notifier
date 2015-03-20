using System;
using System.IO;
using System.Reflection;

namespace SlackWindowsTray
{
    static class Log
    {

        private static readonly TextWriter LogFile;

        static Log()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            var logFileName = Path.Combine(directoryName, "SlackWindowsTray.log");
            LogFile = new StreamWriter(new FileStream(logFileName, FileMode.OpenOrCreate));
        }

        public static void Write(string text)
        {
            var logLine = string.Format("[{0}] {1}", DateTime.Now.ToShortTimeString(), text);

            Console.WriteLine(logLine);
            LogFile.WriteLine(logLine);
            LogFile.Flush();
        }
    }
}