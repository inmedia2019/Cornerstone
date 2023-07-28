using Chloe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VTemplate.Engine;
using Cornerstone.Domain.ContentManage;
using Cornerstone.Domain.Entity.Generate;

namespace Cornerstone.Service.GenerateService
{
    public class GenerateChannel : DataFilterService<ChannelEntity>, IDenpendency
    {
        private readonly GenerateAppService _generateContentApp;
        private TemplateManagerCache _templateManagerCache { get; set; }
        public GenerateChannel(IDbContext context) : base(context)
        {
            _generateContentApp = new GenerateAppService(context);
            _templateManagerCache = new TemplateManagerCache(context);
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
        /// 返回渲染后的模板文件
        /// </summary>
        /// <param name="content"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public async Task<(bool genStatus, string contentHtml)> GenerateChannelHtml(string channelId, int page)
        {
            try
            {
                //Stopwatch watcher = new Stopwatch();
                //watcher.Start();
                var channel = await _generateContentApp.GetChannel(channelId);
                if (channel == null)
                {
                    return (false, null);
                }
                var templateModel = await _templateManagerCache.GetChannelTemplate(channel.channel_template);
                if (templateModel == null)
                {
                    return (false, null);
                }

                //加载模板 先取缓存，没有再初始化一个并且加入缓存
                //this.LoadTemplate(templateModel.template_content);"Template/Channel"
                this.Document = RenderDocumentCache.GetRenderDocument(templateModel.F_Id);
                if (this.Document == null)
                {
                    string templateFile = Path.Combine(GlobalParamsDto.WebRoot, templateModel.F_TemplateFile);
                    this.Document = new TemplateDocument(templateModel.F_TemplateContent, GlobalParamsDto.WebRoot, templateFile);
                    RenderDocumentCache.AddRenderDocument(templateModel.F_Id, this.Document);
                }

                this.Document.Variables.SetValue("this", this);
                //站点数据
                //var site = SiteManagerCache.GetSiteInfo();
                //site.site_title = channel.channel_name;
                //this.Document.Variables.SetValue("site", site);
                //设置顶部导航条数据
                var navigations = _generateContentApp.GetChannelTree(channelId);
                this.Document.Variables.SetValue("navigations", navigations.Result);

                //解析文章列表模板设置数据源
                Tag element = this.Document.GetChildTagById("contents");
                string total = element.Attributes.GetValue("total", "8");

                var contents = await _generateContentApp.GetContentSummaryPage(channelId, page, int.Parse(total));
                //设置变量newsdata的值
                this.Document.Variables.SetValue("channel", channel);
                this.Document.Variables.SetValue("contents", contents.Contents);
                this.Document.Variables.SetValue("pageHtml", contents.PageHtml);
                string renderHtml = this.Document.GetRenderText();


                //watcher.Stop();
                //string msg = $"渲染栏目耗时：{watcher.ElapsedMilliseconds} ms";

                //LogNHelper.Info(msg);

                return (true, renderHtml);
            }
            catch (Exception ex)
            {
               
            }
            return (false, null);
        }

    }
}
