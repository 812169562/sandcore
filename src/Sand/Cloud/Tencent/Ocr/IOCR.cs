using System.Threading.Tasks;
using Sand.Dependency;

namespace  Sand.Cloud.Tencent.Ocr
{
    /// <summary>
    /// 图像识别
    /// </summary>
    public interface IOCR:IDependency
    {
        /// <summary>
        /// 身份证识别
        /// </summary>
        /// <param name="inputParm">输入参数</param>
        /// <returns></returns>
        Task<IdCard> GetIdCard(InputParm inputParm);
    }
}