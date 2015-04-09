using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;

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

        public async Task<IEnumerable<TicketDescription>> GetTicketDescriptions()
        {
            var tickets = await this.ReadTickets();
            if (tickets == null || !tickets.Any())
            {
                return new List<TicketDescription> { new TicketDescription(7.50M, 5) };
            }
            return tickets;
        }

        private async Task<IEnumerable<TicketDescription>> ReadTickets()
        {
            var folder = Windows.Storage.ApplicationData.Current.RoamingFolder;
            var file = await folder.CreateFileAsync("tickets.xml", CreationCollisionOption.OpenIfExists);
            var content = await FileIO.ReadTextAsync(file);
            
            if (content == null || String.IsNullOrEmpty(content))
                return null;

            var xmlSerializer = new XmlSerializer(typeof(List<TicketDescription>));
            using (var reader = new StringReader(content))
            {
                var tickets = xmlSerializer.Deserialize(reader) as List<TicketDescription>;
                return tickets;
            }
        }

        public async void SaveTicketDescriptions(IEnumerable<TicketDescription> tickets)
        {
            var folder = Windows.Storage.ApplicationData.Current.RoamingFolder;
            var file = await folder.CreateFileAsync("tickets.xml", CreationCollisionOption.ReplaceExisting);
            var xmlSerializer = new XmlSerializer(typeof(List<TicketDescription>));
            using (var writer = new StringWriter()) 
            {
                xmlSerializer.Serialize(writer, tickets.ToList());
                await FileIO.WriteTextAsync(file, writer.ToString());
            }
        }
    }
}
