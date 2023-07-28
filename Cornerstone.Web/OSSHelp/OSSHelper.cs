
using Aliyun.OSS;
using Aliyun.OSS.Common;
using Microsoft.AspNetCore.Http;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UEditor.Core;

namespace Cornerstone.Web.OSSHelp
{
    public class OSSHelper
    {

        public OSSHelper()
        {

        }


        /// <summary>
        /// 获取OSS临时url
        /// </summary>
        /// <param name="keyUrl"></param>
        /// <returns></returns>
        public static string GetOSSUrl(string keyUrl)
        {
            if (keyUrl != null)
            {
                // 填写Bucket所在地域对应的Endpoint。以华东1（杭州）为例，Endpoint填写为https://oss-cn-hangzhou.aliyuncs.com。
                var endpoint = Consts.AliyunOssServer.AccessEndpoint;
                // 阿里云账号AccessKey拥有所有API的访问权限，风险很高。强烈建议您创建并使用RAM用户进行API访问或日常运维，请登录RAM控制台创建RAM用户。
                var accessKeyId = Consts.AliyunOssServer.AccessKeyId;
                var accessKeySecret = Consts.AliyunOssServer.AccessKeySecret;
                // 填写Bucket名称，例如examplebucket。
                var bucketName = Consts.AliyunOssServer.BucketName;
                // 填写Object完整路径，完整路径中不包含Bucket名称，例如exampledir/exampleobject.txt。
                var objectName = keyUrl.Replace(Consts.AliyunOssServer.OSSUrl, "");
                if (objectName.IndexOf("?") >= 0)
                {
                    objectName = objectName.Substring(0, objectName.IndexOf("?"));
                }
                // 创建OSSClient实例。
                var client = new OssClient(endpoint, accessKeyId, accessKeySecret);
                try
                {
                    var metadata = client.GetObjectMetadata(bucketName, objectName);
                    var etag = metadata.ETag;
                    // 生成签名URL。
                    var req = new GeneratePresignedUriRequest(bucketName, objectName, SignHttpMethod.Get)
                    {
                        // 设置签名URL过期时间，默认值为3600秒。
                        Expiration = DateTime.Now.AddHours(1),
                    };
                    var uri = client.GeneratePresignedUri(req);
                    return uri.ToString();
                }
                catch (OssException ex)
                {

                }
                catch (Exception ex)
                {

                }
            }
            return keyUrl;
        }

        public static string GetImg(string str = "")
        {
            if (str != null)
            {
                Regex rg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

                var m = rg.Match(str);
                ArrayList list = new ArrayList();
                while (m.Success)
                {
                    string src = m.Groups[1].Value; //这里就是图片路径  
                    list.Add(src);
                    m = m.NextMatch();
                }

                rg = new Regex(@"url\s*\(([^\)]+)\)", RegexOptions.IgnoreCase);

                m = rg.Match(str);

                while (m.Success)
                {
                    string src = m.Groups[1].Value.Replace("&quot;", ""); //这里就是图片路径  
                    list.Add(src);
                    m = m.NextMatch();
                }

                ArrayList backList = new ArrayList();
                for (int i = 0; i < list.Count; i++)
                {
                    str = str.Replace(list[i].ToString(), GetOSSUrl(list[i].ToString()));
                }
            }
            return str;
        }

    }
}
