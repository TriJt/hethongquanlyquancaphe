using QuanLyQuanCaPhe.DAO;
using QuanLyQuanCaPhe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCaPhe
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource CategoryList = new BindingSource();
        BindingSource TableList = new BindingSource();
        BindingSource AccountList = new BindingSource();
        public Account loginAccount; 

        public fAdmin()
        {
            InitializeComponent();
            Load(); 
        }

        #region methods 
        void Load()
        {
            dtgvFood.DataSource = foodList;
            dtgvCategory.DataSource = CategoryList;
            dtgvTable.DataSource = TableList;
            dtgvAccount.DataSource = AccountList; 
            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadListFood();
            LoadListCategory();
            LoadListTable();
            LoadAccount();
            AddAccounBingding(); 
            AddCategoryBinding();
            AddFoodBinding();
            AddTableBinding();
            LoadCategoryIntoCombobox(cbFoodCategory); 
        }

        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkToDate.Value.AddMonths(1).AddDays(-1); 
        }

        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
       {
          dtgvBill.DataSource =  BillDAO.Instance.GetBillListByDate(checkIn, checkOut); 
       } 
        void AddCategoryBinding()
        {
            txbNameCategory.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbIDCategory.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "ID", true, DataSourceUpdateMode.Never));
            
        }
        void AddAccounBingding()
        {
            txbUsernameAccount.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "userName", true, DataSourceUpdateMode.Never));
            txbNameAccount.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "displayName", true, DataSourceUpdateMode.Never));
            nmudTypeAccount.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "type", true, DataSourceUpdateMode.Never));

        }
        void AddFoodBinding()
        {
            txbNameFood.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value",dtgvFood.DataSource,"Price", true, DataSourceUpdateMode.Never)); 
        }
        void AddTableBinding()
        {
            txbNameTable.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbIDTable.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txbStatusTable.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "status", true, DataSourceUpdateMode.Never));
        }
        void LoadAccount()
        {
            AccountList.DataSource = AccountDAO.Instance.GetListAccount(); 
        }
        void LoadCategoryIntoCombobox( ComboBox cb)
        {
            cbFoodCategory.DataSource = CategoryDAO.Instance.GetListCategori();
            cbFoodCategory.DisplayMember = "Name";
        }
        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood(); 
        }
        void LoadListCategory()
        {
            CategoryList.DataSource = CategoryDAO.Instance.GetListCategori(); 
        }
        void LoadListTable()
        {
            TableList.DataSource = TableDAO.Instance.GetListTable(); 
        }
        List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name); 
            return listFood; 
        }
        void AddAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.InsertAccount(userName, displayName, type)) {
                MessageBox.Show("Thêm tài khoản thành công"); 
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại ");
            }
            LoadAccount(); 
        }
        void EditAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại ");
            }
            LoadAccount();
        }
        void DeleteAccount(string userName)
        {
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show(" Vui lòng đừng xóa tài khoản của chính bạn !!!");
                return; 
            }
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại ");
            }
            LoadAccount();
        }
        void ResetPassword(string userName)
        {
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu tài khoản thất bại ");
            }
            LoadAccount();
        }
        #endregion

        #region events 

        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value); 
        }

        private void btnViewFood_Click(object sender, EventArgs e)
        {
            LoadListFood(); 
        }

        private void txbID_TextChanged(object sender, EventArgs e)
        {
            try { 
            if(dtgvFood.SelectedCells.Count > 0)
            {
                int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;

                Category cateogory = CategoryDAO.Instance.GetCategoryByID(id);

                cbFoodCategory.SelectedItem = cateogory;

                int index = -1;
                int i = 0; 
                foreach(Category item in cbFoodCategory.Items)
                {
                    if(item.ID  == cateogory.ID)
                    {
                        index = i;
                        break; 
                    }
                    i++;
                }
                cbFoodCategory.SelectedIndex = index; 
            }
            }
            catch
            {

            }

        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txbNameFood.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price =(float)nmFoodPrice.Value; 

            if(FoodDAO.Instance.InsertFood(name,categoryID, price))
            {
                MessageBox.Show("Thêm món thành công");
                LoadListFood();
                if (insertFood != null)
                {
                    insertFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm món ăn"); 
            }

        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            string name = txbNameFood.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            int idfood = Convert.ToInt32(txbID.Text); 

            if (FoodDAO.Instance.UpdateFood(idfood,name, categoryID, price ))
            {
                MessageBox.Show("Sửa món thành công");
                LoadListFood();
                if (updateFood != null)
                {
                    updateFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa món ăn");
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int idfood = Convert.ToInt32(txbID.Text);

            if (FoodDAO.Instance.DeleteFood(idfood))
            {
                MessageBox.Show("Xóa món thành công");
                LoadListFood();
                if(deleteFood != null)
                {
                    deleteFood(this, new EventArgs()); 
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa món ăn");
            }
        }

        private void btnViewCategory_Click(object sender, EventArgs e)
        {
            LoadListCategory();
        }
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            int idcategory = Convert.ToInt32(txbIDCategory.Text); 
            string name = txbNameCategory.Text;
           

            if (CategoryDAO.Instance.InsertCategory(idcategory,name))
            {
                MessageBox.Show("Thêm danh mục thành công");

                LoadListCategory();
                if (insertCategory != null)
                {
                    insertCategory(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm món ăn");
            }
        }

        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            int idcategory = Convert.ToInt32(txbIDCategory.Text);
            string name = txbNameCategory.Text;


            if (CategoryDAO.Instance.UpdateCategory(idcategory, name))
            {
                MessageBox.Show("Sửa danh mục thành công");

                LoadListCategory();
                if (updateCategory != null)
                {
                    updateCategory(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa món ăn");
            }
        }
        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            int idcategory = Convert.ToInt32(txbID.Text);

            if (CategoryDAO.Instance.DeleteCategory(idcategory))
            {
                MessageBox.Show("Xóa danh mục thành công");
                LoadListFood();
                if (deleteCategory != null)
                {
                    deleteCategory(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa danh mục");
            }
        }
        private void btnViewTable_Click(object sender, EventArgs e)
        {
            LoadListTable();
        }
        private void btnEditTable_Click(object sender, EventArgs e)
        {
            int idTable = Convert.ToInt32(txbIDTable.Text); 
            string name = txbNameTable.Text;
            string status = txbStatusTable.Text;

            if (TableDAO.Instance.UpdateTable(idTable, name, status))
            {
                MessageBox.Show("Sửa bàn thành công");

                LoadListTable();
                if (updateTable != null)
                {
                    updateTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm món ăn");
            }
        }

        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            int idTable = Convert.ToInt32(txbIDTable.Text);
          

            if (TableDAO.Instance.DeleteTable(idTable))
            {
                MessageBox.Show("Xóa bàn thành công");

                LoadListTable();
                if (deleteTable != null)
                {
                    deleteTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm món ăn");
            }
        
    }

        private void btnAddTable_Click(object sender, EventArgs e)
        {
            
            string name = txbNameTable.Text;
            string status = txbStatusTable.Text; 

            if (TableDAO.Instance.InsertTable( name,status))
            {
                MessageBox.Show("Thêm bàn thành công");

                LoadListTable();  
                if (insertTable != null)
                {
                    insertTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm bàn");
            }
        }
        private void btnFindFood_Click(object sender, EventArgs e)
        {
          foodList.DataSource =  SearchFoodByName(txbSearchFood.Text); 
        }
        private void btnViewAccount_Click(object sender, EventArgs e)
        {
            LoadAccount(); 
        }
        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUsernameAccount.Text;
            string displayName = txbNameAccount.Text;
            int type =(int)nmudTypeAccount.Value;
            AddAccount(userName, displayName, type); 
        }
        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUsernameAccount.Text;

            DeleteAccount(userName);
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUsernameAccount.Text;
            string displayName = txbNameAccount.Text;
            int type = (int)nmudTypeAccount.Value;
            EditAccount(userName, displayName, type);
            
        }
        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string userName = txbUsernameAccount.Text;

            ResetPassword(userName);
        }
        // sự kiện của food 
        private event EventHandler insertFood; 
        public event EventHandler InsertFood
        {
            add { insertFood += value;  }
            remove { insertFood -= value;  }
        }
        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }
        // sự kiện của category 
        private event EventHandler insertCategory;
        public event EventHandler InsertCategory
        {
            add { insertCategory += value; }
            remove { insertCategory -= value; }
        }
        private event EventHandler updateCategory;
        public event EventHandler UpdateCategory
        {
            add { updateCategory += value; }
            remove { updateCategory -= value; }
        }

        private event EventHandler deleteCategory;
        public event EventHandler DeleteCategory
        {
            add { deleteCategory += value; }
            remove { deleteCategory -= value; }
        }

        // sự kiện của table 
        private event EventHandler insertTable;
        public event EventHandler InsertTable
        {
            add { insertTable += value; }
            remove { insertTable -= value; }
        }
        private event EventHandler updateTable;
        public event EventHandler UpdateTable
        {
            add { updateTable += value; }
            remove { updateTable -= value; }
        }

        private event EventHandler deleteTable;
        public event EventHandler DeleteTable
        {
            add { deleteTable += value; }
            remove { deleteTable -= value; }
        }












        #endregion

        
    }
}
