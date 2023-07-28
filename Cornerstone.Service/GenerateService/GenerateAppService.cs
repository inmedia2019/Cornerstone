using Chloe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cornerstone.Domain.ContentManage;
using Cornerstone.Domain.Entity.Generate;
using Cornerstone.Service.ContentManage;

namespace Cornerstone.Service.GenerateService
{
    /// <summary>
    /// 模板数据源，通用包含了新闻内容，面包屑，栏目信息的基本数据
    /// </summary>
    public class GenerateAppService : DataFilterService<ChannelEntity>, IDenpendency
    {
        public ContentService _contentservice { get; set; }
        public ChannelService _channelservice { get; set; }
        public GenerateAppService(IDbContext context) : base(context)
        {
            _contentservice = new ContentService(context);
            _channelservice = new ChannelService(context);
        }
        #region 内容数据
        /// <summary>
        /// 获取新闻详情数据
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        public async Task<ContentModel> GetContentInfo(string contentId)
        {
            var model = await _contentservice.GetInfoById(contentId);
            ContentModel contentmodel = new ContentModel();
            if (model != null)
            {
                contentmodel.id = model.F_Id;
                contentmodel.title = model.F_Titile;
                contentmodel.sub_title = model.F_SubTitle;
                contentmodel.content = model.F_Content;
                contentmodel.summary = model.F_Summary;
                contentmodel.cover_image = model.F_CoverImage;
                contentmodel.author = model.F_Author;
                contentmodel.source = model.F_Source;
                contentmodel.channel_id = model.F_ChannelId;
                contentmodel.ip_limit = model.F_IpLimit;
                contentmodel.is_top = model.F_IsTop;
                contentmodel.tags = model.F_Tags;
                contentmodel.recommend = model.F_IsRecommend;
                contentmodel.content_href = model.F_ContentHref;
                contentmodel.hit_count = model.F_HitCount;
                contentmodel.publish_time = model.F_CreatorTime;
                contentmodel.last_edit_time = model.F_LastModifyTime;
                contentmodel.location = await GetNaviLocation(model.F_ChannelId);
            }
            return contentmodel;
        }


        /// <summary>
        /// 根据栏目索引获取数据详情
        /// </summary>
        /// <param name="contentIds"></param>
        /// <returns></returns>
        public async Task<List<ContentModel>> GetContentSummary(string channelname, int page = 1, int psize = 10)
        {

            var channelId = "";
            var channelmodel = await _channelservice.GetInfoByChannelIndex(channelname);
            if (channelmodel != null)
            {
                channelId = channelmodel.F_Id;
            }
            if (channelId == "")
            {
                return new List<ContentModel>();
            }
            ContentModel contentmodel = new ContentModel();
            var contentlist = await _contentservice.GetListByChannelId(channelId, page, psize);
            var list = new List<ContentModel>();
            foreach (var model in contentlist)
            {
                contentmodel.id = model.F_Id;
                contentmodel.title = model.F_Titile;
                contentmodel.sub_title = model.F_SubTitle;
                contentmodel.content = model.F_Content;
                contentmodel.summary = model.F_Summary;
                contentmodel.cover_image = model.F_CoverImage;
                contentmodel.author = model.F_Author;
                contentmodel.source = model.F_Source;
                contentmodel.channel_id = model.F_ChannelId;
                contentmodel.ip_limit = model.F_IpLimit;
                contentmodel.is_top = model.F_IsTop;
                contentmodel.tags = model.F_Tags;
                contentmodel.recommend = model.F_IsRecommend;
                contentmodel.content_href = model.F_ContentHref;
                contentmodel.hit_count = model.F_HitCount;
                contentmodel.publish_time = model.F_CreatorTime;
                contentmodel.last_edit_time = model.F_LastModifyTime;
                contentmodel.location = await GetNaviLocation(model.F_ChannelId);
                contentmodel.content_href = Cornerstone.Code.GlobalContext.SystemConfig.VirtualDirectory+$"/news/{model.F_ChannelId}/{model.F_Id}";
                list.Add(contentmodel);
            };
            if (list.Count == 0)
            {
                return list;
            }
            return list;
        }




