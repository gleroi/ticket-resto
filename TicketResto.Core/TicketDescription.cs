using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicketResto.Core
{
    public class TicketDescription
    {
        public decimal Value { get; set; }
        public int MaxQuantity { get; set; }

        public TicketDescription() { }

        public TicketDescription(decimal value, int maxQuantity)
        {
            this.Value = value;
            this.MaxQuantity = maxQuantity;
        }
    }
}
