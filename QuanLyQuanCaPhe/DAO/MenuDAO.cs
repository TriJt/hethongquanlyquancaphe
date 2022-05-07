using QuanLyQuanCaPhe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCaPhe.DAO
{
    public class MenuDAO
    {
        private static MenuDAO instance;

        public static MenuDAO Instance
        {
            get { if (instance == null) instance = new MenuDAO(); return MenuDAO.instance; }
            private set => instance = value;
        }


        private MenuDAO()
        {
        }

        public List<Menu1> GetListMenuByTable(int id)
        {
            List<Menu1> listMenu = new List<Menu1>();
            string query = "select f.name, bi.count , f.price, f.price * bi.count as totalPrice from dbo.billInfo as bi,dbo.bill as b, dbo.food as f  where bi.idBill = b.id and bi.idFood = f.id and b.status = 0 and b.idTable = " + id;


            DataTable data = DataProvider.Instance.ExecuteQuery(query); 

            foreach(DataRow item in data.Rows)
            {
                Menu1 menu =  new Menu1(item);
                listMenu.Add(menu); 
            }


            return listMenu;
        }

        
    }
}
