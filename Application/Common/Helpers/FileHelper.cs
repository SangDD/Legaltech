namespace CommonData
{
    using System;
    using System.Threading;

    public class FileHelper
    {
        private static readonly ReaderWriterLockSlim s_readWriteLock = new ReaderWriterLockSlim();
        private static readonly object s_lockFileLogin = new object();

        public static void WriteFileLogin(string filePath, string accountName, string ipAddress)
        {
            try
            {
                lock (s_lockFileLogin)
                {
                    s_readWriteLock.EnterWriteLock();
                    var content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + accountName + " - " + ipAddress;
                    try
                    {
                        using (var sw = System.IO.File.AppendText(filePath))
                        {
                            sw.WriteLine(content);
                            sw.Close();
                        }
                    }
                    finally
                    {
                        s_readWriteLock.ExitWriteLock();
                    }
                }
            }
            catch (Exception ex)
            {
                LogInfo.LogException(ex);
            }
        }
    }
}
