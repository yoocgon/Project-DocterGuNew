using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KCureVDIDataBox.Model
{
    /// <summary>
    /// 파일 반출 결과 데이터
    /// </summary>
    internal class AplyResponse: BaseResponse
    {
        public AplyResult result { get; set; } = new AplyResult();
    }

    internal class AplyResult
    {
        public string dataAplyNo { get; set; } = "";
        public bool isApply { get; set; } = false;
        public string applyFailMsg { get; set; } = "";

        public string getString()
        {
            return $"\ndataAplyNo: {dataAplyNo} \nisApply: {isApply} \napplyFailMsg: {applyFailMsg}";
        }
    }
}
