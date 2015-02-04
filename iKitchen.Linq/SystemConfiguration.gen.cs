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
    * Company: FalconMeta
    * Author: allentranks
    * */    
	/// <summary>
	/// 系统配置表
	/// </summary>
	public partial class SystemConfiguration : IEntity
	{
		/// <summary>
		/// 
		/// </summary>
        [Required(ErrorMessage = "必填")]
		public System.Int32 Id { get; set; }
        
		/// <summary>
		/// 提成比例，%
		/// </summary>
        [Required(ErrorMessage = "必填")]
		public System.Int32 Ratio { get; set; }
        
		/// <summary>
		/// 分页大小
		/// </summary>
        [Required(ErrorMessage = "必填")]
		public System.Int32 PageSize { get; set; }
        
		/// <summary>
		/// 发票金额折扣
		/// </summary>
        [Required(ErrorMessage = "必填")]
		public System.Int32 InvoiceRatio { get; set; }
        
		/// <summary>
		/// 
		/// </summary>
        [Required(ErrorMessage = "必填")]
		public System.Int32 KanChaDay { get; set; }
        
		/// <summary>
		/// 
		/// </summary>
        [Required(ErrorMessage = "必填")]
		public System.Int32 ZhuanXieDay { get; set; }
        
		/// <summary>
		/// 
		/// </summary>
        [Required(ErrorMessage = "必填")]
		public System.Int32 ShenHeDay { get; set; }
        
		/// <summary>
		/// 
		/// </summary>
        [Required(ErrorMessage = "必填")]
		public System.Int32 GuiDangDay { get; set; }
        
		/// <summary>
		/// 上月片区价格的评估价格误差
		/// </summary>
        [Required(ErrorMessage = "必填")]
		public System.Int32 ZoneDeviationLastMonth { get; set; }
        
		/// <summary>
		/// 本月片区价格的评估价格误差
		/// </summary>
        [Required(ErrorMessage = "必填")]
		public System.Int32 ZoneDeviationThisMonth { get; set; }
        
		/// <summary>
		/// 上月楼盘价格的评估价格误差
		/// </summary>
        [Required(ErrorMessage = "必填")]
		public System.Int32 ProjectDeviationLastMonth { get; set; }
        
		/// <summary>
		/// 上月楼盘价格的评估价格误差
		/// </summary>
        [Required(ErrorMessage = "必填")]
		public System.Int32 ProjectDeviationThisMonth { get; set; }
        
		/// <summary>
		/// 上月片区价格占评估价格比例
		/// </summary>
        [Required(ErrorMessage = "必填")]
		public System.Int32 ZoneRatioLastMonth { get; set; }
        
		/// <summary>
		/// 本月片区价格占评估价格比例
		/// </summary>
        [Required(ErrorMessage = "必填")]
		public System.Int32 ZoneRatioThisMonth { get; set; }
        
		/// <summary>
		/// 上月楼盘价格占评估价格比例
		/// </summary>
        [Required(ErrorMessage = "必填")]
		public System.Int32 ProjectRatioLastMonth { get; set; }
        
		/// <summary>
		/// 本月楼盘价格占评估价格比例
		/// </summary>
        [Required(ErrorMessage = "必填")]
		public System.Int32 ProjectRatioThisMonth { get; set; }
        
		/// <summary>
		/// 评估报告流水号，自增长
		/// </summary>
        [Required(ErrorMessage = "必填")]
		public System.Int32 AssessmentSequenceNumber { get; set; }
        
		/// <summary>
		/// 客户名称，用于生成报告编号
		/// </summary>
        [StringLength(50)]
		public System.String ClientName { get; set; }
        
		/// <summary>
		/// 状态
		/// </summary>
        [Required(ErrorMessage = "必填")]
		public System.Int32 State { get; set; }
        
		/// <summary>
		/// 创建时间
		/// </summary>
        [Required(ErrorMessage = "必填")]
		public System.DateTime CreateOn { get; set; }
        
		/// <summary>
		/// 更新时间
		/// </summary>
        [Required(ErrorMessage = "必填")]
		public System.DateTime UpdateOn { get; set; }
        
    }
}

