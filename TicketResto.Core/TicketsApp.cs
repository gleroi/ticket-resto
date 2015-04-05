using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketResto.Core
{
    public class TicketsApp
    {
        public IEnumerable<Result> ComputeRepartition(decimal bill, IEnumerable<TicketDescription> descriptions)
        {
			var results = new List<Result>();
			foreach (var sol in EnumerateSolutions(descriptions))
			{
				var remainingChange = bill - sol.Sum(desc => desc.Value * desc.MaxQuantity);
					results.Add(new Result
					{
						ChangeValue = remainingChange,
						Tickets = new ObservableCollection<TicketDescription>(sol),
					});
			}
			return results.OrderBy(desc => Math.Abs(desc.ChangeValue));
        }

		private List<TicketDescription> Next(List<TicketDescription> current, List<TicketDescription> maxs)
		{
			var next = current.Select(desc => new TicketDescription(desc.Value, desc.MaxQuantity)).ToList();

			for (int i = 0; i < current.Count; i++)
			{
				var ticket = next[i];
				if (ticket.MaxQuantity + 1 > maxs[i].MaxQuantity)
				{
					next[i] = new TicketDescription(ticket.Value, 0);
				}
				else
				{
					next[i] = new TicketDescription(ticket.Value, ticket.MaxQuantity + 1);
					break;
				}
			}
			return next;
		}

		private List<TicketDescription> GetZero(IEnumerable<TicketDescription> descriptions)
		{
			return descriptions.Select(desc => new TicketDescription(desc.Value, 0)).ToList();
		}

		public IEnumerable<IEnumerable<TicketDescription>> EnumerateSolutions(IEnumerable<TicketDescription> descriptions) {
			var descs = descriptions.ToList();

			var zero = GetZero(descriptions);
			var current = Next(zero, descs);

			while (!current.All(desc => desc.MaxQuantity == 0))
			{
				yield return current;
				current = Next(current, descs);
			}
		}
    }
}
