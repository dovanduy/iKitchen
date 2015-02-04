using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunTzu.Core.Data;

namespace SunTzu.Web.EntityValidate
{
    interface IEntityValidator
    {
        ValidateResult Validate(IEntity entity);
    }
}
