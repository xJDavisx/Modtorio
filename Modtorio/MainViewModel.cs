using Factorio.Prototype;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Modtorio
{

	public class MainViewModel : INotifyPropertyChanged
	{
		BindingList<Prototype> _created_prototypes = new BindingList<Prototype>();
		Prototype _selected_prototype;

		public MainViewModel()
		{
		}

		public Prototype selected_prototype
		{
			get
			{
				return _selected_prototype;
			}

			set
			{
				_selected_prototype = value;
				Notify("selected_prototype");
			}
		}

		public BindingList<Prototype> created_prototypes
		{
			get
			{
				return _created_prototypes;
			}

			set
			{
				_created_prototypes = value;
				Notify("created_prototypes");
			}
		}

		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		protected void Notify(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion INotifyPropertyChanged implementation
	}
}
