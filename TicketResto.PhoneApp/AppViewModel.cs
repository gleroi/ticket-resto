using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketResto.PhoneApp
{
    class AppViewModel : ViewModelBase
    {
        public ObservableCollection<TicketDescriptionViewModel> TicketDescriptions { get; set; }

        public AppViewModel()
        {
            this.TicketDescriptions = new ObservableCollection<TicketDescriptionViewModel>();
        }

        void AddTicketDescription()
        {
            this.TicketDescriptions.Add(new TicketDescriptionViewModel());
        }
        
    }
}
