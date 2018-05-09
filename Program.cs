using System;
using System.Threading;
using System.Threading.Tasks;

namespace pingdingding
{
    class Program
    {
        static void Main(string[] args)
        {
            var interval = args.Length >= 1 ? int.Parse(args[0]) : 60;
            var logPath = args.Length == 2 ? args[1] : null;
            var pinger = new Pinger();
            var logger = new Logger(logPath);

            logger.Append($"START\tInterval {interval}s, log file {logger.LogfilePath}");

            while (true)
            {
                var result = pinger.Ping().Result;
                if (result.StateChanged)
                    logger.Append(result.Message);

                Task.Delay(1000 * interval).Wait();
            }
        }
    }
}
