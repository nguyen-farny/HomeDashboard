using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoardWPF
{
    public class VehiculeCollection : ObservableCollection<Response>
    {
        public void CopyFrom(IEnumerable<Response> vehicules)
        {
            this.Items.Clear();
            foreach (var p in vehicules)
            {
                this.Items.Add(p);
            }

            this.OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
