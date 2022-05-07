using QuanLyQuanCaPhe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCaPhe.DAO
{
    class TableDAO
    {
        private static TableDAO instance;

        public static TableDAO Instance
        {
            get { if (instance == null) instance = new TableDAO(); return TableDAO.instance; }
            private set { TableDAO.instance = value; }
        }

        public static int TableWidth = 90;
        public static int TableHeight = 90; 

        private TableDAO () {}

        public List<Table> LoadTableList()
        {
            List<Table> TableList = new List<Table>();

            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetTableList");

            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                TableList.Add(table); 
            }

            return TableList; 

        }

        // Lấy thông tin của  table từ datasoure
        public List<Table> GetListTable()
        {
            List<Table> list = new List<Table>();

            string query = "select * from dbo.tablefood";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Table tablelist = new Table(item);

                list.Add(tablelist);
            }

            return list;
        }

        // các chức năng  trong table
        public bool InsertTable( string name, string status)
        {
            string query = string.Format("insert into dbo.tablefood(name, status ) values (  N'{0}', N'{1}' )",name, status);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool UpdateTable(int idTable,  string name, string status)
        {
            string query = string.Format("update dbo.tablefood set name = N'{0}', status = N'{1}' where id = {2} ", name, status, idTable);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool DeleteTable(int idTable)
        {
            FoodDAO.Instance.DeleteFoodInfoByCategoryID(idTable);
            string query = string.Format("Delete tablefood where id = {0}", idTable);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }


    }
}
