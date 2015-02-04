using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunTzu.Core.Authorization
{
    public interface IAuthorization
    {
        [Obsolete("Use ASP.NET Idenity instead")]
        bool Login(string username, string password);
        bool IsLogin { get; }
        bool IsAdmin { get; }
        bool IsSuperAdmin { get; }
        bool HasAuthority(string authorCode);
        bool HasAuthority(int requiredRole);
        void Logout();
        void Refresh();
        [Obsolete("Use CurrentLoginUserId instead, as ASP.NET Idenity required")]
        int CurrentUserId { get; }
        string CurrentLoginUserId { get; }
        string CurrentUserName { get; }

    }
}
