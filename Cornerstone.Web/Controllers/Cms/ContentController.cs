using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cornerstone.Code;
using Cornerstone.Domain.Entity.Generate;
using Cornerstone.Service.GenerateService;

namespace Cornerstone.Web.Controllers.Cms
{
    public class ContentController : ControllerBase
    {
        public GenerateAppService _generateApp { get; set; }
        public GenerateContent _generateContent { get; set; }
        public GenerateChannel _generateChannel { get; set; }

        /// <summary>
        /// 文章详情
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("news/{channelId}/{id}")]
        public async Task<IActionResult> Index(string channelId, string id)
        {
            if (id == "")
            {
                return Redirect("/404.html");
            }
            var ret = await _generateContent.GenerateContentHtml(channelId, id);
            if (!ret.genStatus)
            {
                return Redirect("/404.html");
            }
            return Html(ret.contentHtml);
        }


        /// <summary>
        /// 栏目页面
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        [Route("channel/{channelId}/{page:int?}")]
        public async Task<IActionResult> Channel(string channelId, int page = 1)
        {
            var ret = await _generateChannel.GenerateChannelHtml(channelId, page);
            if (!ret.genStatus)
            {
                return Redirect("/404.html");
            }
            return Html(ret.contentHtml);
        }

        public async Task<IActionResult> GetChannelTree()
        {
            var channelTree = await _generateApp.GetChannelTree();
            return Json(channelTree);
        }

        public IActionResult Visit()
        {
            return Content("");
        }
    }
}