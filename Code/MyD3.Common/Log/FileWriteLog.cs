using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace MyD3.Common.Log
{
    /// <summary>
    /// 暂时不用NLog4 替代
    /// </summary>
    public static class FileWriteLog
    {
        private static string logfolder = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ce_log";
        private const string spliteLine = "---------------------------------------------------------------------------\r\n";

        static FileWriteLog()
        {
            try
            {
                //在web.config文件里面配置,同时确保该目录能写
                string logFileStr = ConfigurationManager.AppSettings["logfolder"];
                if (!string.IsNullOrEmpty(logFileStr))
                {
                    logfolder = logFileStr;
                }
                if (!System.IO.Directory.Exists(logfolder))
                {
                    System.IO.Directory.CreateDirectory(logfolder);
                }
            }
            catch
            {
            }
        }

        public static void LogError(string msg)
        {
            try
            {
                Write(DateTime.Now.ToLongTimeString() + ":" + msg + Environment.NewLine + spliteLine);
            }
            catch
            {
            }
        }

        public static void LogInformation(string msg)
        {
            try
            {
                Write(DateTime.Now.ToLongTimeString() + ":" + msg + Environment.NewLine + spliteLine);
            }
            catch
            {
            }
        }

        public static void LogException(Exception ex)
        {
            try
            {
                Write(DateTime.Now.ToLongTimeString() + ":ExecptionMessage:" + ex.Message + Environment.NewLine);
                Write(DateTime.Now.ToLongTimeString() + ":Source:" + ex.Source + Environment.NewLine);
                Write(DateTime.Now.ToLongTimeString() + ":StackTrace:" + ex.StackTrace + Environment.NewLine);
                Write(spliteLine);
            }
            catch
            {
            }
        }

        private static void Write(string content)
        {
            try
            {
                string logfile = logfolder + string.Format("\\oa_{0}{1:D2}{2:D2}.log", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                using (StreamWriter wrtier = new StreamWriter(logfile, true, Encoding.UTF8))
                {
                    wrtier.Write(content);
                }
            }
            catch
            {
            }
        }
    }
}