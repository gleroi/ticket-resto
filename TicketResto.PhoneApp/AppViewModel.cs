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
    class AppViewModel : Screen, IProgress<int>
    {
        private readonly TicketsApp TicketsApp;

        public AppViewModel()
        {
            this.TicketDescriptions = new ObservableCollection<TicketDescriptionViewModel>();
            this.TicketDescriptions.CollectionChanged += TicketDescriptions_CollectionChanged;
            this.Results = new ObservableCollection<Result>();
            this.IsComputing = false;
            //if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            //{
            //    this.TicketDescriptions.Add(new TicketDescriptionViewModel { MaxQuantity = 5, Value = 7.66M });
            //    this.TicketDescriptions.Add(new TicketDescriptionViewModel { MaxQuantity = 15, Value = 17.66M });
            //    this.TicketDescriptions.Add(new TicketDescriptionViewModel { MaxQuantity = 88, Value = 88.00M });
            //}
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
            : this()
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

        private bool isComputing;
        public bool IsComputing
        {
            get { return isComputing; }
            set
            {
                isComputing = value;
                if (!IsComputing)
                {
                    this.ComputeProgress = 0;
                }
                this.RaisePropertyChanged();
            }
        }

        private int computeProgress;
        public int ComputeProgress
        {
            get { return computeProgress; }
            set { computeProgress = value; this.RaisePropertyChanged(); }
        }


        public async void Compute()
        {
            this.IsComputing = true;

            var tickets = this.TicketDescriptions.Select(desc => new TicketDescription(desc.Value, desc.MaxQuantity));
            var repartition = await this.TicketsApp.ComputeRepartition(this.BillValue, tickets, this);
            this.Results = new ObservableCollection<Result>(repartition);
            this.IsComputing = false;
        }

        public bool CanCompute
        {
            get { return this.TicketDescriptions.Any() && this.BillValue != 0; }
        }

        #region IProgress<int> Members

        public void Report(int value)
        {
            value = value > 100 ? 100 : value;
            this.ComputeProgress = value;
        }

        #endregion
    }
}
