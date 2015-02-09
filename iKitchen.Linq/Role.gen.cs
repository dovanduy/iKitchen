using System;
using System.Collections.Generic;
using SunTzu.Core.Data;
using System.ComponentModel.DataAnnotations;

namespace iKitchen.Linq
{
    /*
    * Title: 角色表 (generated)
    * Description:
    * Copyright: Copyright (c) 2015
    * Company: iKitchen
    * Author: allentranks
    * */    
	/// <summary>
	/// 角色表
	/// </summary>
	public partial class Role : IEntity
	{
		/// <summary>
		/// 
		/// </summary>
        [Required]
		public System.Int32 Id { get; set; }
        
		/// <summary>
		/// 角色名称
		/// </summary>
        [Required]
        [StringLength(50)]
		public System.String Title { get; set; }
        
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

