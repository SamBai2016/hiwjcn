﻿using Dal;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLogic.Model.User;

namespace WebLogic.Dal.User
{
    /// <summary>
    /// 角色
    /// </summary>
    public class RoleDal : EFRepository<RoleModel>
    {
        public RoleDal() { }
    }
}
