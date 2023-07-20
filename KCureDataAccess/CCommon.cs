using DoctorGu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Resources;
using System.Configuration;
using KCureVDIDataBox.Model;
using System.IO.Compression;

namespace KCureVDIDataBox
{
    /// <summary>
    /// App.config 파일에서 사용된 키 목록
    /// </summary>
    public enum ConfigAppKeys
    {
        DefaultDirectoryClient,
        CompressWhenRequest,
        DefaultZipFileName,
        ShowLocalHost,

        ApiDomain,
        ApiKey,
        ApiValue,

        LoginUrl,
        AplyUrl,
    }

    /// <summary>
    /// 공통으로 사용되는 함수
    /// </summary>
    public class CCommon
    {
        public struct Const
        {
            /// <summary>사용자의 선택에 따라 변경되는 설정 파일명</summary>
            public const string ConfigXmlUser = "ConfigUser.xml";
            /// <summary>배포시에 확정되어 변경 불가능한 설정 파일명</summary>
            public const string ConfigXmlApp = "ConfigApp.xml";

            /// <summary>일반 연구자</summary>
            public const string USER_AU006 = "AU006";
            /// <summary>심의 의원</summary>
            public const string REVIEWER_AU005 = "AU005";
        }

        public static CLog Log = new CLog();
        public static CApi Api = new CApi();

        /// <summary>로그인 아이디</summary>
        public static string LoginId { get; set; }
        /// <summary>로그인 시에 localhost 사용을 선택했는 지 여부</summary>
        public static bool useLocalhost { get; set; }

        /// <summary>로그인 성공 시에 받아온 데이터</summary>
        public static LoginResult LoginResult { get; set; } = new LoginResult();

        /// <summary>사용자용 설정 파일 객체</summary>
        private static CXmlConfig _XmlConfigUser;

        static CCommon()
        {
            _XmlConfigUser = new CXmlConfig(GetConfigUserFullPath());

            LoginId = "";
        }

        /// <summary>
        /// 반출 신청에 사용될 기본 폴더를 생성
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultFullPath()
        {
            var drives = GetDrives();
            string drive = drives.First().Name.TrimEnd('\\');

            string dir = ReplaceVar(GetAppSetting(ConfigAppKeys.DefaultDirectoryClient, ""));

            string fullPath = $"{drive}\\{dir}";
            return fullPath;
        }

        /// <summary>
        /// App.config의 값에 있는 {id}와 {date} 변수를 실제 값으로 치환
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ReplaceVar(string value)
        {
            Regex r = new Regex("\\{date:([-yMd]+)\\}");

            value = value.Replace("{id}", LoginId);
            value = r.Replace(value, (Match m) => DateTime.Now.ToString(m.Groups[1].Value));
            return value;
        }

        /// <summary>
        /// 고정 드라이브 목록을 반환
        /// </summary>
        /// <returns></returns>
        /*public static List<DriveInfo> GetDrives()
        {
            return DriveInfo.GetDrives()
                .Where(drv => drv.DriveType == DriveType.Fixed)
                .ToList();
        }*/

        /// <summary>
        /// 고정 드라이브 목록을 반환
        /// </summary>
        /// <returns></returns>
        public static List<DriveInfo> GetDrives()
        {
            List<DriveInfo> allDrives = DriveInfo.GetDrives().ToList();
            List<DriveInfo> fixedDrives = allDrives.Where(drv => drv.DriveType == DriveType.Fixed).ToList();

            // 드라이브가 2개 이상인 경우 D 드라이브만 반환
            if (fixedDrives.Count >= 2)
            {
                return fixedDrives.Where(drv => drv.Name.StartsWith("D", StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return fixedDrives;
        }
        /// <summary>
        /// ConfigUser.xml의 전체 경로를 반환
        /// </summary>
        /// <returns></returns>
        public static string GetConfigUserFullPath()
        {
            return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath) ?? "", CCommon.Const.ConfigXmlUser);
        }

        /// <summary>
        /// TextBox에 로그를 기록하고, 로그 파일에도 기록함.
        /// </summary>
        /// <param name="txtLog"></param>
        /// <param name="text"></param>
        public static void WriteLog(TextBox txtLog, string text)
        {
            var rNewLine = new Regex("\r*\n");
            txtLog.AppendText(rNewLine.Replace(text, " ") + "\r\n");
            Log.WriteLog(text);
        }

        /// <summary>
        /// ConfigUser.xml 파일 객체를 반환
        /// </summary>
        public static CXmlConfig XmlConfigUser
        {
            get { return _XmlConfigUser; }
        }

        /// <summary>
        /// App.config 파일에서 키에 해당하는 값을 타입에 맞게 변환해서 반환
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configAppKey"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetAppSetting<T>(ConfigAppKeys configAppKey, T defaultValue)
        {
            string key = configAppKey.ToString();
            string value = ConfigurationManager.AppSettings[key] ?? "";
            if (value == "")
            {
                return defaultValue;
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// 파일을 압축하고 압축한 파일의 전체 경로를 반환
        /// </summary>
        /// <param name="localDirectory"></param>
        /// <returns></returns>
        public static string CreateZip(string localDirectory)
        {
            DateTime now = DateTime.Now;
            string formattedDate = now.ToString("yyyyMMddhhmmsss");
            //
            string zipFileName = CCommon.GetAppSetting<string>(ConfigAppKeys.DefaultZipFileName, "");
            zipFileName = zipFileName.Replace(".zip", "_" + formattedDate + ".zip");
            string zipFullPathTemp = $"{Path.GetTempPath()}{zipFileName}";
            if (File.Exists(zipFullPathTemp))
            {
                File.Delete(zipFullPathTemp);
            }

            string zipFullPath = Path.Combine(localDirectory, zipFileName);
            if (File.Exists(zipFullPath))
            {
                File.Delete(zipFullPath);
            }

            ZipFile.CreateFromDirectory(localDirectory, zipFullPathTemp);
            File.Move(zipFullPathTemp, zipFullPath, true);

            return zipFullPath;
        }

        /// <summary>
        /// 현재 Debug Mode인지, Release Mode인지를 확인함.
        /// 주의할 것은 DLL 안에 이 함수가 있다면 호출하는 실행파일이
        /// Debug Mode라도 Debug Mode가 아닌 것으로 리턴함.
        /// </summary>
        /// <returns></returns>
        /// <example>
        /// if (IsDebugMode())
        ///	 Console.WriteLine("Debug");
        /// else
        ///	 Console.WriteLine("Release");
        /// </example>
        public static bool IsDebugMode
        {
            get
            {
#if DEBUG
                return true;
#else
				return false;
#endif
            }
        }
    }
}
