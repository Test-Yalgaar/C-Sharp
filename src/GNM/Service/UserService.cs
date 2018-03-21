using GNM.Models;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace GNM.Service
{
    public class UserService : IUserService
    {
        NpgsqlConnection conn = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["GNMConnection"].ConnectionString);

        /// <summary>
        /// LoginAuthentication method is used to Check UserName and Password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User LoginAuthentication(string username, string password)
        {
            try
            {
                User model = null;
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                
                NpgsqlDataReader dr;
                cmd.CommandText = "SELECT * FROM user_mst where username=@username and password =@password";
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@username",NpgsqlDbType.Text ,username);
                cmd.Parameters.AddWithValue("@password",NpgsqlDbType.Text ,password);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    model = new User();
                    model.UserId = dr.GetInt32(dr.GetOrdinal("userid"));
                    model.UserName = dr["username"].ToString();
                    model.Name = dr["name"].ToString();
                }
                conn.Close();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ChangePassword mehtod is used to Change User Password
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public int ChangePassword(int userId,string newPassword)
        {
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            IFormatProvider culture = new CultureInfo("en-US", true);
            cmd.CommandText = "update user_mst set password=@newPassword , password_change_on=@changeOn where userid=@userId";
            cmd.Connection = conn;
            cmd.Parameters.AddWithValue("@newPassword", NpgsqlDbType.Text, newPassword);
            cmd.Parameters.AddWithValue("@userId", NpgsqlDbType.Integer, userId);
            cmd.Parameters.AddWithValue("@changeOn", NpgsqlDbType.Timestamp, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", culture));
            int i = cmd.ExecuteNonQuery();
            conn.Close();
            return i;
        }

        /// <summary>
        /// CheckOldPassword method is used to Check Old Password Correct Or Not
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string CheckOldPassword(int userId)
        {
            conn.Open();
            string pass = "";
            NpgsqlCommand cmd = new NpgsqlCommand();
            NpgsqlDataReader dr;
            cmd.CommandText = "SELECT * FROM user_mst where userid=@userId";
            cmd.Connection = conn;
            cmd.Parameters.AddWithValue("@userId", NpgsqlDbType.Integer, userId);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                 pass = dr["password"].ToString();
            }
            conn.Close();
            return pass;
        }

        /// <summary>
        /// AddUser method is used to Add New User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddUser(User model)
        {
            try
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.CommandText = "Insert into user_mst(name,username,password,email_address,contact_no) values(@name,@userName,@password,@email,@contactNo)";
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@name", NpgsqlDbType.Text, model.Name);
                cmd.Parameters.AddWithValue("@userName", NpgsqlDbType.Text, model.UserName);
                cmd.Parameters.AddWithValue("@password", NpgsqlDbType.Text,model.Password);
                cmd.Parameters.AddWithValue("@email", NpgsqlDbType.Text,model.EmailAddress);
                cmd.Parameters.AddWithValue("@contactNo", NpgsqlDbType.Text, model.ContactNo);
                int i = cmd.ExecuteNonQuery();
                conn.Close();
                return i;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// UpdateUser method is used to Update user information
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateUser(User model)
        {
            try
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.CommandText = "update user_mst set name=@name,email_address=@email,contact_no=@contactNo where userid=@userId";
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@name", NpgsqlDbType.Text, model.Name);
                cmd.Parameters.AddWithValue("@userId", NpgsqlDbType.Integer, model.UserId);
                cmd.Parameters.AddWithValue("@email", NpgsqlDbType.Text, model.EmailAddress);
                cmd.Parameters.AddWithValue("@contactNo", NpgsqlDbType.Text, model.ContactNo);
                int i = cmd.ExecuteNonQuery();
                conn.Close();
                return i;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// IsEmailAddressExist method is used to Check EmailAddress exist or not
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool IsEmailAddressExist(string emailAddress,int userId)
        {
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            NpgsqlDataReader dr;
            cmd.CommandText = "SELECT * FROM user_mst where email_address=@email and userid!=@userId";
            cmd.Connection = conn;
            cmd.Parameters.AddWithValue("@userId", NpgsqlDbType.Integer,userId);
            cmd.Parameters.AddWithValue("@email", NpgsqlDbType.Text, emailAddress);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                conn.Close();
                return true;
            }
            else
            {
                conn.Close();
                return false;
            }
        }

        /// <summary>
        /// IsUserNameExist method is used to Check UserName exist or not
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool IsUserNameExist(string username, int userId)
        {
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            NpgsqlDataReader dr;
            cmd.CommandText = "SELECT * FROM user_mst where Upper(username)=Upper(@userName) and userid!=@userId";
            cmd.Connection = conn;
            cmd.Parameters.AddWithValue("@userId", NpgsqlDbType.Integer, userId);
            cmd.Parameters.AddWithValue("@userName", NpgsqlDbType.Text, username);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                conn.Close();
                return true;
            }
            else
            {
                conn.Close();
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public User GetUserInformationByEmailAddress(string emailAddress)
        {
            conn.Open();
            User model=null;
            NpgsqlCommand cmd = new NpgsqlCommand();
            NpgsqlDataReader dr;
            cmd.CommandText = "SELECT * FROM user_mst where email_address=@email";
            cmd.Connection = conn;
            cmd.Parameters.AddWithValue("@email", NpgsqlDbType.Text, emailAddress);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                model = new User();
                model.EmailAddress = dr["email_address"].ToString();
                model.UserId = Convert.ToInt32(dr["userid"].ToString());
                model.Password = dr["password"].ToString();
            }
            conn.Close();
            return model;
        }

        /// <summary>
        /// GetUserById method used to get user information by userid
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetUserById(int userId)
        {
            conn.Open();
            User model = null;
            NpgsqlCommand cmd = new NpgsqlCommand();
            NpgsqlDataReader dr;
            cmd.CommandText = "SELECT * FROM user_mst where userid=@userId";
            cmd.Connection = conn;
            cmd.Parameters.AddWithValue("@userId", NpgsqlDbType.Integer, userId);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                model = new User();
                model.EmailAddress = dr["email_address"].ToString();
                model.UserId = Convert.ToInt32(dr["userid"].ToString());
                model.UserName = dr["username"].ToString();
                model.Name = dr["name"].ToString();
                model.ContactNo = dr["contact_no"].ToString();
                model.Password = dr["password"].ToString();
            }
            conn.Close();
            return model;
        }
    }
}