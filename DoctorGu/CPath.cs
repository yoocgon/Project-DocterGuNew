using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace DoctorGu
{
    public class CFullPathFullUrl
    {
        public string FullPath;
        public string FullUrl;
    }

    public class CPath
    {
        /// <summary> 
        /// The type of structure that the function stores in the buffer. 
        /// </summary> 
        private enum InfoLevel
        {
            /// <summary> 
            /// The function stores a <see cref="UNIVERSAL_NAME_INFO"/> structure in the buffer. 
            /// </summary> 
            UniversalName = 1,

            /// <summary> 
            /// The function stores a <c>REMOTE_NAME_INFO</c> structure in the buffer. 
            /// </summary> 
            /// <remarks> 
            /// Using this level will throw an <see cref="NotSupportedException"/>. 
            /// </remarks> 
            RemoteName = 2
        }

        /// <summary> 
        /// The <see cref="WNetGetUniversalName(string,int,UNIVERSAL_NAME_INFO,int)"/> function 
        /// takes a drive-based path for a network resource and returns an information 
        /// structure that contains a more universal form of the name. 
        /// </summary> 
        /// <param name="lpLocalPath">A pointer to a constant null-terminated string that 
        /// is a drive-based path for a network resource.</param> 
        /// <param name="dwInfoLevel">The type of structure that the function stores in 
        /// the buffer pointed to by the <paramref name="lpBuffer"/> parameter.</param> 
        /// <param name="lpBuffer">A pointer to a buffer that receives the structure 
        /// specified by the <paramref name="dwInfoLevel"/> parameter.</param> 
        /// <param name="lpBufferSize">A pointer to a variable that specifies the size, 
        /// in bytes, of the buffer pointed to by the <paramref name="lpBuffer"/> parameter.</param> 
        /// <returns>If the function succeeds, the return value is <see cref="NO_ERROR"/>.</returns> 
        [DllImport("mpr.dll", CharSet = CharSet.Auto)]
        private static extern int WNetGetUniversalName(string lpLocalPath, InfoLevel dwInfoLevel, ref UNIVERSAL_NAME_INFO lpBuffer, ref int lpBufferSize);

        /// <summary> 
        /// The <see cref="WNetGetUniversalName(string,int,IntPtr,int)"/> function 
        /// takes a drive-based path for a network resource and returns an information 
        /// structure that contains a more universal form of the name. 
        /// </summary> 
        /// <param name="lpLocalPath">A pointer to a constant null-terminated string that 
        /// is a drive-based path for a network resource.</param> 
        /// <param name="dwInfoLevel">The type of structure that the function stores in 
        /// the buffer pointed to by the <paramref name="lpBuffer"/> parameter.</param> 
        /// <param name="lpBuffer">A pointer to a buffer that receives the structure 
        /// specified by the <paramref name="dwInfoLevel"/> parameter.</param> 
        /// <param name="lpBufferSize">A pointer to a variable that specifies the size, 
        /// in bytes, of the buffer pointed to by the <paramref name="lpBuffer"/> parameter.</param> 
        /// <returns>If the function succeeds, the return value is <see cref="NO_ERROR"/>.</returns> 
        [DllImport("mpr.dll", CharSet = CharSet.Auto)]
        private static extern int WNetGetUniversalName(string lpLocalPath, InfoLevel dwInfoLevel, IntPtr lpBuffer, ref int lpBufferSize);

        /// <summary> 
        /// The <see cref="UNIVERSAL_NAME_INFO"/> structure contains a pointer to a 
        /// Universal Naming Convention (UNC) name string for a network resource. 
        /// </summary> 
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct UNIVERSAL_NAME_INFO
        {
            /// <summary> 
            /// Pointer to the null-terminated UNC name string that identifies a network resource. 
            /// </summary> 
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpUniversalName;
        }

        /// <summary>
        /// 파일의 폴더와 파일에서 폴더만을 리턴함.
        /// </summary>
        /// <param name="FullPath">전체 경로</param>
        /// <param name="DirectorySeparator">경로 구분자(\ 또는 /)</param>
        /// <returns>폴더명</returns>
        /// <example>
        /// <code>
        /// Console.WriteLine(CPath.GetFolderName(@"http://doctorgu.ddns.co.kr/images/test.gif", '/')); //http://doctorgu.ddns.co.kr/images
        /// Console.WriteLine(CPath.GetFolderName(@"C:\Temp\Test.txt")); //C:\Temp
        /// </code>
        /// </example>
        public static string GetFolderName(string FullPath, char DirectorySeparator)
        {
            //오른쪽에서 왼쪽으로 첫번째 \ 기호를 찾음.
            //그 앞의 모든 문자열은 경로이므로 그것을 리턴함.
            int Pos = FullPath.LastIndexOf(DirectorySeparator);
            return (Pos != -1 ? FullPath.Substring(0, Pos) : "");
        }
        /// <summary>
        /// 파일의 경로와 파일에서 경로명을 리턴함. 경로 구분자는 시스템에서 사용되는 \ 또는 / 등이 자동으로 사용됨.
        /// </summary>
        /// <param name="FullPath">전체 경로</param>
        /// <returns>경로명</returns>
        public static string GetFolderName(string FullPath)
        {
            return GetFolderName(FullPath, Path.DirectorySeparatorChar);
        }
        /// <summary>
        /// 파일의 경로와 파일에서 파일명을 리턴함.
        /// </summary>
        /// <param name="FullPath">전체 경로</param>
        /// <param name="DirectorySeparator">경로 구분자(\ 또는 /)</param>
        /// <returns>파일명</returns>
        /// <example>
        /// <code>
        /// Console.WriteLine(CPath.GetFileName(@"http://doctorgu.ddns.co.kr/images/test.gif", '/')); //test.gif
        /// Console.WriteLine(CPath.GetFileName(@"C:\Temp\Test.txt")); //Test.txt
        /// </code>
        /// </example>
        public static string GetFileName(string FullPath, char DirectorySeparator)
        {
            //오른쪽에서 왼쪽으로 첫번째 \ 기호를 찾음.
            //그 뒤의 모든 문자열은 파일이므로 그것을 리턴함.
            int Pos = FullPath.LastIndexOf(DirectorySeparator);
            return (Pos != -1 ? FullPath.Substring(Pos + 1) : FullPath);
        }
        /// <summary>
        /// 파일의 경로와 파일에서 파일명을 리턴함.
        /// </summary>
        /// <param name="FullPath">전체 경로</param>
        /// <returns>파일명</returns>
        public static string GetFileName(string FullPath)
        {
            return GetFileName(FullPath, Path.DirectorySeparatorChar);
        }

        public static string GetExtension(string FullPath, char DirectorySeparator)
        {
            string FileName = GetFileName(FullPath, DirectorySeparator);

            int PosDot = FileName.LastIndexOf(".");

            //Path.GetExtension은 마지막에 .이 있으면 ""를 리턴하므로 같은 결과를 표시하기 위함.
            //참고로 윈도우 운영체제에서 파일 이름에 마지막에 .을 넣으면 자동으로 삭제됨.
            if (PosDot == (FileName.Length - 1))
                PosDot = -1;

            return (PosDot != -1 ? FileName.Substring(PosDot) : "");
        }
        public static string GetExtension(string FullPath)
        {
            return GetExtension(FullPath, Path.DirectorySeparatorChar);
        }

        /// <remarks>
        /// FileName in Windows can have slash(/), So Path.GetFileNameWithoutExtension can be different.
        /// </remarks>
        /// <example>
        /// Path.GetFileNameWithoutExtension("AklWwIyZeqbd/2s9p9U44OZefoc=,기출문제 05회.hml"); // 2s9p9U44OZefoc=,기출문제 05회
        /// CPath.GetFileNameWithoutExtension("AklWwIyZeqbd/2s9p9U44OZefoc=,기출문제 05회.hml"); // AklWwIyZeqbd/2s9p9U44OZefoc=,기출문제 05회
        /// </example>
        public static string GetFileNameWithoutExtension(string FullPath, char DirectorySeparator)
        {
            string FileName = GetFileName(FullPath, DirectorySeparator);

            int PosDot = FileName.LastIndexOf(".");
            return (PosDot != -1) ? FileName.Substring(0, PosDot) : FileName;
        }
        public static string GetFileNameWithoutExtension(string FullPath)
        {
            return GetFileNameWithoutExtension(FullPath, Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// 파일의 파일명과 확장자에서 확장자를 리턴함.
        /// </summary>
        /// <param name="FileExt">확장자를 포함한 파일명</param>
        /// <returns>확장자</returns>
        /// <example>
        /// <code>
        /// Console.WriteLine(CPath.GetExtFromFileExt("Test.txt")); //txt
        /// </code>
        /// </example>
        public static string GetExtFromFileExt(string FileExt)
        {
            //오른쪽에서 왼쪽으로 첫번째 . 기호를 찾음.
            //그 뒤의 모든 문자열은 파일명을 제외한 확장자이므로 그것을 리턴함.
            int PosDot = FileExt.LastIndexOf(".");
            return (PosDot != -1 ? FileExt.Substring(PosDot + 1) : "");
        }

        /// <summary>
        /// Return parent path
        /// </summary>
        /// <param name="PathDest">Path itself</param>
        /// <param name="PathSep">Path separator</param>
        /// <returns>parent path of <paramref name="PathDest"/></returns>
        /// <example>
        /// <code>
        /// Console.WriteLine(CPath.GetParentPath("c:\windows\system\", @"\")); //c:\windows
        /// </code>
        /// </example>
        public static string GetParentPath(string PathDest, char PathSep)
        {
            //Remove "\" if path ends with "\"
            if (PathDest[PathDest.Length - 1] == PathSep)
                PathDest = PathDest.Substring(0, PathDest.Length - 1);

            //Find "\" character from right to left order, then return all characters at the left of found "\" character.
            for (int i = (PathDest.Length - 1); i >= 0; i--)
            {
                char c = PathDest[i];
                if (c == PathSep)
                {
                    string PathNew = PathDest.Substring(0, i);
                    if (PathNew.EndsWith(":"))
                        PathNew += PathSep;

                    return PathNew;
                }
            }

            return "";
        }
        public static string GetParentPath(string PathDest)
        {
            return GetParentPath(PathDest, Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// Url에서 서버의 주소만을 가져옴.
        /// </summary>
        /// <param name="Url">전체 URL</param>
        /// <param name="SubFolderIs">서버 주소 제외한 경로</param>
        /// <param name="ParamIs">파라미터 값</param>
        /// <returns>서버 주소</returns>
        /// <example>
        /// <code>
        /// string SubFolderIs, ParamIs;
        /// string ServerUrl = CPath.GetServerUrl("http://www.happymoney.co.kr/board/list.aspx?kind=1", out SubFolderIs, out ParamIs);
        /// 
        /// Console.WriteLine(ServerUrl); // "www.happymoney.co.kr"
        /// Console.WriteLine(SubFolderIs); // "/board/list.aspx"
        /// Console.WriteLine(ParamIs); // "kind=1"
        /// </code>
        /// </example>
        public static string GetServerUrl(string Url, out string AbsolutePathIs, out string ParamIs)
        {
            AbsolutePathIs = "";
            ParamIs = "";

            Uri u = null;
            try
            {
                u = new Uri(Url);
            }
            catch (Exception) { }

            if (u == null)
            {
                return "";
            }

            string UrlNew = u.Scheme + "://" + u.Host;
            if (u.Port != 80)
            {
                UrlNew += ":" + u.Port.ToString();
            }

            AbsolutePathIs = u.AbsolutePath;

            if (u.Query != "")
            {
                //?a=1234 -> a=1234
                ParamIs = u.Query.Substring(1);
            }

            return UrlNew;

            /*
			SubFolderIs = "";
			ParamIs = "";

			//http://에 있는 //의 위치를 알아냄
			int SlashPos = Url.IndexOf("//");
			if (SlashPos != -1)
			{
				Url = Url.Substring(SlashPos + 2);
			}

			SlashPos = Url.IndexOf("/");
			if (SlashPos != -1)
			{
				SubFolderIs = Url.Substring(SlashPos + 1);
				Url = Url.Substring(0, SlashPos);

				int QuestPos = SubFolderIs.IndexOf("?");
				if (QuestPos != -1)
				{
					ParamIs = SubFolderIs.Substring(QuestPos + 1);
					SubFolderIs = SubFolderIs.Substring(0, QuestPos);
				}

				return Url;
			}
			else
			{
				return Url;
			}
			 */
        }
        /// <summary>
        /// Url에서 서버의 주소만을 가져옴.
        /// </summary>
        /// <param name="Url">전체 URL</param>
        /// <returns>서버 주소</returns>
        /// <example>
        /// <code>
        /// string ServerUrl = CPath.GetServerUrl("http://www.happymoney.co.kr/board/list.aspx?kind=1");
        /// Console.WriteLine(ServerUrl); // "www.happymoney.co.kr"
        /// </code>
        /// </example>
        public static string GetServerUrl(string Url)
        {
            string SubFolderIs, ParamIs;
            return GetServerUrl(Url, out SubFolderIs, out ParamIs);
        }

        public static string GetVariableReplacedFolder(string Folder, string ExecutableFolder)
        {
            NameValueCollection nvVars = new NameValueCollection();

            Folder = Folder.Replace("%MainDir%", ExecutableFolder);
            Folder = GetFolderPathReplaced(Folder);
            Folder = GetEnvironmentVariableReplaced(Folder);

            string DriveName = Path.GetPathRoot(Folder);
            List<DriveInfo> adiFixed = DriveInfo.GetDrives().Where(drv => drv.DriveType == DriveType.Fixed).ToList();
            DriveInfo diFound = adiFixed.FirstOrDefault(di => (string.Compare(di.Name, DriveName) == 0));
            if (diFound == null)
            {
                string DriveNameNew = adiFixed[0].Name;
                Folder = DriveNameNew + Folder.Substring(DriveName.Length);
            }

            return Folder;
        }

        // "%QuickLaunch%" -> "C:\Documents and Settings\Administrator\Application Data\Microsoft\Internet Explorer\Quick Launch"
        public static string GetFolderPathReplaced(string Value)
        {
            Regex r = new Regex(@"%\w+%", RegexOptions.Compiled);
            StringBuilder sb = new StringBuilder();
            foreach (var m in CRegex.GetMatchResult(r, Value))
            {
                sb.Append(m.ValueBeforeMatch);
                if (m.Match != null)
                {
                    string Variable = m.Match.Value;
                    string Result = Environment.GetFolderPath(CEnum.GetValueByName<Environment.SpecialFolder>(Variable.Trim('%')));
                    if (Result != null)
                        sb.Append(Result);
                    else
                        sb.Append(Variable);
                }
            }

            return sb.ToString();
        }

        // "%APPDATA%\Notepad++\plugins\config" -> "C:\Users\sdv\AppData\Roaming\Notepad++\plugins\config"
        public static string GetEnvironmentVariableReplaced(string Value)
        {
            Regex r = new Regex(@"%\w+%", RegexOptions.Compiled);
            StringBuilder sb = new StringBuilder();
            foreach (var m in CRegex.GetMatchResult(r, Value))
            {
                sb.Append(m.ValueBeforeMatch);
                if (m.Match != null)
                {
                    string Variable = m.Match.Value;
                    string Result = Environment.GetEnvironmentVariable(Variable.Trim('%'));
                    if (Result != null)
                        sb.Append(Result);
                    else
                        sb.Append(Variable);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// [시작] - [프로그램] 폴더 안의 특정 바로가기 파일이 속한 프로그램 그룹 폴더의 위치를 리턴함.
        /// </summary>
        /// <param name="ShortcutFileNoExt">확장자를 제외한 바로가기 파일 이름</param>
        /// <returns>프로그램 그룹 폴더의 위치</returns>
        /// <example>
        /// 다음은 문자표.lnk가 위치한 프로그램 그룹의 폴더 위치를 출력합니다.
        /// <code>
        /// string[] aFolder = CFile.GetProgramFolderHasShortcutOf("문자표");
        /// Console.WriteLine(aFolder[0]); // "C:\Documents and Settings\All Users\시작 메뉴\프로그램\보조프로그램\시스템 도구"
        /// </code>
        /// </example>
        public static string[] GetProgramFolderHasShortcutOf(string ShortcutFileNoExt)
        {
            List<string> aFolder = new List<string>();

            for (int i = 0; i < 2; i++)
            {
                string ProgramsFolder = "";
                if (i == 0)
                {
                    ProgramsFolder = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
                }
                else
                {
                    ProgramsFolder = CPath.GetCommonProgramsFolder();
                }

                FileInfo[] afi = CFile.FindFiles(ProgramsFolder, ShortcutFileNoExt + ".lnk", true);

                foreach (FileInfo fi in afi)
                {
                    aFolder.Add(CPath.GetFolderName(fi.FullName));
                }
            }

            return aFolder.ToArray();
        }

        private static string GetCommonProgramsFolder()
        {
            //Administrator
            string UserName = Environment.UserName;

            //C:\Documents and Settings\Administrator\시작 메뉴\프로그램
            string Programs = Environment.GetFolderPath(Environment.SpecialFolder.Programs);

            int Pos = Programs.IndexOf(@"\" + UserName + @"\");
            //C:\Documents and Settings
            string DocSet = Programs.Substring(0, Pos);
            //시작 메뉴\프로그램
            string StartMenu = Programs.Substring(Pos + (@"\" + UserName + @"\").Length);

            //C:\Documents and Settings\All Users\시작 메뉴\프로그램
            string CommonProgramsFolder = DocSet + @"\All Users\" + StartMenu;

            return CommonProgramsFolder;
        }

        /// <summary>
        /// 파일이름으로 적합하지 않은 문자열을 <paramref name="ReplaceChar"/> 값으로 변경함.
        /// </summary>
        /// <param name="FileName">파일 이름</param>
        /// <param name="ReplaceChar">파일 이름에 허용되지 않은 문자열을 이 값으로 변경함.</param>
        /// <returns>변경된 파일 이름</returns>
        /// <example>
        /// <code>
        /// string s = CFile.GetSafeFileName(@"you|me", '_');
        /// Console.WriteLine(s); // "you_me"
        /// </code>
        /// </example>
        public static string GetSafeFileName(string FileName, char ReplaceChar)
        {
            string DisallowedList = CValid.DisallowedListForFileName;

            if (DisallowedList.IndexOf(ReplaceChar) != -1)
                throw new Exception(DisallowedList + "는 ReplaceChar 인수로 사용할 수 없습니다.");

            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(DisallowedList)));
            string FileNameNew = r.Replace(FileName, ReplaceChar.ToString());

            //파일명, 폴더명의 마지막에 점은 허용되지 않음.
            if (FileNameNew.EndsWith("."))
            {
                FileNameNew = FileNameNew.Substring(0, FileNameNew.Length - 1) + ReplaceChar;
            }

            //파일명, 폴더명의 양쪽에 스페이스는 허용되지 않음.
            if (FileNameNew.StartsWith(" "))
            {
                FileNameNew = ReplaceChar + FileNameNew.Substring(1);
            }
            if (FileNameNew.EndsWith(" "))
            {
                FileNameNew = FileNameNew.Substring(0, FileNameNew.Length - 1) + ReplaceChar;
            }

            return FileNameNew;
        }
        /// <summary>
        /// Response.AddHeader에서 ;은 구분자이므로 ;은 %3b로 변경함.
        /// 크롬 브라우저의 경우, Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName);
        /// 부분의 FileName에 ,+ 문자열이 있으면 '서버에서 중복 헤더를 수신했습니다.' 에러 발생함.
        /// 그러므로 해당 문자열을 변경함.
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="ReplaceChar"></param>
        /// <returns></returns>
        public static string GetSafeFileNameForAddHeader(string FileName, char ReplaceChar)
        {
            string FileNameNew = GetSafeFileName(FileName, ReplaceChar);

            string DisallowedList = ";,+";

            if (DisallowedList.IndexOf(ReplaceChar) != -1)
                throw new Exception(DisallowedList + "는 ReplaceChar 인수로 사용할 수 없습니다.");

            FileNameNew = FileNameNew
                .Replace(";", "%3b")
                .Replace(',', ReplaceChar)
                .Replace('+', ReplaceChar);
            FileNameNew = HttpUtility.UrlPathEncode(FileNameNew);

            return FileNameNew;
        }

        /// <summary>
        /// 각 경로를 배열로 리턴함.
        /// </summary>
        /// <param name="FullPath">전체 경로</param>
        /// <returns>각 경로를 항목으로 하는 배열</returns>
        /// <example>
        /// <code>
        /// string[] aPath = CPath.GetAllPathInArray(@"C:\T1\T2");
        /// Console.WriteLine(aPath[0]); // "C:"
        /// Console.WriteLine(aPath[1]); // "T1"
        /// Console.WriteLine(aPath[2]); // "T2"
        /// </code>
        /// </example>
        public static string[] GetAllPathInArray(string FullPath, char DirectorySeparator, StringSplitOptions options)
        {
            if (FullPath == "") return null;

            if (FullPath.Substring(FullPath.Length - 1)[0] == DirectorySeparator)
            {
                FullPath = FullPath.Substring(0, FullPath.Length - 1);
            }

            return FullPath.Split(new char[] { DirectorySeparator }, options);
        }
        public static string[] GetAllPathInArray(string FullPath)
        {
            return GetAllPathInArray(FullPath, Path.DirectorySeparatorChar, StringSplitOptions.None);
        }

        /// <summary>
        /// 가장 마지막 폴더 이름을 리턴함. 실제로 폴더, 파일의 존재 여부는 확인하지 않으므로
        /// 마지막이 파일 이름인 경우는 파일 이름을 리턴하게 됨.
        /// </summary>
        /// <param name="FullPath">전체 경로</param>
        /// <param name="DirectorySeparator">경로 구분자</param>
        /// <returns>마지막 폴더 이름</returns>
        /// <example>
        /// <code>
        /// //파일명을 가져옵니다.
        /// string s = CFile.GetLastFolder("http://localhost:89/manager/test.aspx", '/');
        /// Console.WriteLine(s); //"test.aspx"
        /// 
        /// //마지막 폴더명을 가져옵니다.
        /// string s2 = CFile.GetLastFolder(CPath.GetFolderName("http://localhost:89/manager/test.aspx", '/'), '/');
        /// Console.WriteLine(s2);
        /// </code>
        /// </example>
        public static string GetLastFolder(string FullPath, char DirectorySeparator)
        {
            if (FullPath == "") return "";

            if (FullPath.Substring(FullPath.Length - 1)[0] == DirectorySeparator)
            {
                FullPath = FullPath.Substring(0, FullPath.Length - 1);
            }

            int PosStart = FullPath.LastIndexOf(DirectorySeparator) + 1;
            if (PosStart == 0)
                return "";

            return FullPath.Substring(PosStart);
        }
        public static string GetLastFolder(string FullPath)
        {
            return GetLastFolder(FullPath, Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// Path1의 마지막 PathSeparator, Path2의 처음 PathSeparator는 무조건 제거하고 붙임.
        /// </summary>
        /// <example>
        /// "Path1: a, Path2: b, CPath.CombineSafe: a/b, CUrl.Combine: http://a/b, Path.Combine: a\\b"
        /// "Path1: a/, Path2: b, CPath.CombineSafe: a/b, CUrl.Combine: http://a/b, Path.Combine: a\\b"
        /// "Path1: a, Path2: /b, CPath.CombineSafe: a/b, CUrl.Combine: http://a/b, Path.Combine: \\b"
        /// "Path1: a/, Path2: /b, CPath.CombineSafe: a/b, CUrl.Combine: http://a/b, Path.Combine: \\b"
        /// "Path1: a//, Path2: b, CPath.CombineSafe: a/b, CUrl.Combine: http://a//b, Path.Combine: a\\\\b"
        /// "Path1: a, Path2: //b, CPath.CombineSafe: a/b, CUrl.Combine: http://b/, Path.Combine: \\\\b"
        /// "Path1: a//, Path2: b, CPath.CombineSafe: a/b, CUrl.Combine: http://a//b, Path.Combine: a\\\\b"
        /// "Path1: a//, Path2: //b, CPath.CombineSafe: a/b, CUrl.Combine: http://b/, Path.Combine: \\\\b"
        /// </example>
        public static string CombineSafe(string Path1, string Path2, char DirectorySeparator)
        {
            if (!string.IsNullOrEmpty(Path1) && Path1.EndsWith(DirectorySeparator.ToString()))
            {
                Path1 = Path1.TrimEnd(DirectorySeparator);
            }
            if (!string.IsNullOrEmpty(Path2) && Path2.StartsWith(DirectorySeparator.ToString()))
            {
                Path2 = Path2.TrimStart(DirectorySeparator);
            }

            return Path1 + DirectorySeparator + Path2;
        }
        /// <summary>
        /// Path1의 마지막 PathSeparator, Path2의 처음 PathSeparator, Path3의 처음 PathSeparator는 무조건 제거하고 붙임.
        /// </summary>
        public static string CombineSafe(string Path1, string Path2, string Path3, char DirectorySeparator)
        {
            return CombineSafe(CombineSafe(Path1, Path2, DirectorySeparator), Path3, DirectorySeparator);
        }

        /// <summary>
        /// Path.Combine은 /로 구분되는 URL 주소를 지원하지 않으므로 /로 구분되는 버전을 만듦.
        /// </summary>
        /// <param name="Paths"></param>
        /// <returns></returns>
        public static string CombineUrl(params string[] Paths)
        {
            char Sep = '/';
            List<string> aPath = new List<string>();
            for (int i = 0; i < Paths.Length - 1; i++)
            {
                if (!string.IsNullOrEmpty(Paths[i]))
                {
                    //// http://www.testinter.net/update/wptester_gilbut_20111, /css가 있는 경우
                    //// http://www.testinter.net/update/wptester_gilbut_20111//css와 같이 /가 2개가 되므로 2번째부터는 앞의 /도 자름.
                    // 그러나 "/css"인 경우 http://www.testinter.net 부터 시작함을 뜻하므로 앞의 /를 자르면 안되므로 주석.
                    //if (i == 0)
                    //    aPath.Add(Paths[i].TrimEnd(Sep));
                    //else
                    //    aPath.Add(Paths[i].Trim(Sep));

                    aPath.Add(Paths[i].TrimEnd(Sep));
                }
            }
            //마지막의 Separator는 더 이상 뒤의 폴더와 충돌하지 않으므로 보존함.
            if (!string.IsNullOrEmpty(Paths[Paths.Length - 1]))
                aPath.Add(Paths[Paths.Length - 1]);

            return string.Join(Sep.ToString(), aPath.ToArray());
        }

        public static string TrimServerUrl(string Url)
        {
            string ServerUrl = CPath.GetServerUrl(Url);
            if (!string.IsNullOrEmpty(ServerUrl))
            {
                Url = Url.Substring(ServerUrl.Length);
            }

            return Url;
        }

        /// <summary>
        /// UrlParent가 '/good/manager/'이고 UrlChild가 '../../images/star.gif'인 경우
        /// '/images/star.gif'를 리턴함.
        /// </summary>
        /// <param name="UrlParent"></param>
        /// <param name="UrlChild"></param>
        /// <returns></returns>
        public static string ConvertRelativeToAbsolute(string UrlParent, string UrlChild)
        {
            if (UrlChild.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase)
                || UrlChild.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase)
                || UrlChild.StartsWith("ftp://", StringComparison.CurrentCultureIgnoreCase))
            {
                return UrlChild;
            }

            // http://www.site.com은 제거
            string AbsolutePathIs, ParamIs;
            string UrlServer = CPath.GetServerUrl(UrlParent, out AbsolutePathIs, out ParamIs);
            if (!string.IsNullOrEmpty(UrlServer))
                UrlParent = AbsolutePathIs;

            List<string> aUrlParent = new List<string>(CPath.GetAllPathInArray(UrlParent, '/', StringSplitOptions.RemoveEmptyEntries));
            List<string> aUrlChild = new List<string>(CPath.GetAllPathInArray(UrlChild, '/', StringSplitOptions.RemoveEmptyEntries));

            //파일은 경로에서 삭제
            if (aUrlParent[aUrlParent.Count - 1].IndexOf('.') != -1)
            {
                aUrlParent.RemoveAt(aUrlParent.Count - 1);
            }

            if (UrlChild.StartsWith("/"))
            {
                aUrlParent.Clear();
            }
            else
            {
                for (int i = 0; i < aUrlChild.Count; i++)
                {
                    if (aUrlChild[i] == "..")
                    {
                        aUrlParent.RemoveAt(aUrlParent.Count - 1);
                        aUrlChild.RemoveAt(0);
                        i--;
                    }
                }
            }

            foreach (string UrlChildCur in aUrlChild)
            {
                aUrlParent.Add(UrlChildCur);
            }

            string UrlNew = string.Join("/", aUrlParent.ToArray());
            if (!string.IsNullOrEmpty(UrlServer))
                UrlNew = CPath.CombineUrl(UrlServer, UrlNew);

            return UrlNew;
        }

        public static string GetFrameworkFolder(FrameworkVersion Ver)
        {
            string sVer = "";
            switch (Ver)
            {
                case FrameworkVersion.Fx10:
                    break;
                case FrameworkVersion.Fx11:
                    sVer = "v1.1.4322";
                    break;
                case FrameworkVersion.Fx20:
                    sVer = "v2.0.50727";
                    break;
                case FrameworkVersion.Fx30:
                    sVer = "v3.0";
                    break;
                case FrameworkVersion.Fx35:
                    sVer = "v3.5";
                    break;
                case FrameworkVersion.Fx40Client:
                case FrameworkVersion.Fx40Full:
                    sVer = "v4.0.30319";
                    break;
                default:
                    break;
            }

#if DotNet35
			string Folder = Path.Combine(Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.System)).FullName, @"Microsoft.NET\Framework\" + sVer);
#else
            string Folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"Microsoft.NET\Framework\" + sVer);
#endif

            return Folder;
        }

        public static string PostfixToFileName(string FullPath, string Postfix)
        {
            string FolderName = Path.GetDirectoryName(FullPath);
            string FileNoExt = Path.GetFileNameWithoutExtension(FullPath);
            string Ext = Path.GetExtension(FullPath);

            return Path.Combine(FolderName, FileNoExt + Postfix + Ext);
        }

        /// <summary>
        /// 해당 파일이 있다면 파일명 뒤에 (1), (2)와 같은 문자열을 붙여 리턴함.
        /// </summary>
        /// <param name="FullPath">기준이 되는 파일의 전체 경로</param>
        /// <returns><paramref name="FullPath"/> 파일명, 또는 (1), (2) 등이 붙은 파일명</returns>
        /// <example>
        /// <code>
        /// Console.WriteLine(CPath.GetNumberedFullPath(@"C:\Temp\Setup.ini")); // "C:\Temp\Setup(1).ini"
        /// </code>
        /// </example>
        public static string GetNumberedFullPath(string FullPath)
        {
            bool IsDirectoryExists = Directory.Exists(FullPath);
            bool IsFileExists = File.Exists(FullPath);

            if (!IsDirectoryExists && !IsFileExists)
                return FullPath;


            if (IsFileExists)
            {
                string FolderName = Path.GetDirectoryName(FullPath);
                string FileName = Path.GetFileName(FullPath);
                string FileNoExt = Path.GetFileNameWithoutExtension(FileName);
                string Ext = Path.GetExtension(FileName);

                string FileNoExtNew = FileNoExt;
                int Index = 0;
                while (File.Exists(Path.Combine(FolderName, FileNoExtNew + Ext)))
                {
                    FileNoExtNew = string.Format("{0}({1})", FileNoExt, ++Index);
                }

                return Path.Combine(FolderName, FileNoExtNew + Ext);
            }
            else
            {
                string FolderParent = CPath.GetParentPath(FullPath);
                string FolderCurrent = CPath.GetLastFolder(FullPath);

                string FolderNew = FolderCurrent;
                int Index = 0;
                while (Directory.Exists(Path.Combine(FolderParent, FolderNew)))
                {
                    FolderNew = string.Format("{0}({1})", FolderCurrent, ++Index);
                }

                return Path.Combine(FolderParent, FolderNew);
            }
        }
        /// <summary>
        /// 파일 크기가 MaxSize보다 크면 파일명에 번호를 붙임.
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// int IndexFirst = _dicLogFullPathAndIndex.ContainsKey(LogFullPath) ? _dicLogFullPathAndIndex[LogFullPath] : 1;
        /// int IndexNextIs;
        /// string FullPath = CPath.GetNumberedFullPathIfSizeOver(LogFullPath, _FileSizeForOneFile, IndexFirst, out IndexNextIs);
        /// _dicLogFullPathAndIndex.AddOrUpdate(LogFullPath, IndexNextIs, (s, n) => IndexNextIs);
        /// ]]>
        /// </example>
        public static string GetNumberedFullPathIfSizeOver(string FullPath, long MaxSize, int IndexFirst, out int IndexNextIs)
        {
            //무조건 1을 증가시키므로 1을 먼저 뺌.
            IndexNextIs = Math.Max(IndexFirst - 1, 0);


            bool IsFileExists = File.Exists(FullPath);
            if (!IsFileExists)
                return FullPath;

            string FolderName = Path.GetDirectoryName(FullPath);
            string FileName = Path.GetFileName(FullPath);
            string FileNoExt = Path.GetFileNameWithoutExtension(FileName);
            string Ext = Path.GetExtension(FileName);

            string FileNoExtNew = FileNoExt;
            while (CFile.GetFileSize(Path.Combine(FolderName, FileNoExtNew + Ext)) > MaxSize)
            {
                FileNoExtNew = string.Format("{0}({1})", FileNoExt, ++IndexNextIs);
            }

            return Path.Combine(FolderName, FileNoExtNew + Ext);
        }
        public static string GetNumberedFullPathIfSizeOver(string FullPath, long MaxSize)
        {
            int IndexFirst = 1;
            int IndexNextIs;
            return GetNumberedFullPathIfSizeOver(FullPath, MaxSize, IndexFirst, out IndexNextIs);
        }

        public static string GetMappedDrivesRealLocationOrDefault(string Path)
        {
            const int ERROR_MORE_DATA = 234; // (0xEA)
            const int ERROR_SUCCESS = 0; // (0x0)

            var nfo = new UNIVERSAL_NAME_INFO();
            var size = Marshal.SizeOf(nfo);

            if (ERROR_MORE_DATA != WNetGetUniversalName(Path, InfoLevel.UniversalName, ref nfo, ref size))
                return Path;

            var buffer = Marshal.AllocHGlobal(size);
            if (ERROR_SUCCESS != WNetGetUniversalName(Path, InfoLevel.UniversalName, buffer, ref size))
                return Path;

            nfo = (UNIVERSAL_NAME_INFO)Marshal.PtrToStructure(buffer, typeof(UNIVERSAL_NAME_INFO));
            return nfo.lpUniversalName;
        }

        /// <summary>
        /// Url1과 Url2가 같은 주소인 지 확인.
        /// http://와 마지막의 /를 빼고 비교함.
        /// </summary>
        public static bool CompareUrl(string Url1, string Url2)
        {
            string Url1New = Url1.ToLower();
            string Url2New = Url2.ToLower();

            if (Url1New.StartsWith("http://"))
                Url1New = Url1New.Substring("http://".Length);
            else if (Url1New.StartsWith("https://"))
                Url1New = Url1New.Substring("https://".Length);

            if (Url2New.StartsWith("http://"))
                Url2New = Url2New.Substring("http://".Length);
            else if (Url2New.StartsWith("https://"))
                Url2New = Url2New.Substring("https://".Length);

            if (Url1New.EndsWith("/"))
                Url1New = Url1New.Substring(0, Url1New.Length - "/".Length);
            if (Url2New.EndsWith("/"))
                Url2New = Url2New.Substring(0, Url2New.Length - "/".Length);

            return (Url1New == Url2New);
        }

        public static string GetFullPathDest(string FolderSrc, string FolderDest, string FullPathSrc)
        {
            if (!FullPathSrc.StartsWith(FolderSrc))
                return "";

            string FullPathRest = FullPathSrc.Substring(FolderSrc.Length);
            if (FullPathRest.StartsWith(@"\"))
                FullPathRest = FullPathRest.Substring(1);

            string FullPathDest = Path.Combine(FolderDest, FullPathRest);
            return FullPathDest;
        }

        /// <example>
        /// Debug.WriteLine(GetDateTimeVariableReplaced("%yyyy%-%MM%-%dd% %HH%:%mm%:%ss%.%fff%"));
        /// </example>
        public static string GetDateTimeVariableReplaced(string Path, DateTime dtStandard)
        {
            string Pattern = @"%\w+%";

            Regex r = new Regex(Pattern, RegexOptions.Compiled);
            string Ret = r.Replace(Path, m =>
            {
                // Replace "%yyyy%" with "2018" by using DateTime.ToString
                return dtStandard.ToString(m.Value.Trim('%'));
            });
            return Ret;
        }
        public static string GetDateTimeVariableReplaced(string Path)
        {
            return GetDateTimeVariableReplaced(Path, DateTime.Now);
        }

        /// <summary>
        /// Get <paramref name="localRootDir"/> removed from <paramref name="localFullPath"/> and append it to <paramref name="remoteDir"/>
        /// </summary>
        /// <returns></returns>
        /// <example>
        /// CPath.GetRemotePath("/root/sub1", "C:\\Temp", "C:\\Temp\\Test.txt"); // /root/sub1/Test.txt
        /// </example>
        public static string GetRemotePath(string remoteDir, string localRootDir, string localFullPath)
        {
            string parentDir = remoteDir.TrimEnd('/');
            string subDir = localFullPath.Substring(localRootDir.Length)
                                        .Replace(Path.DirectorySeparatorChar, '/')
                                        .TrimStart('/');
            return $"{parentDir}/{subDir}";
        }
    }
}

