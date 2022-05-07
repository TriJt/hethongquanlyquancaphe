using QuanLyQuanCaPhe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCaPhe.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance; 
        
        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO();  return instance; }
            private set { instance = value; }
        }
        private AccountDAO()
        {

        }

        public bool Login(string userName, string passWord)
        {
            string query = "USP_Login @username , @password";
            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[]{userName, passWord});
            return result.Rows.Count > 0;

        }
        public bool UpdateAccount(string userName, string displayName, string password, string newpass)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("exec USP_UpdateAccount  @userName , @displayName  , @password , @newPassword ", new object[]{userName,displayName, password,newpass});

            return result > 0; 
        }
        public Account GetAccountByUserName(string userName)
        {
           DataTable data =  DataProvider.Instance.ExecuteQuery("select * from account where userName = '" + userName +"'");

            foreach (DataRow item in data.Rows)
            {
                return new Account(item); 

            }
            return null; 
        }
        public DataTable GetListAccount()
        {
            return DataProvider.Instance.ExecuteQuery("select userName, displayName, type from dbo.account"); 
        }
        // các chức năng  trong Account
        public bool InsertAccount(string userName, string displayName, int type)
        {
            string query = string.Format("insert into dbo.account(userName,displayName, type ) values (  N'{0}', N'{1}', {2} )", userName, displayName, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool UpdateAccount(string userName, string displayName, int type)
        {
            string query = string.Format("update dbo.account set  displayName = N'{0}', type = {1} where userName = N'{2}' ",displayName, type, userName);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool DeleteAccount(string username)
        {
           
            string query = string.Format("Delete account where userName = N'{0}'", username);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool ResetPassword(string username)
        {
            string query = string.Format("update account set password = N'0' where userName = N'{0}'", username);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }

   
}
