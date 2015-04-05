using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketResto.Core
{
	public class Result
	{
		public decimal ChangeValue { get; set; }

		public ObservableCollection<TicketDescription> Tickets { get; set; }
	}
}
