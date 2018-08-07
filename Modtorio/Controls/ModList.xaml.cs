using System;
using System.Collections.Generic;
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
using Modtorio;
using Telerik.Windows.Controls;
using System.Reflection;
using System.ComponentModel;

namespace Modtorio.Controls
{
	/// <summary>
	/// Interaction logic for ModList.xaml
	/// </summary>
	public partial class ModList : UserControl, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public event ModSelectionChangedHandler ModSelectionChanged;

		PropertyInfo groupingProperty;
		PropertyInfo sortingProperty;

		public ModList()
		{
			InitializeComponent();
			Mod.AllModsLoaded += Mod_AllModsLoaded;
			cboGroupBy.Items.Clear();
			cboSortBy.Items.Clear();
			foreach (PropertyInfo p in typeof(Mod).GetProperties().Where(x => x.PropertyType == typeof(string)).OrderBy(x => x.Name))
			{
				Telerik.Windows.Controls.Label label = new Telerik.Windows.Controls.Label();
				label.Content = p.Name;
				label.Tag = p;
				Telerik.Windows.Controls.Label label2 = new Telerik.Windows.Controls.Label();
				label2.Content = p.Name;
				label2.Tag = p;
				cboSortBy.Items.Add(label);
				cboGroupBy.Items.Add(label2);
			}
		}

		private void Mod_AllModsLoaded()
		{
			SortTreeView();
		}

		private void SortTreeView()
		{
			ClearTree(treeModList);
			txtSearch.ItemsSource = null;
			List<string> uniqueValues = new List<string>();
			List<string> grouping = new List<string>();
			RadTreeViewItem groupNode = new RadTreeViewItem();
			foreach (Mod m in Mod.Mods.OrderBy(x => sortingProperty != null ? sortingProperty.GetValue(x) as string : x.Name))
			{

				foreach (PropertyInfo p in typeof(Mod).GetProperties().Where(x => x.PropertyType == typeof(string)))
				{
					string s = p.GetValue(m) as string;
					if (!uniqueValues.Contains(s))
					{
						uniqueValues.Add(s);
					}
				}

				var node = new RadTreeViewItem()
				{
					Header = m.Name,
					Tag = m
				};
				node.Selected += ModNode_Selected;

				if (groupingProperty != null)
				{
					string groupPropVal = groupingProperty.GetValue(m) as string;
					if (!grouping.Contains(groupPropVal))
					{
						grouping.Add(groupPropVal);
						groupNode = new RadTreeViewItem()
						{
							Header = groupPropVal
						};
						treeModList.Items.Add(groupNode);
					}
					groupNode.Items.Add(node);
				}
				else
				{
					treeModList.Items.Add(node);
				}
			}
			txtSearch.ItemsSource = uniqueValues;
		}

		private void ModNode_Selected(object sender, Telerik.Windows.RadRoutedEventArgs e)
		{
			SelectedMod = (sender as RadTreeViewItem).Tag as Mod;
		}

		private void ClearTree(RadTreeView treeView)
		{
			foreach(RadTreeViewItem i in treeView.Items)
			{
				ClearTree(i);
			}
			treeView.Items.Clear();
		}

		private void ClearTree(RadTreeViewItem root)
		{
			foreach (RadTreeViewItem i in root.Items)
			{
				ClearTree(i);
				try
				{
					i.Selected -= ModNode_Selected;
				}
				catch{ }
			}
		}

		private void txtSearch_SearchTextChanged(object sender, EventArgs e)
		{
		}

		private void cboSortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			sortingProperty = ((sender as RadComboBox).SelectedItem as Telerik.Windows.Controls.Label).Tag as PropertyInfo;
			SortTreeView();
		}

		private void cboGroupBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			groupingProperty = ((sender as RadComboBox).SelectedItem as Telerik.Windows.Controls.Label).Tag as PropertyInfo;
			SortTreeView();
		}

		public static readonly DependencyProperty SelectedModProperty = DependencyProperty.Register(nameof(SelectedMod), typeof(Mod), typeof(ModList));
		public Mod SelectedMod
		{
			get
			{
				return (Mod)GetValue(SelectedModProperty);
			}
			set
			{
				if(SelectedMod != value)
				{
					SetValue(SelectedModProperty, value);
					ModSelectionChanged?.Invoke(SelectedMod);
					Notify(nameof(SelectedMod));
				}
			}
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
	public delegate void ModSelectionChangedHandler(Mod selectedMod);
}
