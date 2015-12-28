using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Connect.Smartcom.Models
{
    public class CookieToOrderNoAssociation
    {
        private int cookie;
        public int Cookie
        {
            get
            {
                return this.cookie;
            }
        }

        private string orderNo;
        public string OrderNo
        {
            get
            {
                return this.orderNo;
            }
        }

        private CookieToOrderNoAssociation() { }
        public CookieToOrderNoAssociation(int cookie, string orderNo)
        {
            this.cookie = cookie;
            this.orderNo = orderNo;
        }

        public override string ToString()
        {
            return String.Format("{0}, {1}", this.cookie, this.orderNo);
        }
    }
}
