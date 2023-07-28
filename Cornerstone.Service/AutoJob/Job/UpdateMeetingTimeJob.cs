using Chloe;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WaterCloud.Code;
using WaterCloud.Domain.ContentManage;
using WaterCloud.Service.ContentManage;

namespace WaterCloud.Service.AutoJob.Job
{
   
    public class UpdateMeetingTimeJob:IJobTask
    {
        public ContentService _service { get; set; }
        public UpdateMeetingTimeJob(IDbContext context)
        {
            _service = new ContentService(context);
        }
        public async Task<AjaxResult> Start()
        {
            AjaxResult obj = new AjaxResult();
            try
            {
                var data = await _service.GetMeetingTimePeriodList();
                foreach (ContentEntity c in data)
                {
                    var id = c.F_Id;
                    var meetingTimePeriod = c.F_MeetingTimePeriod;
                    var meetingStartTime = c.F_MeetingStartTime;
                    var strArr1 = meetingTimePeriod.Split(',');
                    foreach (string s1 in strArr1)
                    {
                        var strArr2 = s1.Split('~');
                        if (Convert.ToDateTime(strArr2[0].ToString()) > DateTime.Now)
                        {
                            if (Convert.ToDateTime(meetingStartTime).ToString("yyyy-MM-dd HH:mm:ss") != Convert.ToDateTime(strArr2[0].ToString()).ToString("yyyy-MM-dd HH:mm:ss"))
                            {
                                ContentEntity entity = await _service.GetInfoById(id);
                                entity.F_MeetingStartTime = Convert.ToDateTime(strArr2[0]);
                                entity.F_MeetingEndTime = Convert.ToDateTime(strArr2[1]);
                                await _service.SubmitForm(entity, id);
                            }
                        }
                    }
                }
                obj.state = ResultType.success.ToString();
                obj.message = "服务器状态更新成功！";
            }
            catch (Exception ex)
            {
                obj.state = ResultType.error.ToString();
                obj.message = "服务器状态更新失败！" + ex.Message;
            }
            return obj;
        }
    }
}
