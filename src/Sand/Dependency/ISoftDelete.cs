using System.ComponentModel.DataAnnotations;

namespace Sand.Dependency
{
    /// <summary>
    /// 是否逻辑删除
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// 删除标志
        /// </summary>
        [Required]
        bool IsDeleted { get; set; }
    }
}