        /// <summary>
        /// 栏目页面获取数据带分页
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="page"></param>
        /// <param name="psize"></param>
        /// <returns></returns>
        public async Task<ContentPageModel> GetContentSummaryPage(string channelId, int page = 1, int psize = 10)
        {
            var contentpagemodel = new ContentPageModel();
            var contentlist = await _contentservice.GetListByChannelId(channelId, page, psize);
            var list = new List<ContentModel>();
            foreach (var model in contentlist)
            {
                ContentModel contentmodel = new ContentModel();
                contentmodel.id = model.F_Id;
                contentmodel.title = model.F_Titile;
                contentmodel.sub_title = model.F_SubTitle;
                contentmodel.content = model.F_Content;
                contentmodel.summary = model.F_Summary;
                contentmodel.cover_image = model.F_CoverImage;
                contentmodel.author = model.F_Author;
                contentmodel.source = model.F_Source;
                contentmodel.channel_id = model.F_ChannelId;
                contentmodel.ip_limit = model.F_IpLimit;
                contentmodel.is_top = model.F_IsTop;
                contentmodel.tags = model.F_Tags;
                contentmodel.recommend = model.F_IsRecommend;
                contentmodel.content_href = model.F_ContentHref;
                contentmodel.hit_count = model.F_HitCount;
                contentmodel.publish_time = model.F_CreatorTime;
                contentmodel.last_edit_time = model.F_LastModifyTime;
                contentmodel.location = await GetNaviLocation(model.F_ChannelId);
                contentmodel.content_href = Cornerstone.Code.GlobalContext.SystemConfig.VirtualDirectory + $"/news/{model.F_ChannelId}/{model.F_Id}";
                list.Add(contentmodel);
            };
            contentpagemodel.Contents = list;
            contentpagemodel.PageHtml = GetPageHtml(list.Count(), page, psize, channelId);
            return contentpagemodel;
        }
        #endregion

        #region 栏目数据 栏目数据必须存在首页栏目，并且为唯一顶级栏目，这是一个约束

        /// <summary>
        /// 获取栏目数据
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public async Task<ChannelModel> GetChannel(String channelId)
        {
            var channel = await _channelservice.GetInfoById(channelId);
            if (channel == null)
            {
                return null;
            }

            var model = new ChannelModel();
            model.id = channel.F_Id;
            model.parent_id = channel.F_ParentId;
            model.sort_num = channel.F_Order;
            model.channel_name = channel.F_ChannelName;
            model.channel_index = channel.F_ChannelIndex;
            model.channel_image = channel.F_ChannelImages;
            model.channel_href = channel.F_ChannelHref;
            model.channel_template = channel.F_ChannelTemplate;
            model.dis_drawing = channel.F_DisDrawing;
            model.current = 1;
            if (string.IsNullOrEmpty(model.channel_href))
            {
                model.channel_href = $"/channel/{channelId}";
            }
            model.location = await GetNaviLocation(channelId);
            return model;

        }
        /// <summary>
        /// 获取栏目数据列表 必须设置首页栏目
        /// </summary>
        /// <returns></returns>
        public async Task<List<ChannelModel>> GetChannelTree(string channleId = "")
        {
            var channels = await _channelservice.GetList();
            var homeId = channels.Where(s => s.F_ChannelName == "首页").Select(s => s.F_Id).FirstOrDefault();
            if (homeId == "")
            {
                return new List<ChannelModel>();
            }
            var list = await InitChild(homeId, channels, channleId);
            var home = new ChannelModel { id = "0", channel_href = "/index.html", channel_name = "首页" };
            if (channleId == "")
            {
                home.current = 1;
            }
            list.Insert(0, home);
            return list;
        }

