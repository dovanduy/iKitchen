using System;
using System.Collections.Generic;
using SunTzu.Core.Data;
using System.ComponentModel.DataAnnotations;

namespace iKitchen.Linq
{
    /*
    * Title: 系统配置表 (generated)
    * Description:
    * Copyright: Copyright (c) 2015
    * Company: iKitchen
    * Author: Allen.Xia
    * */    
	/// <summary>
	/// 系统配置表
	/// </summary>
	public partial class SystemConfiguration : IEntity
	{
		/// <summary>
		/// 
		/// </summary>
        [Required]
		public System.Int32 Id { get; set; }
        
		/// <summary>
		/// 分页大小
		/// </summary>
        [Required]
		public System.Int32 PageSize { get; set; }
        
		/// <summary>
		/// 状态
		/// </summary>
        [Required]
		public System.Int32 State { get; set; }
        
		/// <summary>
		/// 创建时间
		/// </summary>
        [Required]
		public System.DateTime CreateOn { get; set; }
        
		/// <summary>
		/// 更新时间
		/// </summary>
        [Required]
		public System.DateTime UpdateOn { get; set; }
        
    }
}

