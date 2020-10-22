using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Threading;

namespace SFConnectorFullScreenUI
{
    public static class LoggerController
    {
        private static LogBar _logger;
        private static Dispatcher _dispatcher;
        internal static void Init(LogBar logger, Dispatcher dispatcher)
        {
            _logger = logger;
            _dispatcher = dispatcher;
            ShowStoredLogs();
        }

        internal static void Log(Exception e)
        {
            if (e == null) return;
            Log(e.StackTrace);
            Log(e.Message + Environment.NewLine + e.InnerException?.Message);
        }

        internal static void Log(Exception e, string customMessage)
        {
            Log(e);
            Log(customMessage);
        }

        private delegate void LogHandler(string arg);
        internal static void Log(string arg)
        {
            try
            {
                File.AppendAllText("log.txt", DateTime.Now + " " + arg + Environment.NewLine);
            }
            catch (Exception)
            {
            }
            if (string.IsNullOrEmpty(arg)) return;
            if (_dispatcher == null || _logger == null)
            {
                storedEntries.Add(arg);
                return;
            }
            // use dispatcher passed in
            if (_dispatcher.CheckAccess())
            {
                _logger.Log(arg);
            }
            else
            {
                _dispatcher.Invoke(new LogHandler(_logger.Log), arg);
            }
        }

        private static List<string> storedEntries = new List<string>();
        private static void ShowStoredLogs()
        {
            foreach (var item in storedEntries)
            {
                Log(item);
            }
        }

    }
}