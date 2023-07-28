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
    public  class ChannelManagerCache : DataFilterService<ChannelEntity>, IDenpendency
    {
        private  ConcurrentDictionary<string, ChannelEntity> _channels;
        private  ChannelService _channelservice { get; set; }
        private TemplateManagerCache _templateManagerCache { get; set; }
        public ChannelManagerCache(IDbContext context) : base(context)
        {
            _channels = new ConcurrentDictionary<string, ChannelEntity>();
            _channelservice = new ChannelService(context);
            _templateManagerCache = new TemplateManagerCache(context);
        }
        

        /// <summary>
        /// 添加栏目
        /// </summary>
        /// <param name="channel"></param>
        public  void AddChannel(ChannelEntity channel)
        {
            if (_channels.ContainsKey(channel.F_Id))
            {
                _channels[channel.F_Id] = channel;
                return;
            }

            _channels.TryAdd(channel.F_Id, channel);
        }

        /// <summary>
        /// 移除栏目
        /// </summary>
        /// <param name="channelId"></param>
        public  void RemoveChannel(string channelId)
        {
            if (_channels.ContainsKey(channelId))
            {
                _channels.TryRemove(channelId, out ChannelEntity channel);
            }
        }
        /// <summary>
        /// 获取栏目
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public  async Task<ChannelEntity> GetChannel(string channelId)
        {
            if (_channels.ContainsKey(channelId))
            {
                return _channels[channelId];
            }
            var model = await _channelservice.GetInfoById(channelId);
            if (model != null)
            {
                AddChannel(model);
            }
            return model;
        }

        /// <summary>
        /// 设置栏目链接方式为1的栏目链接
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="contentId"></param>
        public  async Task SetChannelLink(string channelId, string contentId)
        {
            var channel = await GetChannel(channelId);
            if (channel == null)
            {
                return;
            }
            channel.F_ChannelHref = Cornerstone.Code.GlobalContext.SystemConfig.VirtualDirectory+$"/news/{channelId}/{contentId}";
            AddChannel(channel);
        }
        /// <summary>
        /// 获取栏目列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<ChannelEntity>> GetChannelList()
        {
            var list =  new  List<ChannelEntity>();
            foreach (var dic in _channels)
            {
                list.Add(dic.Value);
            }
            return list;
        }
        /// <summary>
        /// 清空列表
        /// </summary>
        public  void ClearChannels()
        {
            _channels.Clear();
        }

        /// <summary>
        /// 获取栏目模板
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public  async Task<TemplateEntity> GetChannelTemplate(string channelId)
        {
            var channel = await GetChannel(channelId);
            if (channel == null)
            {
                return null;
            }

            return  await _templateManagerCache.GetChannelTemplate(channel.F_ChannelTemplate);
        }


        /// <summary>
        /// 获取栏目的文章模板
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public  async Task<TemplateEntity> GetContentTemplate(string channelId)
        {
            var channel = await GetChannel(channelId);
            if (channel == null)
            {
                return null;
            }

            return await _templateManagerCache.GetContentTemplate(channel.F_ContentTemplate);
        }


    }
}
