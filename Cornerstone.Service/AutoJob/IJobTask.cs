using System.Threading.Tasks;
using Cornerstone.Code;

namespace Cornerstone.Service.AutoJob
{
    public interface IJobTask
    {
        Task<AjaxResult> Start();
    }
}
