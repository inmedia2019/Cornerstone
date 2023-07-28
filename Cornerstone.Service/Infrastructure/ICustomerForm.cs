using System.Threading.Tasks;

namespace Cornerstone.Service
{
    public interface  ICustomerForm
    {
        Task Add(string flowInstanceId, string frmData);
        Task Edit(string flowInstanceId, string frmData);
    }
}
