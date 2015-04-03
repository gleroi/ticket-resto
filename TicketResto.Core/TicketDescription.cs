using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicketResto.Core
{
    class TicketDescription
    {
        public decimal Value { get; private set; }
        public int MaxQuantity { get; private set; }

        public TicketDescription(decimal value, int maxQuantity)
        {
            this.Value = value;
            this.MaxQuantity = maxQuantity;
        }
    }
}
