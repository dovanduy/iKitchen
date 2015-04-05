using System;

namespace SunTzu.Core.Data
{
    /// <summary>
    /// 实体数据对象接口
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 数据ID标识
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        DateTime UpdateOn { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        int State { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateOn { get; set; }
    }
}
