using System;
using System.IO;
using Grimoire.Domain.Abstraction.Services;
using Grimoire.Domain.Models;

namespace Grimoire.Services
{
    public class LogService : ILogService
    {
        private IConfigurationService configuration;

        public LogService(IConfigurationService configuration)
        {
            this.configuration = configuration;
        }

        public void Log(Exception e)
        {
            try
            {
                SaveLog($"ERROR|{e.Message}|{e.StackTrace}");
            }
            catch (Exception)
            {
            }
        }

        public void Log(string message, LogLevel level)
        {
            try
            {
                string logLevel;
                switch (level)
                {
                    case LogLevel.INFO:
                        logLevel = "INFO";
                        break;

                    case LogLevel.WARN:
                        logLevel = "WARN";
                        break;

                    default:
                        logLevel = "ERROR";
                        break;
                }
                SaveLog($"{logLevel}|{message}");
            }
            catch (Exception)
            {
            }
        }

        private void SaveLog(string message)
        {
            string logFile = GetLogFileName();
            using (StreamWriter file = new StreamWriter(logFile, true))
            {
                string logMessage = $"{DateTime.UtcNow}|{message}";
                file.WriteLine(logMessage);
            }
        }

        private string GetLogFileName()
        {
            string logFolder = Path.Combine(configuration.WorkDirectory, "logs");
            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }
            return Path.Combine(logFolder, $"log_{DateTime.Now:yyyyMMdd}.log");
        }
    }
}