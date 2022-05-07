using QuanLyQuanCaPhe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCaPhe.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instance;

        public static FoodDAO Instance
        {
            get { if (instance == null) instance = new FoodDAO(); return FoodDAO.instance; }
            set => instance = value;
        }

        private FoodDAO() { }

        public List<Food> GetFoodByCategoryByID(int id)
        {
            List<Food> list = new List<Food>();

            string query = "select * from dbo.food where idCategory = " + id; 

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food); 
            }
                return list;

        }

        public List<Food> GetListFood()
        {

            List<Food> list = new List<Food>();

            string query = "select * from dbo.food ";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);
            }
            return list;

        }
        public List<Food> SearchFoodByName(string name)
        {
            List<Food> list = new List<Food>();

            string query = string.Format("select * from dbo.food where  dbo.fuConvertToUnsign1(name)   like N'%' +  dbo.fuConvertToUnsign1(N'{0}') + '%'  ",  name);

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);
            }
            return list;
        }
        public bool InsertFood(string name, int id , float price)
        {
            string query = string.Format("insert into dbo.food(name, idCategory, price) values ( N'{0}', {1}, {2} )", name, id , price);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0; 
        }
        public bool UpdateFood(int idfood, string name, int id, float price)
        {
            string query = string.Format("update dbo.food set name = N'{0}', idCategory = {1}, price = {2} where id = {3} ", name, id, price, idfood);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool DeleteFood(int idfood)
        {
            BillInfoDAO.Instance.DeleteBillInfoByFoodID(idfood); 
            string query = string.Format("Delete food where id = {0}",idfood);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public void DeleteFoodInfoByCategoryID(int id)
        {
            DataProvider.Instance.ExecuteQuery(" delete dbo.food where idCategory = " + id);
        }

    }
}
