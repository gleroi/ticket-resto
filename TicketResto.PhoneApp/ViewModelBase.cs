using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace TicketResto.PhoneApp
{
    class ViewModelBase : PropertyChangedBase
    {
        protected void RaisePropertyChanged([CallerMemberName]string property = null)
        {
            this.NotifyOfPropertyChange(property);
        }
    }
}
