using Chloe;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cornerstone.Domain.ContentManage;
using Cornerstone.Service.ContentManage;

namespace Cornerstone.Service.GenerateService
{
    public class TemplateManagerCache : DataFilterService<TemplateEntity>, IDenpendency
    {
        private TemplateService _service { get; set; }
        private ConcurrentDictionary<string, TemplateEntity> _templates;
        private ConcurrentDictionary<string, TemplateEntity> _defaultTemplates;
        public TemplateManagerCache(IDbContext context) : base(context)
        {
            _templates = new ConcurrentDictionary<string, TemplateEntity>();
            _defaultTemplates = new ConcurrentDictionary<string, TemplateEntity>();
            _service = new TemplateService(context);
            AddDefaultTemplate();
        }

        /// <summary>
        /// 1-首页，2-栏目模板，3-内容模板，4-单页模板
        /// </summary>
        private void AddDefaultTemplate()
        {
            _defaultTemplates.TryAdd("1", new TemplateEntity());
            _defaultTemplates.TryAdd("2", new TemplateEntity());
            _defaultTemplates.TryAdd("3", new TemplateEntity());
        }

        /// <summary>
        /// 添加栏目
        /// </summary>
        /// <param name="channel"></param>
        public void AddTemplate(TemplateEntity template)
        {
            if (template.F_IsDefault == true)
            {
                if (template.F_TemplateMode == "1001")
                {
                    _defaultTemplates["1"] = template;
                }
                else if (template.F_TemplateMode == "1002")
                {
                    _defaultTemplates["2"] = template;
                }
                else if (template.F_TemplateMode == "1003")
                {
                    _defaultTemplates["3"] = template;
                }
            }
            if (_templates.ContainsKey(template.F_Id))
            {
                _templates[template.F_Id] = template;
                return;
            }
            _templates.TryAdd(template.F_Id, template);
        }

        /// <summary>
        /// 获取首页模板
        /// </summary>
        /// <returns></returns>
        public TemplateEntity GetHomeTemplate()
        {
            return _defaultTemplates["1"];
        }
        /// <summary>
        /// 获取栏目
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public async Task<TemplateEntity> GetTemplate(string template)
        {
            if (_templates.ContainsKey(template))
            {
                return _templates[template];
            }
            var model = await _service.GetInfoById(template);
            if (model != null)
            {
                AddTemplate(model);
            }
            return model;
        }

        public void ClearTemplates()
        {
            _defaultTemplates["1"] = new TemplateEntity();
            _defaultTemplates["2"] = new TemplateEntity();
            _defaultTemplates["3"] = new TemplateEntity();
            _templates.Clear();
        }

        /// <summary>
        /// 获取栏目模板
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public async Task<TemplateEntity> GetChannelTemplate(string templateId)
        {
            if (templateId == "")
            {
                var defaultChannelTemplate = _defaultTemplates["2"];
                if (defaultChannelTemplate.F_Id == "")
                {
                    return null;
                }
                return defaultChannelTemplate;
            }

            return await GetTemplate(templateId);
        }


        /// <summary>
        /// 获取内容模板
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public async Task<TemplateEntity> GetContentTemplate(string templateId)
        {
            if (templateId == "")
            {
                var defaultContentTemplate = _defaultTemplates["3"];
                if (defaultContentTemplate.F_Id == "")
                {
                    return null;
                }
                return defaultContentTemplate;
            }

            return await GetTemplate(templateId);
        }
    }
}
