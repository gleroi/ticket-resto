using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketResto.Core;

namespace TicketResto.PhoneApp
{
    class AppViewModel : ViewModelBase
    {
		private readonly TicketsApp TicketsApp;

		public AppViewModel()
		{
			this.TicketDescriptions = new ObservableCollection<TicketDescriptionViewModel>();
			this.Results = new ObservableCollection<Result>();
		}

		public AppViewModel(TicketsApp app)
			:  this()
		{
			this.TicketsApp = app;
		}

		public ObservableCollection<TicketDescriptionViewModel> TicketDescriptions { get; set; }

		ObservableCollection<Result> _results;
		public ObservableCollection<Result> Results
		{
			get { return this._results; }
			private set
			{
				this._results = value; this.RaisePropertyChanged();
			}
		}

		void AddTicketDescription()
        {
            this.TicketDescriptions.Add(new TicketDescriptionViewModel());
			this.NotifyOfPropertyChange(() => this.CanCompute);
		}

		private decimal _billValue;

		public decimal BillValue
		{
			get { return _billValue; }
			set
			{
				_billValue = value;
				this.RaisePropertyChanged();
				this.NotifyOfPropertyChange(() => this.CanCompute);
			}
		}

		public void Compute()
		{
			var tickets = this.TicketDescriptions.Select(desc => new TicketDescription(desc.Value, desc.MaxQuantity));
			this.Results = new ObservableCollection<Result>(this.TicketsApp.ComputeRepartition(this.BillValue, tickets));
		}

		public bool CanCompute
		{
			get { return this.TicketDescriptions.Any() && this.BillValue != 0; }
		}

	}
}
