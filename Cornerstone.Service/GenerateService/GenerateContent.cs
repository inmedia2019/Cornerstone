using Chloe;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VTemplate.Engine;
using Cornerstone.Code;
using Cornerstone.Domain.ContentManage;
using Cornerstone.Domain.Entity.Generate;
using Cornerstone.Service.ContentManage;

namespace Cornerstone.Service.GenerateService
{
    public class GenerateContent : DataFilterService<ContentEntity>, IDenpendency
    {
        private ContentService _contentservice { get; set; }
        private ChannelManagerCache _channelManagerCache { get; set; }
        private readonly GenerateAppService _contentApp;
        public GenerateContent(IDbContext context) : base(context)
        {
            _contentApp = new GenerateAppService(context);
            _channelManagerCache = new ChannelManagerCache(context);
        }
        /// <summary>
        /// 当前页面的模板文档对象
        /// </summary>
        protected TemplateDocument Document
        {
            get;
            private set;
        }

        /// <summary>
        /// 返回渲染后的页面文件
        /// </summary>
        /// <param name="content"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public async Task<(bool genStatus, string contentHtml)> GenerateContentHtml(string channelId, string id)
        {
            try
            {
                Stopwatch watcher = new Stopwatch();
                watcher.Start();
                var templateModel = await _channelManagerCache.GetContentTemplate(channelId);
                if (templateModel == null)
                {
                    return (false, "");
                }
                var content = await _contentApp.GetContentInfo(id);
                if (content == null)
                {
                    return (false, "");
                }
                //加载模板 先取缓存，没有再初始化一个并且加入缓存
                this.Document = RenderDocumentCache.GetRenderDocument(templateModel.F_Id);
                if (this.Document == null)
                {
                    string templateFile = Path.Combine(GlobalParamsDto.WebRoot, templateModel.F_TemplateFile);
                    LogHelper.Write(templateFile);
                    this.Document = new TemplateDocument(templateModel.F_TemplateContent, GlobalParamsDto.WebRoot, templateFile);
                    RenderDocumentCache.AddRenderDocument(templateModel.F_Id, this.Document);
                }

                this.Document.Variables.SetValue("this", this);
                this.Document.Variables.SetValue("news", content);
                string renderHtml = this.Document.GetRenderText();
                //renderHtml = HtmlPlayerHandler.CreateVideo(renderHtml);
                watcher.Stop();
                string msg = $"渲染内容页耗时：{watcher.ElapsedMilliseconds} ms";

                return (true, renderHtml);
            }
            catch (Exception ex)
            {

            }
            return (false, "");
        }

        /// <summary>
        /// 加载模板文件，生成模板内容
        /// </summary>
        /// <param name="contentId"></param>
        public async Task CreateHtml(string contentId)
        {
            try
            {
                var content = await _contentservice.GetInfoById(contentId);
                if (content == null)
                {
                    return;
                }
                string contentFolder = $@"static/content/{content.F_ChannelId}";
                string contentFile = $@"static/content/{content.F_ChannelId}/{ content.F_Id}.html";

                string contentFolderPath = Path.Combine(GlobalParamsDto.WebRoot, contentFolder);
                string contentFilePath = Path.Combine(GlobalParamsDto.WebRoot, contentFile);
                string templatePath = Path.Combine(GlobalParamsDto.WebRoot, "Template/article.html");
                if (!File.Exists(templatePath))
                {
                    return;
                }
                if (!Directory.Exists(contentFolderPath))
                {
                    Directory.CreateDirectory(contentFolderPath);
                }


                //加载模板
                //this.LoadTemplateFile(templatePath);
                //this.InitPageTemplate(content);
                //this.Document = new TemplateDocument(templatePath, fileName);

                using (var filestream = new FileStream(contentFilePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    string renderHtml = this.Document.GetRenderText();

                    using (StreamWriter writer = new StreamWriter(filestream, Encoding.UTF8))
                    {

                        writer.WriteLine(renderHtml);
                        writer.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 装载模板文件
        /// </summary>
        /// <param name="fileName"></param>
        protected virtual void LoadTemplateFile(string fileName)
        {
            this.Document = null;
            //if ("cache".Equals(this.TestType, StringComparison.InvariantCultureIgnoreCase) || this.IsLoadCacheTemplate)
            //{
            //    //测试缓存模板文档
            //    this.Document = TemplateDocument.FromFileCache(fileName, Encoding.UTF8, this.DocumentConfig);
            //}
            //else
            //{
            //    //测试实例模板文档
            //    this.Document = new TemplateDocument(fileName, Encoding.UTF8, this.DocumentConfig);
            //}
            //测试实例模板文档
            //this.Document = new TemplateDocument(fileName, Encoding.UTF8);
        }

        protected virtual void LoadTemplate(string templateContent, string fileName)
        {
            this.Document = new TemplateDocument(templateContent, GlobalParamsDto.WebRoot, fileName);
        }
        protected virtual void InitPageTemplate(ContentModel content)
        {
            this.Document.Variables.SetValue("this", this);
            this.Document.Variables.SetValue("news", content);
        }
    }
}
