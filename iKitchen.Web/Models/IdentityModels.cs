using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace iKitchen.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [StringLength(100)]
        public System.String Email { get; set; }

        [Display(Name = "Nick name")]
        [StringLength(100)]
        public System.String Nickname { get; set; }

        /// <summary>
        /// 性别，0先生，1女士，2小姐
        /// </summary>
        public System.Int32 Sex { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [Display(Name = "Mobile number")]
        public System.String Mobile { get; set; }

        [Display(Name = "Address")]
        public System.String Address { get; set; }

        /// <summary>
        /// 角色，1 普通用户，2 系统管理员，3 超级管理员，4 业务员，5 客服员，7 勘察员，8 撰写员，9 审核员
        /// </summary>
        public System.Int32 RoleId { get; set; }

        /// <summary>
        /// 权限配置，按位取，0是保持角色默认值
        /// </summary>
        public System.Int32 SystemSet { get; set; }

        /// <summary>
        /// 状态，0正常，1禁用
        /// </summary>
        public System.Int32 State { get; set; }

        /// <summary>
        /// 创建时间，iKitchen用作入职时间
        /// </summary>
        public System.DateTime CreateOn { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public System.DateTime UpdateOn { get; set; }

        public System.String AvatarPath { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
    }
}