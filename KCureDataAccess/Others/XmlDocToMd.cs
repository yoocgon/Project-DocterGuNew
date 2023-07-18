using DoctorGu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KCureDataAccess.Others
{
    /// <summary>
    /// XML로 된 주석을 Markdown으로 변환
    /// </summary>
    /// <example>
    /// if (CInfo.IsDebugMode)
    /// {
    ///     XmlDocToMd dm = new XmlDocToMd("C:\\source\\repos\\KCureDataAccess\\KCureDataAccess");
    ///     string value = dm.Convert();
    /// }
    /// </example>
    public class XmlDocToMd
    {
        private string rootFolder;

        /// <summary>
        /// 객체를 생성할 때 .cs를 포함하는 폴더를 지정
        /// </summary>
        /// <param name="rootFolder"></param>
        public XmlDocToMd(String rootFolder) { 
            this.rootFolder = rootFolder;
        }

        /// <summary>
        /// .cs 파일의 주석과 1번째 줄의 코드를 읽어 Markdown을 변환한 문자열을 반환
        /// </summary>
        /// <returns></returns>
        public string Convert()
        {
            string pattern = $"(?<tabs> +)\\/\\/\\/ <summary>(?<summary>.+?)<\\/summary>.+?(?<line>(?!public|private|internal).+?)$";
            Regex r = new Regex(pattern, RegexOptions.Multiline | RegexOptions.Singleline);

            string patParamRef = $"<(paramref|typeparamref) name=\"(?<name>[^\"]+)\"/>";
            Regex rParamRef = new Regex(patParamRef);

            string patDel = $" +/// <(returns|example|param|typeparam|exception|remarks).+?</\\1>\\n";
            Regex rDel = new Regex(patDel, RegexOptions.Multiline | RegexOptions.Singleline);


            string patSummary = $"( +)/// ";
            Regex rPat = new Regex(patSummary);

            DirectoryInfo di = new DirectoryInfo(rootFolder);
            FileInfo[] fis = di.GetFiles("*.cs", SearchOption.AllDirectories)
                                .Where(fi => !fi.Name.EndsWith("Designer.cs") && fi.FullName.IndexOf("\\obj\\") == -1)
                                .ToArray();

            List<string> rets = new List<string>();
            foreach (FileInfo fi in fis)
            {
                rets.Add($"# {fi.Name}");

                string code = fi.OpenText().ReadToEnd().Replace("\r\n", "\n");
                code = rParamRef.Replace(code, "${name}");
                code = rDel.Replace(code, "");

                foreach (Match m in r.Matches(code))
                {
                    int len = m.Groups["tabs"].Value.Length / 4;
                    string sharp = CFindRep.Repeat('#', len + 2);

                    string summary = m.Groups["summary"].Value;
                    summary = CFindRep.TrimWhiteSpace(rPat.Replace(summary, ""));

                    string line = CFindRep.TrimWhiteSpace(m.Groups["line"].Value);
                    
                    rets.Add($"{sharp} {line}\n{summary}");
                }
            }

            string ret = string.Join("\n\n", rets);
            return ret;
        }
    }
}
