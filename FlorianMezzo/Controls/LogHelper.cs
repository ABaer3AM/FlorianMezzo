using System;
using System.IO;

namespace FlorianMezzo.Controls
{
    public static class LogHelper
    {
        private static readonly string LogFilePath = Path.Combine(FileSystem.AppDataDirectory, "errorlog.txt");

        public static void WriteLog(string message)
        {
            try
            {
                using (StreamWriter writer = File.AppendText(LogFilePath))
                {
                    writer.WriteLine($"{DateTime.Now}: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to log: {ex.Message}");
            }
        }
    }
}