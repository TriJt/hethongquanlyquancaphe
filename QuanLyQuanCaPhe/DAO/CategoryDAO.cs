using QuanLyQuanCaPhe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCaPhe.DAO
{
    public class CategoryDAO
    {
        private static CategoryDAO instance;

        public static CategoryDAO Instance { get { if (instance == null) instance = new CategoryDAO();  return CategoryDAO.instance ; }
            set => instance = value; }

        private CategoryDAO() {  }
        // lấy dữ liệu cảu category 
        public List<Category> GetListCategori()
        {
            List<Category> list = new List<Category>();

            string query = "select * from dbo.foodcategory";

            DataTable data = DataProvider.Instance.ExecuteQuery(query); 

            foreach(DataRow item in data.Rows)
            {
                Category category = new Category(item);

                list.Add(category); 
            }

            return list; 
        }

        // lấy thông tin category theo id
        public Category GetCategoryByID( int id )
        {
            Category category = null;
            string query = "select * from dbo.foodcategory where id =" + id ;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                 category = new Category(item);

                return category; 
            }


            return category;
        }

        public bool InsertCategory(int idCategory , string name)
        {
            string query = string.Format("insert into dbo.foodcategory(id,name ) values ({0},  N'{1}' )", idCategory, name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool UpdateCategory(int idcategory, string name)
        {
            string query = string.Format("update dbo.foodcategory set id = {0}, name = N'{1}' where id = {2} ",idcategory, name, idcategory);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool DeleteCategory(int idcategory)
        {
            FoodDAO.Instance.DeleteFoodInfoByCategoryID(idcategory);
            string query = string.Format("Delete foodcategory where id = {0}", idcategory);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
