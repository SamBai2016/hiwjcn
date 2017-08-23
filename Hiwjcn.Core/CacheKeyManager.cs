﻿using Lib.cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiwjcn.Core
{
    public static class CacheKeyManager
    {
        public static string AuthTokenKey(string token_uid) => $"auth.token.uid={token_uid}".WithCacheKeyPrefix();

        public static string AuthClientKey(string client_uid) => $"auth.client.uid={client_uid}".WithCacheKeyPrefix();

        public static string AuthScopeKey(string scope_uid) => $"auth.scope.uid={scope_uid}".WithCacheKeyPrefix();

        public static string AuthScopeAllKey() => "auth.scope.all".WithCacheKeyPrefix();

        public static string AuthUserInfoKey(string user_uid) => $"auth.user.uid={user_uid}".WithCacheKeyPrefix();

        public static string SysPageKey(string page_name) => $"sys.page.name={page_name}".WithCacheKeyPrefix();

        public static string SysCategoryListKey(string category_type) =>
            $"sys.category.list.type={category_type}".WithCacheKeyPrefix();

        public static string SysLinkListKey(string link_type) =>
            $"sys.link.list.type={link_type}".WithCacheKeyPrefix();

        public static string SysOptionListKey() => $"sys.option.list.all".WithCacheKeyPrefix();
        
    }
}
