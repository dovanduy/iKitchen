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
    * Author: allentranks
    * */    
	/// <summary>
	/// 
	/// </summary>
	public partial class Event : IEntity
	{
		/// <summary>
		/// 
		/// </summary>
        [Required]
		public System.Int32 Id { get; set; }
        
		/// <summary>
		/// The host user id
		/// </summary>
        [Required]
        [StringLength(128)]
		public System.String UserId { get; set; }
        
		/// <summary>
		/// 
		/// </summary>
        [Required]
        [StringLength(100)]
		public System.String Title { get; set; }
        
		/// <summary>
		/// 
		/// </summary>
        [Required]
        [StringLength(200)]
		public System.String Summary { get; set; }
        
		/// <summary>
		/// 
		/// </summary>
		public System.String Description { get; set; }
        
		/// <summary>
		/// 
		/// </summary>
        [Required]
		public System.Decimal Price { get; set; }
        
		/// <summary>
		/// Use as expiry time for one time Events
		/// </summary>
		public System.DateTime? EventTime { get; set; }
        
		/// <summary>
		/// True for one time Event, default to False
		/// </summary>
        [Required]
		public System.Boolean IsOneTime { get; set; }
        
		/// <summary>
		/// Address, copy default value from Profile
		/// </summary>
        [StringLength(100)]
		public System.String Address { get; set; }
        
		/// <summary>
		/// 0 No limit
		/// </summary>
        [Required]
		public System.Int32 GuestLimitCount { get; set; }
        
		/// <summary>
		/// Original Event id if it is clone from existing Event
		/// </summary>
		public System.Int32? OriginalEventId { get; set; }
        
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

