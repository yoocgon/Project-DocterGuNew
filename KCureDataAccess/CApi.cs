using DoctorGu;
using KCureVDIDataBox.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;


namespace KCureVDIDataBox
{
    /// <summary>
    /// HTTP 요청을 통해 원격 서버와 상호 작용하는 API 함수
    /// 로그인, 파일 반출 용 API에서 공통으로 사용하는 부분을 따로 떼어냄
    /// </summary>
    public class CApi
    {
        private string ApiKey;
        private string ApiValue;

        /// <summary>
        /// API Key, Value를 App.config에서 읽어와 설정
        /// </summary>
        public CApi()
        {
            this.ApiKey = CCommon.GetAppSetting<string>(ConfigAppKeys.ApiKey, "");
            this.ApiValue = CCommon.GetAppSetting<string>(ConfigAppKeys.ApiValue, "");
        }

        /// <summary>
        /// API 호출에 공통으로 사용되는 함수
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="jsonContent"></param>
        /// <returns></returns>
        internal async Task<(bool, string, T)> Send<T>(string url, string jsonContent) where T : new()
        {
            try
            {
                string ApiDomain = CCommon.GetAppSetting<string>(ConfigAppKeys.ApiDomain, "");
                if (CCommon.useLocalhost)
                {
                    ApiDomain = "http://localhost:8080";
                }
                string requestUrl = $"{ApiDomain}{url}";
                Log("requestUrl", requestUrl);

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
                request.Headers.Add(ApiKey, ApiValue);

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();

                string jsonResponseTest = @"
                {
                  ""httpStatus"": 200,
                  ""successYn"": ""Y"",
                  ""errorMsg"": """",
                  ""result"": {
                    ""isLogin"": ""true"",
                    ""loginFailMsg"": null,
                    ""nasUrl"": ""198.18.229.52"",
                    ""nasId"": ""centos"",
                    ""nasPw"": """",
                    ""nasPort"": ""22"",
                    ""nasBaseFolder"": ""/nasdata/svcAplc/"",
                    ""nasEncoding"": ""euc-kr"",
                    ""today"": ""20230710"",
                    ""applyResult"": {
                        ""authCode"": [
                              {
                                 ""AU006"": [
                                       {
                                           ""DataAplcNo"": ""KC20230714003"",
                                           ""RsrSbjNm"": ""KUGH11000-001 위암 임상연구"",
                                           ""DataAplpId"": ""research02@test.com"",
                                           ""DtuSdt"": ""20230101"",
                                           ""DtuEdt"": ""20230930"",
                                           ""CryOutApply"": ""Y"",
                                           ""AttfPthNm"": ""/K-CURE/apv/KC20230714003/00090"",
                                           ""AttfNm"": ""KC20230714003_KUGH11000-001_서울대_데이터파일.xlsx"",
                                           ""AttfStrNm"": ""00090_KC20230714003_CNCR_001.xlsx"",
                                           ""AttfSpcd"": ""03"",
                                           ""AttfSeq"": 11
                                       },
                                       {
                                           ""DataAplcNo"": ""KC20230714003"",
                                           ""RsrSbjNm"": ""KUGH11000-001 위암 임상연구"",
                                           ""DataAplpId"": ""research02@test.com"",
                                           ""DtuSdt"": ""20230101"",
                                           ""DtuEdt"": ""20230930"",
                                           ""CryOutApply"": ""Y"",
                                           ""AttfPthNm"": ""/K-CURE/apv/KC20230714003/00060"",
                                           ""AttfNm"": ""KC20230714003_KUGH11000-001_부산대_데이터파일.xlsx"",
                                           ""AttfStrNm"": ""00060_KC20230714003_CNCR_001.xlsx"",
                                           ""AttfSpcd"": ""03"",
                                           ""AttfSeq"": 12
                                       },
                                       {
                                           ""DataAplcNo"": ""KC20230714003"",
                                           ""RsrSbjNm"": ""KUGH11000-001 위암 임상연구"",
                                           ""DataAplpId"": ""research02@test.com"",
                                           ""DtuSdt"": ""20230101"",
                                           ""DtuEdt"": ""20230930"",
                                           ""CryOutApply"": ""Y"",
                                           ""AttfPthNm"": ""/K-CURE/brn/KC20230714003"",
                                           ""AttfNm"": ""전송현황+(1).xlsx"",
                                           ""AttfStrNm"": ""KC20230714003_04_91_20230715113927446.xlsx"",
                                           ""AttfSpcd"": ""04"",
                                           ""AttfSeq"": 8
                                       }
                                    ],
                               ""AU005"": [
                                        {
                                          ""DataAplcNo"": ""KC20230713003"",
                                          ""RsrSbjNm"": ""KUGH11000-008 공공 0713"",
                                          ""DtuSdt"": ""20230701"",
                                          ""DtuEdt"": ""20241231"",
                                          ""CryOutApply"": ""N"",
                                          ""AttfPthNm"": ""/K-CURE/apc/dsb/KC20230713003"",
                                          ""AttfNm"": ""8. 심의결과 통지서.hwp"",
                                          ""AttfStrNm"": ""KC20230713003_01_12_20230713010615589.hwp"",
                                          ""AttfSpcd"": ""01"",
                                          ""AttfSeq"": 2
                                        },
                                        {
                                          ""DataAplcNo"": ""KC20230713003"",
                                          ""RsrSbjNm"": ""KUGH11000-008 공공 0713"",
                                          ""DtuSdt"": ""20230701"",
                                          ""DtuEdt"": ""20241231"",
                                          ""CryOutApply"": ""N"",
                                          ""AttfPthNm"": ""/K-CURE/apc/dsb/KC20230713003"",
                                          ""AttfNm"": ""9. 감면율 적용근거.hwp"",
                                          ""AttfStrNm"": ""KC20230713003_01_13_20230713010620630.hwp"",
                                          ""AttfSpcd"": ""01"",
                                          ""AttfSeq"": 3
                                        },
                                        {
                                          ""DataAplcNo"": ""KC20230713003"",
                                          ""RsrSbjNm"": ""KUGH11000-008 공공 0713"",
                                          ""DtuSdt"": ""20230701"",
                                          ""DtuEdt"": ""20241231"",
                                          ""CryOutApply"": ""N"",
                                          ""AttfPthNm"": ""/K-CURE/apc/dsb/KC20230713003"",
                                          ""AttfNm"": ""5. 개인정보 수집·이용 및 제3자 제공 동의서.hwp"",
                                          ""AttfStrNm"": ""KC20230713003_01_07_20230713010827349.hwp"",
                                          ""AttfSpcd"": ""01"",
                                          ""AttfSeq"": 4
                                        }
                                    ]
                                }
                            ]
                        }
                    }
                }";

                //
                // jsonResponse = jsonResponseTest;
                Log("jsonResponse", PrettifyJSON(jsonResponse));

                //Dictionary<string, object> dicJson = DeserializeJson(jsonResponse);
                //object objResult = null;
                //dicJson.TryGetValue("result", out objResult);
                //Dictionary<string, object> dicResult = (Dictionary<string, object>)objResult;

                //object? objData = null;
                //dicResult?.TryGetValue("data", out objData);
                //List<object>? listObj = (List<object>) objData;

                //List<Dictionary<string, object>> listDic = new List<Dictionary<string, object>>();
                //foreach(object item in listObj) {
                //    Dictionary<string, object> dicRow = (Dictionary<string, object>)item;
                //    listDic.Add(dicRow);
                //}

                //Log("size", listDic.Count.ToString());

                //
                JsonDocument doc = JsonDocument.Parse(jsonResponse);

                var baseReponse = BaseResponse.ToObject<BaseResponse>(doc.RootElement);
                if (baseReponse == null)
                {
                    string msg = "Cannot convert to BaseResponse";
                    return (false, msg, new T());
                }

                var success = baseReponse.successYn == "Y";
                if (!success)
                {
                    string msg = baseReponse.errorMsg;
                    return (false, msg, new T());
                }

                var typeResponse = BaseResponse.ToObject<T>(doc.RootElement);
                if (response == null)
                {
                    string msg2 = $"Cannot convert to {typeof(T).Name}";
                    return (false, msg2, new T());
                }

                return (true, "", typeResponse ?? new T());
            }
            catch (Exception ex)
            {
                CCommon.Log.WriteLog(ex);
                return (false, ex.Message, new T());
            }
        }

