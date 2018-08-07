using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace Modtorio.Controls
{
	/// <summary>
	/// Interaction logic for ModStructure.xaml
	/// </summary>
	public partial class ModStructure : UserControl, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public event ModSelectionChangedHandler ModSelectionChanged;
		public event OpenFileHandler OpenFile;

		public ModStructure()
		{
			InitializeComponent();
			ModSelectionChanged += ModStructure_ModSelectionChanged;
		}

		private void ModStructure_ModSelectionChanged(Mod selectedMod)
		{
			selectedMod.Open();
			treeModDirectory.Items.Clear();
			treeModDirectory.Items.Add(PopulateDirTree(selectedMod.workingDir));
		}

		RadTreeViewItem PopulateDirTree(DirectoryInfo root)
		{
			RadTreeViewItem current = new RadTreeViewItem();
			current.Header = root.Name;
			foreach(DirectoryInfo di in root.GetDirectories())
			{
				current.Items.Add(PopulateDirTree(di));
			}
			foreach(FileInfo fi in root.GetFiles())
			{
				RadTreeViewItem file = new RadTreeViewItem();
				file.DoubleClick += TreeViewItem_File_DoubleClick;
				file.Header = fi.Name;
				file.Tag = fi;
				current.Items.Add(file);
			}
			return current;
		}

		private void TreeViewItem_File_DoubleClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
		{
			FileInfo item = (sender as RadTreeViewItem).Tag as FileInfo;
			OpenFile?.Invoke(item);
		}

		public static readonly DependencyProperty SelectedModProperty = DependencyProperty.Register(nameof(SelectedMod), typeof(Mod), typeof(ModStructure));
		public Mod SelectedMod
		{
			get
			{
				return (Mod)GetValue(SelectedModProperty);
			}
			set
			{
				if (SelectedMod != value)
				{
					SetValue(SelectedModProperty, value);
					ModSelectionChanged?.Invoke(SelectedMod);
					Notify(nameof(SelectedMod));
				}
			}
		}

		private void txtSearch_SearchTextChanged(object sender, EventArgs e)
		{

		}

		#region INotifyPropertyChanged implementation

		protected void Notify(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion INotifyPropertyChanged implementation
	}
	public delegate void OpenFileHandler(FileInfo file);
}
