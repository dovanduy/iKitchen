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
	public partial class ResetPassword : IEntity
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
		public System.Guid Guid { get; set; }
        
		/// <summary>
		/// 
		/// </summary>
        [Required]
        [StringLength(128)]
		public System.String UserId { get; set; }
        
		/// <summary>
		/// 
		/// </summary>
        [Required]
		public System.DateTime CreateOn { get; set; }
        
		/// <summary>
		/// 
		/// </summary>
        [Required]
		public System.DateTime UpdateOn { get; set; }
        
		/// <summary>
		/// 
		/// </summary>
        [Required]
		public System.Int32 State { get; set; }
        
    }
}