        /// <summary>
        /// 로그인
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        internal async Task<(bool, string, LoginResult)> Login(string id, string password)
        {
            string url = CCommon.GetAppSetting<string>(ConfigAppKeys.LoginUrl, "");
            Log("url", url);

            var request = new LoginRequest(id, password);
            string jsonContent = JsonSerializer.Serialize(request);
            Log("jsonContent", PrettifyJSON(jsonContent));

            (bool success, string msg, LoginResponse response) = await Send<LoginResponse>(url, jsonContent);
            if (!success)
                return (false, msg, new LoginResult());

            if (response.result.isLogin != "true")
            {
                string msg2 = response.result.loginFailMsg;
                return (false, msg2, new LoginResult());
            }

            return (true, "", response.result);
        }

        /// <summary>
        /// 파일 반출
        /// </summary>
        /// <param name="dataAplyNo"></param>
        /// <param name="userId"></param>
        /// <param name="cryOutFiles"></param>
        /// <returns></returns>
        internal async Task<(bool, string, AplyResponse)> Aply(string dataAplyNo, string userId, List<string> cryOutFiles)
        {
            string url = CCommon.GetAppSetting<string>(ConfigAppKeys.AplyUrl, "");
            Log("url", url);

            var request = new AplyRequest(dataAplyNo, userId, cryOutFiles);
            string jsonContent = JsonSerializer.Serialize(request);
            Log("jsonContent", PrettifyJSON(jsonContent));

            (bool success, string msg, AplyResponse response) = await Send<AplyResponse>(url, jsonContent);
            if (!success)
                return (false, msg, new AplyResponse());

            //if (!response.result.isApply)
            //    return (false, response.result.applyFailMsg, new AplyResponse());

            return (true, "", response);
        }

