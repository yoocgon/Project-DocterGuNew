using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KCureVDIDataBox
{
    /// <summary>
    /// 현재 프로그램의 경로 정보를 표시
    /// </summary>
    partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
            //this.Text = String.Format("About {0}", AssemblyTitle);
            this.Text = String.Format("About {0}", Application.ProductName);
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text = AssemblyCompany;
            this.textBoxDescription.Text = GetLocation();
        }
        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 현재 프로그램의 경로(실행 파일, 설정 파일, 로그 파일) 정보를 반환
        /// </summary>
        /// <returns></returns>
        private string GetLocation()
        {
            string fullPathApp = Application.ExecutablePath;
            string dirApp = Path.GetDirectoryName(fullPathApp) ?? "";

            string fileName = Path.GetFileName(fullPathApp);
            string fileNameNoExt = Path.GetFileNameWithoutExtension(fileName);
            string configName = $"{fileNameNoExt}.dll.config";
            string logName = $"{DateTime.Now.ToString("yyyyMMdd")}.log";

            string fullPathConfig = Path.Combine(dirApp, configName);
            string fullPathLog = Path.Combine(dirApp, "Log", fileNameNoExt, logName);

            string info =
$"fullPathApp: {fullPathApp}\r\n\r\n" +
$"fullPathConfig: {fullPathConfig}\r\n\r\n" +
$"fullPathLog: {fullPathLog}\r\n\r\n" +
$"";
            return info;
        }


        #region Assembly Attribute Accessors
        /// <summary>
        /// 제목
        /// </summary>
        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
            }
        }

        /// <summary>
        /// 버전
        /// </summary>
        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        /// <summary>
        /// 설명
        /// </summary>
        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        /// <summary>
        /// 제품
        /// </summary>
        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        /// <summary>
        /// 저작권
        /// </summary>
        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        /// <summary>
        /// 회사
        /// </summary>
        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion
    }
}
