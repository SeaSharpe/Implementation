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

    public class CartViewModel
    {
        public CartViewModel(OrderItem item, bool download, bool hardCopy)
        {
            this.item = item;
            this.download = download;
            this.hardCopy = hardCopy;
        }

        public OrderItem item;

        public bool download;

        public bool hardCopy;
    }
}