        Dictionary<string, object> DeserializeJson(string jsonData)
        {
            using (JsonDocument document = JsonDocument.Parse(jsonData))
            {
                Dictionary<string, object> dataDictionary = new Dictionary<string, object>();
                foreach (JsonProperty property in document.RootElement.EnumerateObject())
                {
                    object value = GetJsonValue(property.Value);
                    dataDictionary.Add(property.Name, value);
                }

                return dataDictionary;
            }
        }


        object GetJsonValue(JsonElement element)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    return DeserializeJson(element.GetRawText());
                case JsonValueKind.Array:
                    List<object> arrayValues = new List<object>();
                    foreach (JsonElement arrayElement in element.EnumerateArray())
                    {
                        arrayValues.Add(GetJsonValue(arrayElement));
                    }
                    return arrayValues;
                case JsonValueKind.String:
                    return element.GetString();
                case JsonValueKind.Number:
                    return element.GetDecimal();
                case JsonValueKind.True:
                    return true;
                case JsonValueKind.False:
                    return false;
                case JsonValueKind.Null:
                default:
                    return null;
            }
        }

        public void Log(string category, string log)
        {
            Console.WriteLine($"\nDEBUG>>> ({category}) \n{log}");
        }

        public string PrettifyJSON(string json)
        {
            string beautifiedJson = Newtonsoft.Json.JsonConvert.SerializeObject(
            Newtonsoft.Json.JsonConvert.DeserializeObject(json),
            Newtonsoft.Json.Formatting.Indented);

            return beautifiedJson;
        }
    }
}
