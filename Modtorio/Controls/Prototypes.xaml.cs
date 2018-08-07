using Factorio.Prototype;
using LuaInterface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
	/// Interaction logic for Prototypes.xaml
	/// </summary>
	public partial class Prototypes : UserControl
	{
		private Lua lua = new Lua();

		public event PrototypeTypeAddedHandler PrototypeTypeAdded;
		public event ViewPrototypeCodeHandler ViewPrototypeCode;
		public event SelectedPrototypeChangedHandler SelectedPrototypeChanged;
		public event DeletedPrototypeHandler DeletedPrototype;

		Dictionary<Type, RadTreeViewItem> prototypeTreeViewItems = new Dictionary<Type, RadTreeViewItem>();
		private Prototype _selectedPrototype;

		public Prototype SelectedPrototype
		{
			get
			{
				return _selectedPrototype;
			}

			set
			{
				if (_selectedPrototype != value)
				{
					_selectedPrototype = value;
					SelectedPrototypeChanged?.Invoke(_selectedPrototype);
				}
			}
		}

		public Prototypes()
		{
			InitializeComponent();
			Prototype.PrototypeCreated += Prototype_PrototypeCreated;
			PopulateTreeView();
		}

		private void PopulateTreeView()
		{
			List<Type> prototypes = Prototype.AvailablePrototypes();
			RadTreeViewItem parent = new RadTreeViewItem();
			parent.Header = "Prototype";
			parent.IsExpanded = true;
			treePrototypes.Items.Add(parent);
			foreach (Type t in prototypes)
			{
				if (!t.IsAbstract)
				{
					PrototypeTypeAdded?.Invoke(t);
					RadTreeViewItem child = addNode(t.FullName.Replace("Factorio.Prototype.", ""), parent);
					prototypeTreeViewItems.Add(t, child);
					child.Tag = t;
					child.ContextMenu = this.Resources["contextMenuType"] as ContextMenu;
					child.ContextMenuOpening += NewTv_ContextMenuOpening;
					child.PreviewMouseRightButtonUp += Child_PreviewMouseRightButtonUp;

				}
			}
		}


		private void Child_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			RadTreeViewItem t = sender as RadTreeViewItem;
			t.IsSelected = true;
		}


		private void mnuCreateNew_Click(object sender, RoutedEventArgs e)
		{
			Activator.CreateInstance(prototypeTreeViewItems.Keys.Single(x => prototypeTreeViewItems[x] == (treePrototypes.SelectedItem as RadTreeViewItem)));
		}

		private void mnuDelete_OnClick(object sender, RoutedEventArgs e)
		{
			//RadTreeViewItem tv = treePrototypes.SelectedItem as RadTreeViewItem;
			//tv.PreviewMouseRightButtonUp -= Child_PreviewMouseRightButtonUp;
			//Prototype p = tv.Tag as Prototype;
			//List<RadPane> panes = radDockMain.Panes.ToList();
			//for (int i = 0; i < panes.Count; i++)
			//{
			//	if (panes[i] is RadDocumentPane)
			//	{
			//		RadDocumentPane doc = panes[i] as RadDocumentPane;
			//		if (doc.Content is TextBox)
			//		{
			//			var tb = doc.Content as TextBox;
			//			if (tb.GetBindingExpression(TextBox.TextProperty).ResolvedSource == p)
			//			{
			//				doc.RemoveFromParent();
			//				i--;
			//			}
			//		}
			//	}
			//}
			//BindingOperations.ClearAllBindings(tv);
			//_mvm.selected_prototype = null;
			//tv.Tag = null;
			//(tv.Parent as RadTreeViewItem).Items.Remove(tv);

		}

		private void mnuViewCode_Click(object sender, RoutedEventArgs e)
		{
			ViewPrototypeCode?.Invoke((treePrototypes.SelectedItem as RadTreeViewItem).Tag as Prototype);
		}

		private void NewTv_ContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			ContextMenu c = (sender as RadTreeViewItem).ContextMenu;
			MenuItem m = c.Items[0] as MenuItem;
			m.Header = "Create new " + (sender as RadTreeViewItem).Header;
		}

		private void treeViewItemSelected(object sender, RoutedEventArgs e)
		{
			if (sender is RadTreeViewItem)
			{
				RadTreeViewItem tv = sender as RadTreeViewItem;
				if (tv.Tag is Prototype)
				{
					Prototype p = tv.Tag as Prototype;
					SelectedPrototype = p;
				}
			}
		}

		public void ParseCodeFile(FileInfo file)
		{
			//TODO: Parse lua files.
			var func = lua.LoadFile(file.FullName);
			lua.GetType();
		}

		private void Prototype_PrototypeCreated(Prototype newPrototype)
		{
			RadTreeViewItem tv = prototypeTreeViewItems[newPrototype.GetType()];
			RadTreeViewItem newTv = new RadTreeViewItem();
			tv.Items.Add(newTv);
			ExpandTo(tv);
			newTv.DoubleClick += NewTv_DoubleClick;
			newTv.PreviewMouseRightButtonUp += Child_PreviewMouseRightButtonUp;
			newTv.Selected += treeViewItemSelected;
			newPrototype.name = "new-" + newPrototype.GetType().Name.ToLower();
			newTv.Tag = newPrototype;


			newTv.ContextMenu = this.Resources["contextMenuInstance"] as ContextMenu;
			Binding myBinding = new Binding();
			myBinding.Source = newTv.Tag;
			myBinding.Path = new PropertyPath("name");
			myBinding.Mode = BindingMode.TwoWay;
			myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
			BindingOperations.SetBinding(newTv, RadTreeViewItem.HeaderProperty, myBinding);
		}

		private RadTreeViewItem addNode(string values, RadTreeViewItem root)
		{
			var n = root;

			foreach (var val in values.Split('.'))
			{
				var isNew = true;

				foreach (var existingNode in n.Items)
				{
					if (((RadTreeViewItem)existingNode).Header.ToString() == val)
					{
						n = (RadTreeViewItem)existingNode;
						isNew = false;
					}
				}

				if (isNew)
				{
					var newNode = new RadTreeViewItem();
					newNode.Header = val;
					n.Items.Add(newNode);

					n = newNode;
				}
			}
			return n;
		}


		private void NewTv_DoubleClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
		{
			ViewPrototypeCode?.Invoke((sender as RadTreeViewItem).Tag as Prototype);
		}

		void ExpandTo(RadTreeViewItem itemToExpandTo)
		{
			if (itemToExpandTo.Parent != null && itemToExpandTo.Parent is RadTreeViewItem)
			{
				ExpandTo(itemToExpandTo.Parent as RadTreeViewItem);
			}
			itemToExpandTo.IsExpanded = true;
		}

		private void txtSearch_SearchTextChanged(object sender, EventArgs e)
		{

		}
	}

	public delegate void PrototypeTypeAddedHandler(Type t);
	public delegate void ViewPrototypeCodeHandler(Prototype p);
	public delegate void SelectedPrototypeChangedHandler(Prototype p);
	public delegate void DeletedPrototypeHandler(Prototype p);
}
