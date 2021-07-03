using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Authorization
{
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        const string POLICY_PREFIX = "Permission";
        private string _permission;

        public PermissionAuthorizeAttribute(string permission) => Permission = permission;

        public string Permission
        {
            get {
                return _permission;
            }
            set
            {
                _permission = value;
                Policy = $"{POLICY_PREFIX}:{_permission}";
            }
        }
    }
}
