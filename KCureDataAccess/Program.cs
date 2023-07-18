using DoctorGu;
using DoctorGu.Encryption;
using KCureDataAccess.Others;

namespace KCureVDIDataBox
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            CCommon.Log.WriteLog("Application Start");
            if (string.IsNullOrEmpty(CCommon.LoginId))
            {
                // 로그인
                LoginBox login = new LoginBox();
                DialogResult dret = login.ShowDialog();
                if (dret != DialogResult.OK)
                {
                    return;
                }
            }

            Application.Run(new MainForm());
            CCommon.Log.WriteLog("Application End");
        }
    }
}