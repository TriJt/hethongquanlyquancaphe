using QuanLyQuanCaPhe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCaPhe.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance { get { if (instance == null) instance = new BillDAO();
                return BillDAO.instance;  }
           private   set => BillDAO.instance = value; }

        private BillDAO()
        {

        }
        //  THÀNH CÔNG : BILL ID 
        //  THẤT BẠI : -1 

        public int GetUncheckBillIDByTableID(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.bill where idTable = " + id + "and status = 0");
            
            if(data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID1;
            }
            return -1; 
        }

        public void Checkout(int id, int discount, float totalPrice)
        {
            string query = "update dbo.bill set dateCheckout = GETDATE(),  status = 1, "+ "discount = " + discount + ",totalPrice = " + totalPrice +  "where id =  " + id;
            DataProvider.Instance.ExecuteNonQuery(query); 

        }

        public DataTable GetBillListByDate(DateTime checkIn, DateTime checkOut)
        {
          return  DataProvider.Instance.ExecuteQuery("exec USP_GetListBillByDate @checkIn , @checkOut", new object[] {checkIn, checkOut });

        } 

        public void InsertBill( int id)
        {
            DataProvider.Instance.ExecuteNonQuery("exec USP_InsertBill @idTable", new object[] { id });
        }

        public int GetMaxIDBill()
        { 
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar("Select max(id) from dbo.Bill");

            }
            catch
            {
                return 1; 
            }
        }

    }
}
