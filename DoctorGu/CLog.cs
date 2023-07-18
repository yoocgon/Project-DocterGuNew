using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DoctorGu
{
    public class CWriteLogEvent : EventArgs
    {
        public string Log;
    }

    public enum LogFileNameFormat
    {
        yyyyMMdd,
        yyyyMMddHH,
    }

    public enum LogFileNamePrefix
    {
        Success,
        Fail,
        Error,
        Task,
        Test,
    }

    /// <summary>
    /// <![CDATA[
    /// for (int i = 0; i < 100; i++)
    /// {
    ///	 Thread th = new Thread(WriteLogThread);
    ///	 th.Name = "th" + i.ToString();
    ///	 th.Start();
    /// }			
    /// private static void WriteLogThread()
    /// {
    ///		CLog Log = new CLog();
    ///	 for (int i = 0; i < 100; i++)
    ///	 {
    ///		 string s = Thread.CurrentThread.Name + " " + i.ToString() + " " + DateTime.Now.ToString();
    ///		 Log.WriteLog(LogTypes.Test, s);
    ///	 }
    /// }
    /// ]]>
    /// </summary>
    public class CLog
    {
        private string _LogFolder;
        private bool _LogOnly;
        private int _DaysBeforeToDelete;
        private bool _DeletedFilesOlderThan;

        private ReaderWriterLockSlim _Locker = new ReaderWriterLockSlim();

        public event EventHandler<CWriteLogEvent> BeforeWriteLog;

        public CLog(string LogFolder, bool LogOnly, int DaysBeforeToDelete)
        {
            if (!string.IsNullOrEmpty(LogFolder))
                _LogFolder = LogFolder;
            else
                _LogFolder = GetDefaultLogFolder();

            if (!Directory.Exists(_LogFolder))
                Directory.CreateDirectory(_LogFolder);

            _LogOnly = LogOnly;
            _DaysBeforeToDelete = DaysBeforeToDelete;
        }
        public CLog(bool LogOnly, int DaysBeforeToDelete) : this(null, LogOnly, DaysBeforeToDelete) { }
        public CLog(bool LogOnly) : this(null, LogOnly, 0) { }
        public CLog() : this(null, false, 0) { }

        private string GetDefaultLogFolder()
        {
            Assembly Assem = CAssembly.GetEntryOrExecuting();
            if (Assem == null)
                throw new Exception("Assem is null");

            string AppFolder;
            //If it is ruuning in IIS, use CodeBase property instead of Location property to get IIS root folder.
            if (Assem.Location.IndexOf(@"\Temporary ASP.NET Files\", StringComparison.CurrentCultureIgnoreCase) != -1)
            {
                AppFolder = Path.GetDirectoryName(Assem.CodeBase).Replace(@"file:\", "");
                if (AppFolder.EndsWith(@"\bin"))
                    AppFolder = AppFolder.Substring(0, (AppFolder.Length - @"\bin".Length));
            }
            else
            {
                AppFolder = CAssembly.GetFolder(Assem);
            }

            string AppName = Assem.GetName().Name;
            return Path.Combine(AppFolder, @"Log\" + AppName);
        }

        private void DeleteFilesOlderThan(string LogFolder, int DaysBefore)
        {
            CFile.DeleteOldFiles(LogFolder, DaysBefore);
            CFile.DeleteFolderHasNoFile(LogFolder);
        }

        private string GetLogFullPath(Enum FileNamePrefix, LogFileNameFormat FileNameFormat)
        {
            string FileName = string.Format("{0}{1}.log", FileNamePrefix, DateTime.Now.ToString(FileNameFormat.ToString()));

            return Path.Combine(_LogFolder, FileName);
        }

        public void WriteLogWithPath(string LogFullPath, string log)
        {
            Log("WriteLog", log);

            if (BeforeWriteLog != null)
                BeforeWriteLog(null, new CWriteLogEvent() { Log = log });

            string LogFolder = Path.GetDirectoryName(LogFullPath);
            if (!Directory.Exists(LogFolder))
                Directory.CreateDirectory(LogFolder);

            if (!_LogOnly)
                log = "==================================================" + CConst.White.RN
                    + DateTime.Now.ToString(CConst.Format_yyyy_MM_dd_HH_mm_ss_fff) + CConst.White.RN
                    + log + CConst.White.RN;

            _Locker.EnterWriteLock();
            try
            {
                File.AppendAllText(LogFullPath, log);

                if ((_DaysBeforeToDelete > 0) && !_DeletedFilesOlderThan)
                {
                    DeleteFilesOlderThan(LogFolder, _DaysBeforeToDelete);
                    _DeletedFilesOlderThan = true;
                }
            }
            finally
            {
                _Locker.ExitWriteLock();
            }
        }
        public void WriteLogWithPath(string LogFullPath, Exception ex)
        {
            string Log = CInfo.GetExceptionText(ex);
            WriteLogWithPath(LogFullPath, Log);
        }
        public void WriteLog(Enum FileNamePrefix, string Log, LogFileNameFormat FileNameFormat)
        {
            string LogFullPath = GetLogFullPath(FileNamePrefix, FileNameFormat);
            WriteLogWithPath(LogFullPath, Log);
        }
        public void WriteLog(string Log)
        {
            LogFileNameFormat FileNameFormat = LogFileNameFormat.yyyyMMdd;
            WriteLog(null, Log, FileNameFormat);
        }
        public void WriteLog(Enum FileNamePrefix, string Log)
        {
            LogFileNameFormat FileNameFormat = LogFileNameFormat.yyyyMMdd;
            WriteLog(FileNamePrefix, Log, FileNameFormat);
        }
        public void WriteLog(Exception ex, LogFileNameFormat FileNameFormat)
        {
            string Log = CInfo.GetExceptionText(ex);
            WriteLog(LogFileNamePrefix.Error, Log, FileNameFormat);
        }
        public void WriteLog(Exception ex)
        {
            string Log = CInfo.GetExceptionText(ex);
            LogFileNameFormat FileNameFormat = LogFileNameFormat.yyyyMMdd;
            WriteLog(LogFileNamePrefix.Error, Log, FileNameFormat);
        }
        public void WriteLog(Enum FileNamePrefix, Exception ex)
        {
            string Log = CInfo.GetExceptionText(ex);
            LogFileNameFormat FileNameFormat = LogFileNameFormat.yyyyMMdd;
            WriteLog(FileNamePrefix, Log, FileNameFormat);
        }
        public void WriteLog(string Log, Exception ex, LogFileNameFormat FileNameFormat)
        {
            Log = "Log:" + Log + CConst.White.RN + CInfo.GetExceptionText(ex);
            WriteLog(LogFileNamePrefix.Error, Log, FileNameFormat);
        }
        public void WriteLog(string Log, Exception ex)
        {
            Log = "Log:" + Log + CConst.White.RN + CInfo.GetExceptionText(ex);
            LogFileNameFormat FileNameFormat = LogFileNameFormat.yyyyMMdd;
            WriteLog(LogFileNamePrefix.Error, Log, FileNameFormat);
        }

        public void Log(string category, string log)
        {
            Console.WriteLine($"\nDEBUG>>> ({category}) \n{log}");
        }
    }
}

