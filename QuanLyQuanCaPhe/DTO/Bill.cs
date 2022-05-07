using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data; 
using System.Threading.Tasks;

namespace QuanLyQuanCaPhe.DTO
{
   public class Bill
    {
        public Bill( int id, DateTime? datecheckin, DateTime? datecheckout, int status, int discount = 0)
        {
            this.ID = id;
            this.DateCheckIn = datecheckin;
            this.DateCheckout = datecheckout;
            this.Status = status;
            this.Discount = discount;
        }
        public Bill(DataRow row)
        {
            this.ID =  (int)row["id"];
            this.DateCheckIn =(DateTime?)row["datecheckin"];
            var dateCheckoutTemp = row["datecheckout"]; 
            if(dateCheckoutTemp.ToString() != "" )
                    this.DateCheckout = (DateTime?)dateCheckoutTemp;
            this.Status = (int)row["status"];
            this.Discount = (int)row["discount"];
        }
        private int discount; 
        private int status; 
        private DateTime? dateCheckout;
        private DateTime? dateCheckIn;
        private int ID;

        public int ID1 { get => ID; set => ID = value; }
        public DateTime? DateCheckout { get => dateCheckout; set => dateCheckout = value; }
        public DateTime? DateCheckIn { get => dateCheckIn; set => dateCheckIn = value; }
        public int Status { get => status; set => status = value; }
        public int Discount { get => discount; set => discount = value; }
    }
}
