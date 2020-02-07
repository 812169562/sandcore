using System.Threading.Tasks;

namespace Sand.Cloud.Tencent
{
    public interface IOCR
    {
        Task<IdCard> GetIdCard();
    }
}