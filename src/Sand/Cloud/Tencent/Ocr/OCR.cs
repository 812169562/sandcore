using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestSharp;
using Sand.DI;
using Sand.Exceptions;
using Sand.Helpers;
using Sand.Maps;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Ocr.V20181119;
using TencentCloud.Ocr.V20181119.Models;

namespace Sand.Cloud.Tencent.Ocr
{
    /// <summary>
    /// 
    /// </summary>
    public class OCR : IOCR
    {
        /// <summary>
        /// 身份证识别
        /// </summary>
        /// <param name="inputParm">输入参数</param>
        /// <returns></returns>
        public async Task<IdCard> GetIdCard(InputParm inputParm)
        {
            var credential = new Credential();
            var config=Ioc.GetService<IConfiguration>();
            credential.SecretId = config.GetSection("Tencent:SecretId").Value??"AKIDResEl6RSeZEeqjrxYbNvDH7g8qJWMBfk";
            credential.SecretKey = config.GetSection("Tencent:SecretKey").Value ?? "7svkJbdSjom2D7kU1Wn1f3EAsNccN3b4";
            ClientProfile clientProfile = new ClientProfile();
            HttpProfile httpProfile = new HttpProfile();
            httpProfile.Endpoint = ("ocr.tencentcloudapi.com");
            clientProfile.HttpProfile = httpProfile;
            OcrClient client = new OcrClient(credential, "ap-guangzhou", clientProfile);
            IDCardOCRRequest req = new IDCardOCRRequest();
            req = IDCardOCRRequest.FromJsonString<IDCardOCRRequest>(Json.ToJson(inputParm));
            IDCardOCRResponse resp = await client.IDCardOCR(req);
            return resp.MapTo<IdCard>();
        }
    }
}