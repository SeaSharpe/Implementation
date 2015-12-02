using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeaSharpe_CVGS.Models
{
    public class OrderHistoryViewModel
    {
        public OrderHistoryViewModel(DateTime opd, DateTime sd, string gn, string pn, decimal pp)
        {
            this.orderPlacementDate = opd;
            this.shipDate = sd;
            this.gameName = gn;
            this.platform = pn;
            this.pricePaid = pp;
        }
        
        public DateTime orderPlacementDate;

        public DateTime shipDate;

        public string gameName;

        public string platform;

        public decimal pricePaid;
    }

    public class OrderDetails
    {
        public OrderDetails(Order order, ApplicationUser user, IEnumerable<OrderItem> items)
        {
            this.order = order;
            this.user = user;
            this.items = items;
        }
        public int orderNumber;

        public Order order;

        public DateTime orderPlacementDate;

        public ApplicationUser user;

        public IEnumerable<OrderItem> items;
    }
}