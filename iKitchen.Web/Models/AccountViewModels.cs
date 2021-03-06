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

    public class ExternalLoginViewModel
    {
        public string Action { get; set; }
        public string ReturnUrl { get; set; }
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
        [Required(ErrorMessage = "{0} must not be empty")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} must not be empty")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remeber me?")]
        public bool RememberMe { get; set; }
    }

    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "{0} must not be empty")]
        [Display(Name = "Username")]
        public string UserName { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "{0} must not be empty")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password must not be empty")]
        [StringLength(100, ErrorMessage = "At least 6 charactors.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Confirm password and New password do not match")]
        public string ConfirmPassword { get; set; }

        public string Guid { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Username must not be empty")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password must not be empty")]
        [StringLength(100, ErrorMessage = "At least 6 charactors.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Curreny Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Confirm password and New password do not match")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Not a valid Email address")]
        [StringLength(100)]
        public string Email { get; set; }
    }

    public class ProfileSettingsViewModel
    {
        [Display(Name = "User name")]
        [StringLength(100)]
        public string UserName { get; set; }

        [Display(Name = "Nick name")]
        [StringLength(100)]
        public string NickName { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Confirm password and New password do not match")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email address")]
        [StringLength(100)]
        public string Email { get; set; }


        /// <summary>
        /// 手机
        /// </summary>
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }


        [Display(Name = "Address")]
        public string Address { get; set; }
    }

    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "{0}can not be empty")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "User name")]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0}can not be empty")]
        [StringLength(100, ErrorMessage = "{0}长度至少{2}为6个字符", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessage = "{0}can not be empty")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email address")]
        [StringLength(100)]
        public string Email { get; set; }

        /// <summary>
        /// 入职时间
        /// </summary>
        [Required(ErrorMessage = "{0}can not be empty")]
        public DateTime CreateOn { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [Required(ErrorMessage = "{0}can not be empty")]
        [StringLength(11)]
        [Display(Name = "Mobile number")]
        public string Mobile { get; set; }

        /// <summary>
        /// 角色，1普管用户，2？？用户（未启用），3超管用户
        /// </summary>
        [Required(ErrorMessage = "{0}can not be empty")]
        public int RoleId { get; set; }
    }

}
