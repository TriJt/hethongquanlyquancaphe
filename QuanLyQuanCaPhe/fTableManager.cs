using QuanLyQuanCaPhe.DAO;
using QuanLyQuanCaPhe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace QuanLyQuanCaPhe
{
    public partial class fTableManager : Form
    {
        private Account loginAccount;   
        public Account LoginAccount {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount.Type); } } 
         
        public fTableManager(Account acc)
        {
            InitializeComponent();

            this.LoginAccount = acc;

            LoadTable();
            LoadCategory(); 
            
        }

        #region Method 

        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinTàiKhoảnToolStripMenuItem.Text += "(" + LoginAccount.DisplayName + ")"; 
        }

        // Load loại thức ăn 
        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategori();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "name"; 
        }
        // Load thức ăn có trong loại 
        void LoadFoodListByCategoryID(int id)
        {

            List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryByID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "name";
        }


        void LoadTable()
        {
            flpTable.Controls.Clear(); 
           List<Table>  tableList =  TableDAO.Instance.LoadTableList(); 

            foreach (Table item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight};

                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Click += btn_Click;
                btn.Tag = item;
                switch (item.Status )
                {
                    case   "Có người" :
                        btn.BackColor = Color.Green; 
                        break;
                    default:
                        btn.BackColor = Color.Red; 
                        break;
                   
                }

                flpTable.Controls.Add(btn); 

            }

        }
        void ShowBill( int id )

        {
            lsvBill.Items.Clear();
            List<QuanLyQuanCaPhe.DTO.Menu1> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            double totalPrice = 0; 
            foreach(QuanLyQuanCaPhe.DTO.Menu1 item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice; 
                lsvBill.Items.Add(lsvItem);
            }
            
            txbTotalPrice.Text = totalPrice.ToString();

             
        } 

        #endregion

        #region Events 

         void btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag; 

            ShowBill(tableID);

        }
        

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile( LoginAccount);
            f.UpdateAccount += f_UpdateAccount;  
            f.ShowDialog(); 
        }

        void f_UpdateAccount(object sender, AccountEvent e)
        {
            thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản (" + e.Acc.DisplayName + ")"; 
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {

            fAdmin f = new fAdmin();
            f.loginAccount = LoginAccount; 
            f.InsertFood+= f_InsertFood;
            f.DeleteFood += f_DeleteFood;
            f.UpdateFood += f_UpdateFood;
            f.ShowDialog(); 

        }

         void f_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        void f_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadTable(); 
        }

        void f_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if(lsvBill.Tag !=null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            Category selected = cb.SelectedItem as Category;
            id = selected.ID;  
            LoadFoodListByCategoryID(id); 
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if(table == null)
            {
                MessageBox.Show("Hãy chọn bàn cần thêm món");
                return; 
            }

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int foodID = (cbFood.SelectedItem as Food).ID;
            int count = (int)nmFoodCount.Value; 

            if(idBill == -1)
            {
                BillDAO.Instance.InsertBill(table.ID);
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(),foodID,count); 
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);

            }
            ShowBill(table.ID);
            LoadTable();

        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int discount = (int)nmDiscount.Value;
            double totalPrice = Convert.ToDouble(txbTotalPrice.Text) ;
            double finalTotalPrice = totalPrice - (totalPrice/100) * discount;
            if(idBill != -1)
            {
                if(MessageBox.Show(string.Format("Bạn có chắc thanh toán hóa đơn cho bàn {0} \n  Tổng tiền - (Tổng tiền/100) x Giảm giá = {1} - ({1}/100) x {2} = {3}  ",table.Name,totalPrice,discount,finalTotalPrice),"Thông báo",MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)

                {
                    BillDAO.Instance.Checkout(idBill,discount,(float)finalTotalPrice);
                    ShowBill(table.ID);
                    LoadTable();
                }
            }

        }
        private void thêmMónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnAddFood_Click(this, new EventArgs());
        }

        private void thanhToánToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnCheckOut_Click(this, new EventArgs());
        }

        #endregion


    }
}
