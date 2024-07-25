using MyEcommerceAdmin.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyEcommerceAdmin.SingletonPattern
{
    public class AdminLoginService
    {
        private static AdminLoginService instance;
        private static readonly object lockObject = new object();

        private AdminLoginService() { }

        public static AdminLoginService GetInstance()
        {
            lock (lockObject)
            {
                if (instance == null)
                {
                    instance = new AdminLoginService();
                }
                return instance;
            }
        }

        public admin_Login ValidateUser(MyEcommerceDbContext db, string userName, string password)
        {
            string query = "SELECT * FROM admin_Login WHERE UserName = @UserName AND Password = @Password";
            var parameters = new SqlParameter[]
            {
            new SqlParameter("@UserName", userName),
            new SqlParameter("@Password", password)
            };

            return db.Database.SqlQuery<admin_Login>(query, parameters).FirstOrDefault();
        }
    }

}