        /// <summary>
        /// 迭代子栏目
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="channels"></param>
        /// <returns></returns>
        private async Task<List<ChannelModel>> InitChild(string pid, List<ChannelEntity> channels, string channleId)
        {
            var topChannels = new List<ChannelModel>();

            var channellist = await _channelservice.GetListByParentId(pid);
            foreach (var model in channellist)
            {
                ChannelModel channelmodel = new ChannelModel();
                channelmodel.id = model.F_Id;
                channelmodel.parent_id = model.F_ParentId;
                channelmodel.sort_num = model.F_Order;
                channelmodel.channel_name = model.F_ChannelName;
                channelmodel.channel_index = model.F_ChannelIndex;
                channelmodel.channel_image = model.F_ChannelImages;
                channelmodel.channel_href = model.F_ChannelHref;
                channelmodel.channel_template = model.F_ChannelTemplate;
                channelmodel.dis_drawing = model.F_DisDrawing;
                topChannels.Add(channelmodel);
            };

            try
            {

                //设置连接和导航
                foreach (var channel in topChannels)
                {
                    if (string.IsNullOrEmpty(channel.channel_href))
                    {
                        channel.channel_href = $"/channel/{channel.id}";
                        channel.location = await GetNaviLocation(channel.id);
                    }
                    if (channel.id == channleId)
                    {
                        channel.current = 1;
                    }
                    var subChannels = channels.Where(s => s.F_ParentId == channel.id).ToList();
                    if (subChannels.Count > 0)
                    {
                        var childs = await InitChild(channel.id, channels, channleId);
                        channel.sub_count = childs.Count;
                        if (channel.sub_count > 0)
                        {
                            channel.sub_channels = childs;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return topChannels;
        }
        #endregion





        /// <summary>
        /// 获取栏目定位  
        /// </summary> 
        /// <param name="channelId"></param>
        /// <returns>首页>新闻中心</returns>
        private async Task<string> GetNaviLocation(string channelId)
        {
            List<string> channelNames = new List<string>();

            var channel = await _channelservice.GetInfoById(channelId);
            if (channel != null)
            {
                string href = $"/channel/{channel.F_Id}";
                if (!string.IsNullOrEmpty(channel.F_ChannelHref))
                {
                    href = channel.F_ChannelHref;
                }

                string channelHref = $"<a href='{href}'>{channel.F_ChannelName}</a>";
                channelNames.Insert(0, channelHref);
                string channelPid = channel.F_ParentId;
                while (channelPid != "0")
                {
                    channel = await _channelservice.GetInfoById(channelPid);
                    if (channel != null)
                    {
                        string channelHref2 = "<a href='/index.html'>首页</a>";
                        if (channel.F_ChannelName != "首页")
                        {
                            if (!string.IsNullOrEmpty(channel.F_ChannelHref))
                            {
                                href = channel.F_ChannelHref;
                            }
                            channelHref2 = $"<a href='{href}'>{channel.F_ChannelName}</a>";
                        }
                        channelNames.Insert(0, channelHref2);
                        channelPid = channel.F_ParentId;
                    }
                }

            }

            if (channelNames.Count == 0)
            {
                return "<a href='/index.html'>首页</a>";
            }

            return string.Join(' ', channelNames);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="total"></param>
        /// <param name="psize"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public string GetPageHtml(long total, int pageIndex, int psize, string channelId)
        {
            long page = total / psize;
            if (page == 0)
            {
                return "";
            }
            if (page > 20)
            {
                page = 20;
            }
            else
            {
                if (total % psize > 0)
                {
                    page++;
                }
            }
            int lastPage = pageIndex - 1;
            int nextPage = pageIndex + 1;
            if (lastPage < 1)
            {
                lastPage = 1;
            }
            if (nextPage > page)
            {
                nextPage = (int)page;
            }
            var html = "<div class=\"page-nav\">";
            html += $"<a class=\"prev page-numbers\" href=\"/channel/{channelId}/{lastPage}\">« 上一页</a>";
            for (long i = 1; i <= page; i++)
            {
                if (i == pageIndex)
                {
                    html += $"<span class=\"page-numbers current\">{i}</span>";
                }
                else
                {
                    html += $"<a class=\"page-numbers\" href=\"/channel/{channelId}/{i}\">{i}</a>";
                }

            }
            html += $"<a class=\"next page-numbers\" href=\"/channel/{channelId}/{nextPage}\"> 下一页 »</a>";
            html += "</div>";
            return html;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="total"></param>
        /// <param name="psize"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public string GetPageHtml(long total, int psize, int channelId)
        {
            long page = total / psize;
            if (page == 0)
            {
                return "";
            }
            if (page > 20)
            {
                page = 20;
            }
            else
            {
                if (total % psize > 0)
                {
                    page++;
                }
            }

            var html = "<nav aria-label=\"Page navigation\">";
            html += "<ul class=\"pagination\">";
            html += "<li>";
            html += "<a href =\"#\" aria-label=\"上一页\">";
            html += " <span aria-hidden=\"true\">&laquo;</span>";
            html += "</a>";
            html += "</li>";
            for (long i = 1; i <= page; i++)
            {
                html += $"<li><a href=\"/channel/{channelId}/{i}\">{i}</a></li>";
            }
            html += "<li>";
            html += "<a href=\"#\" aria-label=\"下一页\">";
            html += " <span aria-hidden=\"true\">&raquo;</span>";
            html += "</a>";
            html += " </li>";
            html += "</ul>";
            html += "</nav>";
            return html;
        }
    }
}
