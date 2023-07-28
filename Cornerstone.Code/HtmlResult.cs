using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cornerstone.Code
{
    public class HtmlResult : ActionResult
    {
        /// <summary>
        /// Html字符串
        /// </summary>
        public object Value { get; private set; }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="value"></param>
        public HtmlResult(object value)
        {
            Value = value;
        }
        /// <summary>
        /// Result执行者
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public override async Task ExecuteResultAsync(ActionContext context)
        {
            var services = context.HttpContext.RequestServices;
            var executor = services.GetRequiredService<IActionResultExecutor<HtmlResult>>();
            await executor.ExecuteAsync(context, this);
        }
    }
}
