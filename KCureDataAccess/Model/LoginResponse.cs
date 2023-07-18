using DoctorGu;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KCureVDIDataBox.Model
{
    public enum UrlDirectoryTypes
    {
        /// <summary>경로의 원본 사용</summary>
        Raw,
        /// <summary>경로의 앞에서 nasBaseFolder를 제거</summary>
        TrimBaseFolder,
        /// <summary>경로의 앞에 nasBaseFolder를 붙임</summary>
        PrefixBaseFolder,
    }

    public class LoginResponse : BaseResponse
    {
        public LoginResult result { get; set; } = new LoginResult();
    }

    /// <summary>
    /// 로그인 결과 데이터
    /// </summary>
    public class LoginResult
    {
        public string isLogin { get; set; } = "false";
        public string loginFailMsg { get; set; } = "";
        public string nasUrl { get; set; } = "";
        public string nasId { get; set; } = "";
        public string nasPw { get; set; } = "";
        public string nasPort { get; set; } = "";
        public string nasBaseFolder { get; set; } = "";
        public string nasEncoding { get; set; } = "";
        public string today { get; set; } = "";

        public ApplyResult? applyResult { get; set; } = new ApplyResult();

        /// <summary>
        /// <paramref name="AuthCode"/>가 <see cref="applyResult"/>에 있는지 여부를 반환
        /// </summary>
        /// <param name="AuthCode"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool HasAuthCode(string AuthCode)
        {
            if (applyResult == null)
            {
                throw new Exception($"{typeof(ApplyResult).Name} is null.");
            }

            var dic = applyResult.authCode[0];
            return dic.ContainsKey(AuthCode);
        }

        /// <summary>
        /// <paramref name="type"/>에 의해 <paramref name="url"/>을 가공하여 반환
        /// </summary>
        /// <param name="type"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetUrlDirectory(UrlDirectoryTypes type, string url)
        {
            string urlNew = url.TrimEnd('/');
            if (type == UrlDirectoryTypes.Raw)
            {
                // Do nothing
            }
            else if (type == UrlDirectoryTypes.TrimBaseFolder)
            {
                urlNew = CFindRep.TrimStart(urlNew, nasBaseFolder, StringComparison.CurrentCultureIgnoreCase);
                urlNew = $"/{urlNew.TrimStart('/')}";
            }
            else if (type == UrlDirectoryTypes.PrefixBaseFolder)
            {
                if (!urlNew.StartsWith(nasBaseFolder, StringComparison.CurrentCultureIgnoreCase))
                {
                    urlNew = nasBaseFolder.TrimEnd('/') + '/' + urlNew.TrimStart('/');
                }
            }
            else
            {
                throw new Exception($"Wrong type:{type}");
            }

            return urlNew;
        }

        ///// <summary>
        ///// 참여기관전송, 반입 여부에 따라 <see cref="applyResult"/>에서 <see cref="Data"/>를 반환
        ///// </summary>
        ///// <param name="forCarry"></param>
        ///// <returns></returns>
        ///// <exception cref="Exception"></exception>
        //public List<Data> GetUserValidDatas(bool forCarry)
        //{
        //    DateTime now = CDateTime.GetDateFromYYYYMMDD(today);
        //    if (applyResult == null)
        //    {
        //        throw new Exception("applyResult is null");
        //    }

        //    var list = new List<Data>();
        //    var authCodes = applyResult.authCode[0];
        //    foreach (var item in authCodes)
        //    {
        //        string authCode = item.Key;
        //        if (authCode != CCommon.Const.USER_AU006)
        //            continue;

        //        var datas = item.Value;
        //        foreach (var data in datas)
        //        {
        //            if (string.IsNullOrEmpty(data.dtuSdt))
        //                continue;

        //            if (string.IsNullOrEmpty(data.dtuEdt))
        //                continue;

        //            if (!forCarry && string.IsNullOrEmpty(data.prtiFile))
        //                continue;

        //            if (forCarry && string.IsNullOrEmpty(data.carryFile))
        //                continue;

        //            DateTime start = CDateTime.GetDateFromYYYYMMDD(data.dtuSdt);
        //            DateTime end = CDateTime.GetDateFromYYYYMMDD(data.dtuEdt);
        //            if (now < start || now > end)
        //                continue;

        //            Log("data", data.getString());
        //            list.Add(data);
        //        }
        //    }

        //    return list;
        //}

        /// <summary>
        /// 참여기관전송, 반입 여부에 따라 <see cref="applyResult"/>에서 <see cref="Data"/>를 반환
        /// </summary>
        /// <param name="forCarry"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Data> GetUserValidDatas()
        {
            DateTime now = CDateTime.GetDateFromYYYYMMDD(today);
            if (applyResult == null)
            {
                throw new Exception("applyResult is null");
            }

            var list = new List<Data>();
            var authCodes = applyResult.authCode[0];
            foreach (var item in authCodes)
            {
                string authCode = item.Key;
                if (authCode != CCommon.Const.USER_AU006)
                    continue;

                var datas = item.Value;
                foreach (var data in datas)
                {
                    if (string.IsNullOrEmpty(data.DtuSdt))
                        continue;

                    if (string.IsNullOrEmpty(data.DtuEdt))
                        continue;

                    //if (!forCarry && string.IsNullOrEmpty(data.prtiFile))
                    //    continue;

                    //if (forCarry && string.IsNullOrEmpty(data.carryFile))
                    //    continue;

                    DateTime start = CDateTime.GetDateFromYYYYMMDD(data.DtuSdt);
                    DateTime end = CDateTime.GetDateFromYYYYMMDD(data.DtuEdt);
                    if (now < start || now > end)
                        continue;

                    Log("data", data.getString());
                    list.Add(data);
                }
            }

            return list;
        }


        ///// <summary>
        ///// 참여기관전송, 반입 여부에 따라 FTP의 모든 루트 경로를 반환
        ///// </summary>
        ///// <param name="forCarry"></param>
        ///// <returns></returns>
        //public List<string> GetUserRootDirs(bool forCarry)
        //{
        //    var dirs = new List<string>();
        //    var datas = GetUserValidDatas(forCarry);
        //    foreach (var data in datas) {
        //        string dir = "";
        //        if (!forCarry && !string.IsNullOrEmpty(data.prtiFile))
        //        {
        //            dir = data.prtiFile;
        //        }
        //        else if (forCarry && !string.IsNullOrEmpty(data.carryFile))
        //        {
        //            dir = data.carryFile;
        //        }
        //        if (dir == "")
        //            continue;

        //        Log("dir", dir);
        //        dirs.Add(CCommon.LoginResult.GetUrlDirectory(UrlDirectoryTypes.PrefixBaseFolder, dir));
        //    }

        //    return dirs;
        //}

        /// <summary>
        /// 참여기관전송, 반입 여부에 따라 FTP의 모든 루트 경로를 반환
        /// </summary>
        /// <param name="forCarry"></param>
        /// <returns></returns>
        public List<string> GetUserRootDirs(string fileCode)
        {
            var datas = GetUserValidDatas();
            var dirs = new List<string>();
            foreach (var data in datas)
            {
                string dir = CCommon.LoginResult.GetUrlDirectory(UrlDirectoryTypes.PrefixBaseFolder, data.AttfPthNm);
                if (!dirs.Contains(dir) && fileCode == data.AttfSpcd)
                {
                    Log("dir", dir);
                    dirs.Add(dir);
                }
            }
            return dirs;
        }


        ///// <summary>
        ///// 참여기관전송, 반입 여부에 따라 데이터신청번호를 반환
        ///// </summary>
        ///// <param name="forCarry"></param>
        ///// <returns></returns>
        //public List<string> GetDataAplcNos(bool forCarry)
        //{
        //    var dataAplcNos = new List<string>();
        //    var datas = GetUserValidDatas(forCarry);
        //    foreach (var data in datas)
        //    {
        //        if (string.IsNullOrEmpty(data.dataAplcNo)) continue;

        //        dataAplcNos.Add(data.dataAplcNo);
        //    }

        //    return dataAplcNos;
        //}

        /// <summary>
        /// 참여기관전송, 반입 여부에 따라 데이터신청번호를 반환
        /// </summary>
        /// <param name="forCarry"></param>
        /// <returns></returns>
        public List<string> GetDataAplcNos(string fileCode)
        {
            var dataAplcNos = new List<string>();
            var datas = GetUserValidDatas();
            foreach (var data in datas)
            {
                if (string.IsNullOrEmpty(data.DataAplcNo)) continue;

                dataAplcNos.Add(data.DataAplcNo);
            }

            return dataAplcNos;
        }

        /// <summary>
        /// 반출 신청 시 파일이 업로드될 경로를 반환
        /// </summary>
        /// <param name="dataAplcNo"></param>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public string GetAplyUrl(string dataAplcNo, string loginId)
        {
            string today = CDateTime.GetDateFromYYYYMMDD(this.today).ToString("yyyyMMdd");
            // K-CURE/cro/신청번호/연구자및신청자ID/YYYYMMDD
            string url = $"K-CURE/cro/{dataAplcNo}/{loginId}/{today}";
            string urlFull = GetUrlDirectory(UrlDirectoryTypes.PrefixBaseFolder, url);
            return urlFull;
        }

        public void Log(string category, string log)
        {
            Console.WriteLine($"\nDEBUG>>> ({category}) \n{log}");
        }
    }

    /// <summary>
    /// 로그인 성공 후 가져올 Model
    /// </summary>
    public class ApplyResult
    {
        public List<Dictionary<string, List<Data>>> authCode { get; set; } = new List<Dictionary<string, List<Data>>>();
    }

    /// <summary>
    /// 로그인 성공 후 가져올 하위 Model
    /// </summary>
    //public class Data
    //{
    //    public string dataAplcNo { get; set; } = "";
    //    public string dtuSdt { get; set; } = "";
    //    public string dtuEdt { get; set; } = "";
    //    public string cryOutApply { get; set; } = "";

    //    public string? prtiFile { get; set; } = "";
    //    public string? carryFile { get; set; } = "";

    //    public string? subFile { get; set; } = "";

    //    public string getString()
    //    {
    //        return $"\ndataAplcNo: {dataAplcNo} \ndtuSdt: {dtuSdt} \ndtuEdt: {dtuEdt} \nprtiFile: {prtiFile} \ncarryFile: {carryFile} \nsubFile: {subFile}";
    //    }
    //}

        public class Data
        {
            public string DataAplcNo { get; set; } = "";
            public string RsrSbjNm { get; set; } = "";
            public string DataAplpId { get; set; } = "";
            public string DtuSdt { get; set; } = "";
            public string? DtuEdt { get; set; } = "";
            public string? CryOutApply { get; set; } = "";
            public string? AttfPthNm { get; set; } = "";
            public string? AttfNm { get; set; } = "";
            public string? AttfStrNm { get; set; } = "";
            public string? AttfSpcd { get; set; } = "";
            public int? AttfSeq { get; set; } = -1;


        public string getString()
        {
                return $"\nDataAplcNo: {DataAplcNo} \nRsrSbjNm: {RsrSbjNm} \nDataAplpId: {DataAplpId} " +
                       $"\nDtuSdt: {DtuSdt} \nDtuEdt: {DtuEdt} \nCryOutApply: {CryOutApply}" +
                       $"\nAttfPthNm: {AttfPthNm} \nAttfNm: {AttfNm}  \nAttfStrNm: {AttfStrNm}  \nAttfSpcd: {AttfSpcd}  \nAttfSeq: {AttfSeq}";
        }
    }
}
