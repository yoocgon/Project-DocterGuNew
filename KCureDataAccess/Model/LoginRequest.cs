using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCureVDIDataBox.Model
{
    /// <summary>
    /// 로그인 요청 파라미터
    /// </summary>
    internal class LoginRequest
    {
        public string userId { get; set; } = "";
        public string loginPswd { get; set; } = "";

        public LoginRequest(string userId, string loginPswd)
        {
            this.userId = userId;
            this.loginPswd = loginPswd;
        }
    }
}
