using QuanLyQuanCaPhe.DAO;
using QuanLyQuanCaPhe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCaPhe
{
    public partial class fAccountProfile : Form
    {
        private Account loginAccount;
        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount); }
        }

        public fAccountProfile(Account acc)
        {
            InitializeComponent();
            LoginAccount = acc; 
        }
        void ChangeAccount(Account acc)
        {
            txbUseName.Text = LoginAccount.UserName;
            txbDisplayName.Text = LoginAccount.DisplayName;

              }
        void UpdateAcountForForm()
        {
            string displayName = txbDisplayName.Text;
            string password = txbPassWord.Text;
            string newpass = txbNewPassword.Text;
            string reenterpass = txbReEnterPass.Text;
            string userName = txbUseName.Text;

            if (!newpass.Equals(reenterpass))
            {
                MessageBox.Show("Vui lòng nhập lại mật khẩu đúng với mật khẩu mới!!"); 

            }
            else
            {
                if(AccountDAO.Instance.UpdateAccount(userName, displayName,password,newpass))
                {
                    MessageBox.Show("Cập nhật thành công!"); 
                    if(updateAccount != null)
                    {
                        updateAccount(this,new AccountEvent( AccountDAO.Instance.GetAccountByUserName(userName))); 
                    }
                }
                else
                {
                    MessageBox.Show("Sai mật khẩu!");
                }
                     
            }
        }

        private event EventHandler<AccountEvent> updateAccount; 
        public event EventHandler<AccountEvent> UpdateAccount
        {
            add { updateAccount += value;  } 
            remove { updateAccount -= value; }
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close(); 

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAcountForForm(); 
        }
    }
    public class AccountEvent : EventArgs
    {
        private Account acc;

        public Account Acc { get => acc; set => acc = value; }
        public AccountEvent(Account acc)
        {
            this.Acc = acc; 
        }
    }
}
