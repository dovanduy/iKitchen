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
	public partial class OpLog : IEntity
	{
		/// <summary>
		/// 
		/// </summary>
        [Required]
		public System.Int32 Id { get; set; }
        
		/// <summary>
		/// 
		/// </summary>
        [Required]
        [StringLength(128)]
		public System.String UserName { get; set; }
        
		/// <summary>
		/// 
		/// </summary>
        [StringLength(50)]
		public System.String OpType { get; set; }
        
		/// <summary>
		/// 
		/// </summary>
        [StringLength(128)]
		public System.String OpDescription { get; set; }
        
		/// <summary>
		/// 
		/// </summary>
        [StringLength(10)]
		public System.String OpResult { get; set; }
        
		/// <summary>
		/// 
		/// </summary>
        [StringLength(128)]
		public System.String OpRemark { get; set; }
        
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

