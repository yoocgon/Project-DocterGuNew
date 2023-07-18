using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCureVDIDataBox.Model
{
    /// <summary>
    /// 파일 반출 요청 파라미터
    /// </summary>
    internal class AplyRequest
    {
        public string dataAplyNo { get; set; }
        public string userId { get; set; }
        public List<string> cryOutFiles { get; set; }

        public AplyRequest(string dataAplyNo, string userId, List<string> cryOutFiles)
        {
            this.dataAplyNo = dataAplyNo;
            this.userId = userId;
            this.cryOutFiles = cryOutFiles;
        }
    }
}
