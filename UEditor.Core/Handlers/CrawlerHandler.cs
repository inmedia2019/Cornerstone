using Aliyun.OSS;
using Aliyun.OSS.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Net;
using static UEditor.Core.Consts;

namespace UEditor.Core.Handlers
{
    public class CrawlerHandler : Handler
    {
        private string[] Sources;
        private Crawler[] Crawlers;
        public CrawlerHandler(HttpContext context) : base(context) { }

        public override void Process()
        {
            Sources = Request.Form["source[]"];
            if (Sources == null || Sources.Length == 0)
            {
                WriteJson(new
                {
                    state = "参数错误：没有指定抓取源"
                });
                return;
            }
            Crawlers = Sources.Select(x => new Crawler(x).Fetch()).ToArray();
            WriteJson(new
            {
                state = "SUCCESS",
                list = Crawlers.Select(x => new
                {
                    state = x.State,
                    source = x.SourceUrl,
                    url = x.ServerUrl
                })
            });
        }
    }

    public class Crawler
    {
        public string SourceUrl { get; set; }
        public string ServerUrl { get; set; }
        public string State { get; set; }

        // private HttpServerUtility Server { get; set; }


        public Crawler(string sourceUrl)
        {
            this.SourceUrl = sourceUrl;
            // this.Server = server;
        }

        public Crawler Fetch()
        {
            if (!IsExternalIPAddress(this.SourceUrl))
            {
                State = "INVALID_URL";
                return this;
            }

            if (this.SourceUrl.IndexOf(AliyunOssServer.OSSUrl) >= 0)
            {
                State = "SUCCESS";
                this.ServerUrl = GetOSSUrl(this.SourceUrl);
              
                return this;
            }

            var request = HttpWebRequest.Create(this.SourceUrl) as HttpWebRequest;
            using (var response = request.GetResponseAsync().Result as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    State = "Url returns " + response.StatusCode + ", " + response.StatusDescription;
                    return this;
                }
                if (response.ContentType.IndexOf("image") == -1)
                {
                    State = "Url is not an image";
                    return this;
                }
                string fileName = this.SourceUrl;
                if (this.SourceUrl.IndexOf('?') > 0)
                    fileName = this.SourceUrl.Substring(0, this.SourceUrl.IndexOf('?'));
                ServerUrl = PathFormatter.Format(Path.GetFileName(fileName), Config.GetString("catcherPathFormat"));

               

                //var savePath = Path.Combine(Config.WebRootPath, ServerUrl);
                var savePath = Path.Combine(Config.GetString("catcherSaveAbsolutePath"), ServerUrl);
                //

                try
                {
                    var stream = response.GetResponseStream();
                    //
                    if (Config.GetValue<bool>("catcherFtpUpload"))
                    {
                        var fileExt = ServerUrl.Substring(savePath.LastIndexOf('.')).TrimStart('.');
                        var key = Config.GetString("catcherPathFormat") + "." + fileExt;
                        //FtpUpload.UploadFile(stream, savePath, Consts.ImgFtpServer.ip,
                        //    Consts.ImgFtpServer.account, Consts.ImgFtpServer.pwd);
                        AliyunOssUpload.UploadFile(Consts.AliyunOssServer.AccessEndpoint,
                            Consts.AliyunOssServer.AccessKeyId, Consts.AliyunOssServer.AccessKeySecret,
                            Consts.AliyunOssServer.BucketName, key, fileExt, stream);
                    }
                    else
                    {
                        if (!Directory.Exists(Path.GetDirectoryName(savePath)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                        }

                        var reader = new BinaryReader(stream);
                        byte[] bytes;
                        using (var ms = new MemoryStream())
                        {
                            byte[] buffer = new byte[4096];
                            int count;
                            while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                ms.Write(buffer, 0, count);
                            }

                            bytes = ms.ToArray();
                        }

                        File.WriteAllBytes(savePath, bytes);
                    }
                    ServerUrl = GetOSSUrl(ServerUrl);
                    State = "SUCCESS";
                }
                catch (Exception e)
                {
                    State = "抓取错误：" + e.Message;
                }
                return this;
            }
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
                if (objectName.IndexOf("?") >= 0) {
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

        private bool IsExternalIPAddress(string url)
        {
            var uri = new Uri(url);
            switch (uri.HostNameType)
            {
                case UriHostNameType.Dns:
                    var ipHostEntry = Dns.GetHostEntryAsync(uri.DnsSafeHost).Result;
                    foreach (IPAddress ipAddress in ipHostEntry.AddressList)
                    {
                        byte[] ipBytes = ipAddress.GetAddressBytes();
                        if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            if (!IsPrivateIP(ipAddress))
                            {
                                return true;
                            }
                        }
                    }
                    break;

                case UriHostNameType.IPv4:
                    return !IsPrivateIP(IPAddress.Parse(uri.DnsSafeHost));
            }
            return false;
        }

        private bool IsPrivateIP(IPAddress myIPAddress)
        {
            if (IPAddress.IsLoopback(myIPAddress)) return true;
            if (myIPAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                byte[] ipBytes = myIPAddress.GetAddressBytes();
                // 10.0.0.0/24 
                if (ipBytes[0] == 10)
                {
                    return true;
                }
                // 172.16.0.0/16
                else if (ipBytes[0] == 172 && ipBytes[1] == 16)
                {
                    return true;
                }
                // 192.168.0.0/16
                else if (ipBytes[0] == 192 && ipBytes[1] == 168)
                {
                    return true;
                }
                // 169.254.0.0/16
                else if (ipBytes[0] == 169 && ipBytes[1] == 254)
                {
                    return true;
                }
            }
            return false;
        }
    }
}