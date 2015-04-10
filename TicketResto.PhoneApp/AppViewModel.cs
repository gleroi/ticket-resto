using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using TicketResto.Core;

namespace TicketResto.PhoneApp
{
    class AppViewModel : Screen
    {
		private readonly TicketsApp TicketsApp;

		public AppViewModel()
		{
			this.TicketDescriptions = new ObservableCollection<TicketDescriptionViewModel>();
            this.TicketDescriptions.CollectionChanged += TicketDescriptions_CollectionChanged;
			this.Results = new ObservableCollection<Result>();
		}

        void TicketDescriptions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e != null & e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    var ticket = item as TicketDescriptionViewModel;
                    if (ticket != null)
                        ticket.PropertyChanged += ticket_PropertyChanged;
                }
            }

            if (e != null && e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    var ticket = item as TicketDescriptionViewModel;
                    if (ticket != null && !this.TicketDescriptions.Contains(item))
                        ticket.PropertyChanged -= ticket_PropertyChanged;
                }
            }
        }

        void ticket_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.SaveTickets();
        }

		public AppViewModel(TicketsApp app)
			:  this()
		{
			this.TicketsApp = app;
            Initialize();
		}

        private async void Initialize()
        {
            var tickets = await this.TicketsApp.GetTicketDescriptions();
            foreach (var ticket in tickets)
            {
                this.TicketDescriptions.Add(new TicketDescriptionViewModel
                {
                    Value = ticket.Value,
                    MaxQuantity = ticket.MaxQuantity
                });
            }
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

        protected void RaisePropertyChanged([CallerMemberName]string property = null)
        {
            this.NotifyOfPropertyChange(property);
        }

		void AddTicketDescription()
        {
            this.TicketDescriptions.Add(new TicketDescriptionViewModel());
            SaveTickets();
			this.NotifyOfPropertyChange(() => this.CanCompute);
		}

        void RemoveTicketDescription(TicketDescriptionViewModel vm)
        {
            this.TicketDescriptions.Remove(vm);
            SaveTickets();
            this.NotifyOfPropertyChange(() => this.CanCompute);
        }

        private void SaveTickets()
        {
            var tickets = this.TicketDescriptions.Select(desc => new TicketDescription(desc.Value, desc.MaxQuantity));
            this.TicketsApp.SaveTicketDescriptions(tickets);
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
