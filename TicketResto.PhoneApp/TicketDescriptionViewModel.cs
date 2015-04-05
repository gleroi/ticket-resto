using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicketResto.PhoneApp
{
    class TicketDescriptionViewModel : ViewModelBase
    {
        public TicketDescriptionViewModel()
        {
            this.MaxQuantity = 10;
            this.Value = 0;
        }

        private int _maxQuantity;

        public int MaxQuantity
        {
            get { return _maxQuantity; }
            set { _maxQuantity = value; this.RaisePropertyChanged(); }
        }

        private decimal _value;

        public decimal Value
        {
            get { return _value; }
            set { _value = value; this.RaisePropertyChanged();}
        }

        void IncreaseQuantity()
        {
            this.MaxQuantity += 1;
        }

        void DecreaseQuantity()
        {
            this.MaxQuantity -= 1;
        }
    }
}
