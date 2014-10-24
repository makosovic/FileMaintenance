using System;
using System.IO;

namespace LogMaintenance.Logging
{
    public class LoggingService : ILoggingService
    {

        #region private fields

        private string _path;

        #endregion

        #region constructors

        public LoggingService()
        {
            string configPath = System.Configuration.ConfigurationManager.AppSettings["Log.Path"];
            _path = Path.GetFullPath(configPath);
        }

        public LoggingService(string path)
        {
            _path = Path.GetFullPath(path);
        }

        #endregion

        #region public methods

        public void HandleException(Exception ex)
        {
            WriteToFile(ex.ToString());
        }

        public void HandleMessage(string message)
        {
            WriteToFile(message);
        }

        #endregion

        #region private methods

        private void WriteToFile(String message)
        {
            if (!String.IsNullOrEmpty(_path))
            {
                FileStream file = null;
                StreamWriter writer = null;

                try
                {
                    Directory.CreateDirectory(_path);

                    if ((_path.EndsWith("/") == false) && (_path.EndsWith("\\") == false)) _path += "\\";
                    _path = String.Concat(_path, DateTime.Now.Year.ToString(), "-", DateTime.Now.Month.ToString().PadLeft(2, '0'), "-", DateTime.Now.Day.ToString().PadLeft(2, '0'), ".txt");

                    file = File.Open(_path, FileMode.Append);

                    writer = new StreamWriter(file);
                    writer.WriteLine(" Vrijeme: " + DateTime.Now.ToString());
                    writer.WriteLine(String.Empty);
                    writer.WriteLine(message);
                    writer.WriteLine("----------------------------------------------------------------------------------------------------");
                    writer.WriteLine(String.Empty);

                    writer.Flush();
                    writer.Close();
                    file.Close();
                }
                catch
                {
                    if (writer != null) writer.Close();
                    if (file != null) file.Close();
                }
            }
        }

        #endregion

    }
}
