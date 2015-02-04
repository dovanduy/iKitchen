using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunTzu.Core.Authorization
{
    public class DummyAuthorization : IAuthorization
    {
        public bool TrueOrFalse
        {
            get;
            set;
        }

        public DummyAuthorization()
        {
            // true for default;
            TrueOrFalse = true;
        }
        
        #region IAuthorization Members

        public bool Login(string username, string password)
        {
            return TrueOrFalse;
        }

        public bool IsLogin
        {
            get { return TrueOrFalse; }
        }

        public bool IsAdmin
        {
            get { return TrueOrFalse; }
        }

        public bool HasAuthority(string authorCode)
        {
            return TrueOrFalse;
        }

        public bool HasAuthority(int authorCode)
        {
            return TrueOrFalse;
        }

        public void Logout()
        {
            TrueOrFalse = false;
        }

        public int CurrentUserId
        {
            get { return 1; }
        }

        public string CurrentUserName
        {
            get { return "dummyuser"; }
        }

        public bool IsSuperAdmin
        {
            get { return false; }
        }
        
        public void Refresh()
        {
        }

        public string CurrentLoginUserId
        {
            get { return ""; }
        }

        #endregion

    }
}
