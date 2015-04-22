using System;
using System.Collections.Generic;
using SunTzu.Core.Data;
using System.ComponentModel.DataAnnotations;

namespace iKitchen.Linq
{
    /*
    * Title:  (generated)
    * Description:
    * Copyright: Copyright (c) 2015
    * Company: iKitchen
    * Author: Allen.Xia
    * */    
	/// <summary>
	/// 
	/// </summary>
	public partial class Feedback : IEntity
	{
		/// <summary>
		/// 
		/// </summary>
        [Required]
		public System.Int32 Id { get; set; }
        
		/// <summary>
		/// 
		/// </summary>
        [StringLength(128)]
		public System.String UserId { get; set; }
        
		/// <summary>
		/// 
		/// </summary>
        [StringLength(128)]
		public System.String HostUserId { get; set; }
        
		/// <summary>
		/// 
		/// </summary>
        [Required]
		public System.Int32 EventId { get; set; }
        
		/// <summary>
		/// 0~9, use stars on UI
		/// </summary>
        [Required]
		public System.Int32 Rate { get; set; }
        
		/// <summary>
		/// 
		/// </summary>
        [StringLength(500)]
		public System.String Description { get; set; }
        
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

