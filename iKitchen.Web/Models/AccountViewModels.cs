﻿using System;
using System.ComponentModel.DataAnnotations;

namespace iKitchen.Web.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required(ErrorMessage = "{0}不能为空")]
        [DataType(DataType.Password)]
        [Display(Name = "当前密码")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(100, ErrorMessage = "{0}长度至少为{2}个字符", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("NewPassword", ErrorMessage = "确认密码与新密码不匹配")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0}不能为空")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住我？")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "用户名不能为空")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [StringLength(100, ErrorMessage = "{0}长度至少{2}为6个字符", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "初始密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "确认密码与新密码不匹配")]
        public string ConfirmPassword { get; set; }
    }

    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "{0}不能为空")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "用户名")]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(100, ErrorMessage = "{0}长度至少{2}为6个字符", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "初始密码")]
        public string Password { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "无效的Email地址")]
        [StringLength(100)]
        public string Email { get; set; }

        /// <summary>
        /// 入职时间
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空")]
        public DateTime CreateOn { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(11)]
        [Display(Name = "手机号码")]
        [RegularExpression("[1][3,4,5,8][0-9]{9}$", ErrorMessage = "手机号码格式不正确")]
        public string Mobile { get; set; }

        /// <summary>
        /// 角色，1普管用户，2？？用户（未启用），3超管用户
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空")]
        public int RoleId { get; set; }
    }

}