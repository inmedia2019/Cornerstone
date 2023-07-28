using Chloe;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VTemplate.Engine;
using Cornerstone.Domain.ContentManage;
using Cornerstone.Domain.Entity.Generate;
using Cornerstone.Service.ContentManage;

namespace Cornerstone.Service.GenerateService
{
    public class GenerateHome : DataFilterService<TemplateEntity>, IDenpendency
    {
        private readonly GenerateAppService _generateContentApp;
        private TemplateService _service { get; set; }
        protected TemplateDocument Document
        {
            get;
            private set;
        }
        public GenerateHome(IDbContext context) : base(context)
        {
            _generateContentApp = new GenerateAppService(context);
            _service = new TemplateService(context);
        }
        /// <summary>
        /// 当前页面的模板文档对象
        /// </summary>
       

        /// <summary>
        /// 返回渲染后的模板文件
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public async Task GenerateHomeHtml(string keyValue)
        {
            try
            {
                var templateModel = await _service.GetInfoById(keyValue);
                if (templateModel.F_Id == "")
                {
                    throw new Exception("找不到模板");
                }
                //加载模板 先取缓存，没有再初始化一个并且加入缓存
                this.Document = RenderDocumentCache.GetRenderDocument(templateModel.F_Id);
                if (this.Document == null)
                {
                    string templateFile = Path.Combine(GlobalParamsDto.WebRoot, templateModel.F_TemplateFile);
                    this.Document = new TemplateDocument(templateModel.F_TemplateContent, GlobalParamsDto.WebRoot, templateFile);
                    RenderDocumentCache.AddRenderDocument(templateModel.F_Id, this.Document);
                }
                //this.LoadTemplate(templateModel.template_content);

                this.Document.Variables.SetValue("this", this);
                //站点基本信息
                //var site = SiteManagerCache.GetSiteInfo();
                //this.Document.Variables.SetValue("site", site);
                //设置顶部导航条数据
                var navigations = _generateContentApp.GetChannelTree();

                //var tablename= ((VTemplate.Engine.Tag)((VTemplate.Engine.Tag)this.Document.InnerElements[3]).InnerElements[1]).Attributes.GetValue("tableName");
                this.Document.Variables.SetValue("navigations", navigations.Result);

                //获取栏目文章模板
                ElementCollection<Template> templates = this.Document.GetChildTemplatesByName("channels");
                foreach (Template template in templates)
                {
                    string total = template.Attributes.GetValue("total", "10");
                    //根据模板块里定义的type属性条件取得新闻数据
                    var data = _generateContentApp.GetContentSummary(template.Attributes.GetValue("type"), 1, int.Parse(total));
                    //设置变量newsdata的值
                    template.Variables.SetValue("contents", data);

                    //取得模板块下Id为newslist的标签(也即是在cnblogs_newsdata.html文件中定义的foreach标签)
                    //Tag tag = template.GetChildTagById("newslist");
                    //if (tag is ForEachTag)
                    //{
                    //    //如果标签为foreach标签则设置其BeforeRender事件用于设置变量表达式{$:#.news.url}的值
                    //    tag.BeforeRender += new System.ComponentModel.CancelEventHandler(Tag_BeforeRender);
                    //}
                }

                string contentFilePath = Path.Combine(GlobalParamsDto.WebRoot, "index.html");
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


    }
}
