using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Caching;
using prjBusinessLogic;
using prjInfo;

namespace prjComponents
{
    public static class MenuCache
    {
        public const string CurrentUserSessionKey = "ATFM_CURRENT_USER";
        private const string CacheKeyPrefix = "ATFM_MENU_ROWS_CACHE_";
        private const string RoleCacheKeyPrefix = "ATFM_MENU_ROLES_CACHE_";
        private static readonly TimeSpan CacheLifetime = TimeSpan.FromDays(1);
        private static readonly ConcurrentDictionary<int, object> UserSyncRoots =
            new ConcurrentDictionary<int, object>();

        private sealed class RoleCacheBucket
        {
            public readonly object SyncRoot = new object();
            public readonly Dictionary<int, T_RolePermission> Roles =
                new Dictionary<int, T_RolePermission>();
        }

        public static T_Users ResolveCurrentUser(UserDAL userDAL)
        {
            if (userDAL == null)
                throw new ArgumentNullException("userDAL");

            HttpContext context = HttpContext.Current;
            if (context == null
                || context.User == null
                || context.User.Identity == null
                || !context.User.Identity.IsAuthenticated
                || string.IsNullOrWhiteSpace(context.User.Identity.Name))
                return null;

            string userName = context.User.Identity.Name;
            T_Users user = context.Session == null
                ? null
                : context.Session[CurrentUserSessionKey] as T_Users;

            if (user != null
                && !string.Equals(user.UserName, userName, StringComparison.OrdinalIgnoreCase))
            {
                context.Session.Remove(CurrentUserSessionKey);
                user = null;
            }

            if (user == null)
            {
                user = userDAL.GetUserByUserName(userName);
                if (user != null && context.Session != null)
                    context.Session[CurrentUserSessionKey] = user;
            }

            return user;
        }

        public static DataTable Get(int userID)
        {
            if (userID <= 0)
                return null;

            string cacheKey = GetCacheKey(userID);
            DataTable rows = HttpRuntime.Cache[cacheKey] as DataTable;
            if (IsValid(rows))
                return rows;

            if (rows != null)
                HttpRuntime.Cache.Remove(cacheKey);

            return null;
        }

        public static DataTable GetOrLoad(int userID, UserDAL userDAL)
        {
            if (userID <= 0)
                throw new ArgumentOutOfRangeException("userID");
            if (userDAL == null)
                throw new ArgumentNullException("userDAL");

            DataTable rows = Get(userID);
            if (rows != null)
                return rows;

            object userSyncRoot = UserSyncRoots.GetOrAdd(userID, id => new object());
            lock (userSyncRoot)
            {
                rows = Get(userID);
                if (rows != null)
                    return rows;

                rows = userDAL.GetAllMenu4User(userID);
                if (!IsValid(rows))
                    throw new InvalidOperationException("GetAllMenu4User returned invalid data.");

                DataTable cachedRows = rows.Copy();
                HttpRuntime.Cache.Insert(
                    GetCacheKey(userID),
                    cachedRows,
                    null,
                    DateTime.UtcNow.Add(CacheLifetime),
                    Cache.NoSlidingExpiration);
                return cachedRows;
            }
        }

        public static T_RolePermission GetRoleOrLoad(
            int userID,
            int menuID,
            UserDAL userDAL)
        {
            if (userID <= 0)
                throw new ArgumentOutOfRangeException("userID");
            if (menuID <= 0)
                throw new ArgumentOutOfRangeException("menuID");
            if (userDAL == null)
                throw new ArgumentNullException("userDAL");

            string cacheKey = GetRoleCacheKey(userID);
            RoleCacheBucket bucket = HttpRuntime.Cache[cacheKey] as RoleCacheBucket;
            if (bucket == null)
            {
                object userSyncRoot = UserSyncRoots.GetOrAdd(userID, id => new object());
                lock (userSyncRoot)
                {
                    bucket = HttpRuntime.Cache[cacheKey] as RoleCacheBucket;
                    if (bucket == null)
                    {
                        bucket = new RoleCacheBucket();
                        HttpRuntime.Cache.Insert(
                            cacheKey,
                            bucket,
                            null,
                            DateTime.UtcNow.Add(CacheLifetime),
                            Cache.NoSlidingExpiration);
                    }
                }
            }

            lock (bucket.SyncRoot)
            {
                T_RolePermission role;
                if (!bucket.Roles.TryGetValue(menuID, out role))
                {
                    role = userDAL.GetRole4UserMenu(userID, menuID);
                    if (role == null)
                        throw new InvalidOperationException("GetRole4UserMenu returned no data.");
                    bucket.Roles[menuID] = role;
                }

                return role;
            }
        }

        public static bool ContainsMenu(DataTable rows, int menuID)
        {
            if (!IsValid(rows) || menuID <= 0)
                return false;

            foreach (DataRow row in rows.Rows)
            {
                if (CommonLib.CheckNullInt(row["ID"]) == menuID)
                    return true;
            }

            return false;
        }

        public static bool TryGetMenuNames(
            DataTable rows,
            int menuID,
            out string menuName,
            out string parentName)
        {
            menuName = string.Empty;
            parentName = string.Empty;
            if (!IsValid(rows))
                return false;

            int parentID = 0;
            foreach (DataRow row in rows.Rows)
            {
                if (CommonLib.CheckNullInt(row["ID"]) != menuID)
                    continue;

                menuName = CommonLib.CheckNullStr(row["MENUNAME"]);
                parentID = CommonLib.CheckNullInt(row["PARRENTID"]);
                break;
            }

            if (string.IsNullOrWhiteSpace(menuName))
                return false;

            if (parentID > 0)
            {
                foreach (DataRow row in rows.Rows)
                {
                    if (CommonLib.CheckNullInt(row["ID"]) == parentID)
                    {
                        parentName = CommonLib.CheckNullStr(row["MENUNAME"]);
                        break;
                    }
                }
            }

            return true;
        }

        public static void Remove(int userID)
        {
            if (userID > 0)
            {
                HttpRuntime.Cache.Remove(GetCacheKey(userID));
                HttpRuntime.Cache.Remove(GetRoleCacheKey(userID));
            }
        }

        private static string GetCacheKey(int userID)
        {
            return CacheKeyPrefix + userID;
        }

        private static string GetRoleCacheKey(int userID)
        {
            return RoleCacheKeyPrefix + userID;
        }

        private static bool IsValid(DataTable rows)
        {
            return rows != null
                && rows.Columns.Contains("ID")
                && rows.Columns.Contains("MENUNAME")
                && rows.Columns.Contains("MENUORDER")
                && rows.Columns.Contains("PARRENTID")
                && rows.Columns.Contains("MENUURL")
                && rows.Columns.Contains("MENUICON");
        }
    }
}
