using GNM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNM.Service
{
    interface IUserService
    {
         /// <summary>
        /// LoginAuthentication method is used to Check UserName and Password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        User LoginAuthentication(string username, string password);

        /// <summary>
        /// ChangePassword mehtod is used to Change User Password
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        int ChangePassword(int userId, string newPassword);

        /// <summary>
        /// CheckOldPassword method is used to Check Old Password Correct Or Not
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        string CheckOldPassword(int userId);

        /// <summary>
        /// AddUser method is used to Add New User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddUser(User model);

        /// <summary>
        /// IsEmailAddressExist method is used to Check EmailAddress exist or not
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        bool IsEmailAddressExist(string emailAddress, int userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        User GetUserInformationByEmailAddress(string emailAddress);

        /// <summary>
        /// IsUserNameExist method is used to Check UserName exist or not
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        bool IsUserNameExist(string username, int userId);

         /// <summary>
        /// GetUserById method used to get user information by userid
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        User GetUserById(int userId);

        /// <summary>
        /// UpdateUser method is used to Update user information
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int UpdateUser(User model);
    }
}
