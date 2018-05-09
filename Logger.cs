using System;
using System.IO;

namespace pingdingding
{
    public class Logger
    {
        public readonly string LogfilePath;

        private string WithTimestamp(string row)
        {
            return $"{DateTime.Now.ToString("s")}\t{row}";
        }

        public Logger(string logPath)
        {
            this.LogfilePath = logPath == null
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "pingdingding.txt")
                : logPath;
        }

        public void Append(string row)
        {
            try
            {
                string logRow = WithTimestamp(row);
                using (var writer = File.AppendText(this.LogfilePath))
                {
                    Console.WriteLine(logRow);
                    writer.WriteLine(logRow);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(this.WithTimestamp($"ERROR\tFailed to append to logfile: #{exc.Message}"));
            }
        }
    }
}