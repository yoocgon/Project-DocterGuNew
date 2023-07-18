using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KCureVDIDataBox.Model
{
    /// <summary>
    /// 로그인, 파일 반출에 공통으로 사용되는 결과 데이터
    /// </summary>
    public class BaseResponse
    {
        public int httpStatus { get; set; }
        public string successYn { get; set; } = "N";
        public string errorMsg { get; set; } = "";

        public static T? ToObject<T>(JsonElement element)
        {
            var json = element.GetRawText();
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}