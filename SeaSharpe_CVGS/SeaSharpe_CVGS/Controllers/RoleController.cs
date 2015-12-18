using SeaSharpe_CVGS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace SeaSharpe_CVGS.Controllers
{
    /// <summary>
    /// This class is responsible for determining what users are in what roles
    /// </summary>
    public class RoleController : RoleProvider
    {
        private ApplicationDbContext _dbContext;

        public ApplicationDbContext DbContext
        {
            get
            {
                return _dbContext ?? new ApplicationDbContext();
            }

            protected set
            {
                _dbContext = value;
            }
        }

        enum Roles
        {
            None, Member, User, Employee
        }

        /// <summary>
        /// Not Implemented but override is required
        /// </summary>
        /// <param name="usernames"></param>
        /// <param name="roleNames"></param>
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the application name
        /// </summary>
        public override string ApplicationName
        {
            get
            {
                return typeof(RoleController).FullName;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Not Implemented but override is required
        /// </summary>
        /// <param name="roleName"></param>
        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not Implemented but override is required
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="throwOnPopulatedRole"></param>
        /// <returns>NotImplementedException</returns>
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not Implemented but override is required
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="usernameToMatch"></param>
        /// <returns>NotImplementedException</returns>
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all roles
        /// </summary>
        /// <returns>A string array containing all rolenames</returns>
        public override string[] GetAllRoles()
        {
            return Enum.GetNames(typeof(Roles));
        }

        /// <summary>
        /// Get all roles for a user
        /// </summary>
        /// <param name="username">The user in question</param>
        /// <returns>A string array containing all rolenames</returns>
        public override string[] GetRolesForUser(string username)
        {
            var roles = new System.Collections.Generic.List<string>();
            if (DbContext.Members.Any(m => m.User.UserName == username)) 
                roles.Add(Enum.GetName(typeof(Roles), Roles.Member));
            if (DbContext.Employees.Any(e => e.User.UserName == username)) 
                roles.Add(Enum.GetName(typeof(Roles), Roles.Employee));
            if (DbContext.Users.Any(u => u.UserName == username)) 
                roles.Add(Enum.GetName(typeof(Roles), Roles.User));
            return roles.ToArray();
        }

        /// <summary>
        /// Get all users for a role
        /// </summary>
        /// <param name="roleName">The name of the role</param>
        /// <returns>A string array with all users in the role</returns>
        public override string[] GetUsersInRole(string roleName)
        {
            Roles role = Roles.None;
            Enum.TryParse<Roles>(roleName, out role);
            switch (role)
            {
                case Roles.Member:
                    return DbContext.Members.Select(m => m.User.UserName).ToArray();
                case Roles.User:
                    return DbContext.Users.Select(u => u.UserName).ToArray();
                case Roles.Employee:
                    return DbContext.Employees.Select(e => e.User.UserName).ToArray();
                default:
                    return new string[0];
            }
        }

        /// <summary>
        /// Find out if a user is in a role
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="roleName">The role name</param>
        /// <returns>true if the user is in the role</returns>
        public override bool IsUserInRole(string username, string roleName)
        {
            return GetUsersInRole(roleName).Contains(username);
        }

        /// <summary>
        /// Not Implemented but override is required
        /// </summary>
        /// <param name="usernames"></param>
        /// <param name="roleNames"></param>
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check if a role exists
        /// </summary>
        /// <param name="roleName">the role</param>
        /// <returns>true when roleName does exist</returns>
        public override bool RoleExists(string roleName)
        {
            foreach (string role in Enum.GetNames(typeof(Roles)))
            {
                if (role.ToLower() == roleName.ToLower()) return true;
            }
            return false;
        }
    }
}