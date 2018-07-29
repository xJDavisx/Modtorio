using Factorio.Prototype;
using Jesse.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace Modtorio
{
	/// <summary>
	/// Interaction logic for frmMain.xaml
	/// </summary>
	public partial class frmMain : Window
	{
		List<BindingExpression> docWindowsExpressions = new List<BindingExpression>();
		MainViewModel _mvm = new MainViewModel();
		public frmMain()
		{
			InitializeComponent();
			StartupCheck();
			DataContext = _mvm;
			File.saveFile = true;
			Mod.ModLoaded += Mod_ModLoaded;
			Mod.ModLoadFailed += Mod_ModLoadFailed;
			Mod.AllModsLoaded += Mod_AllModsLoaded;
			lblStatus.Content = "Reading mod-list.json contents...";
			Prototype.PrototypeCreated += Prototype_PrototypeCreated;
			Prototype.PrototypeDestroyed += Prototype_PrototypeDestroyed;
			try
			{
				Mod.LoadMods();
			}
			catch
			{
				return;
			}
			pbrStatus.Maximum = Mod.modsFound;
			PopulateTreeView();
			propertyGrid.PropertyValueChanged += PropertyGrid_PropertyValueChanged;
		}

		private void PropertyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
		{
			foreach (var b in docWindowsExpressions)
			{
				b.UpdateTarget();
			}
		}

		private void Prototype_PrototypeDestroyed(Prototype destroyedPrototype)
		{
		}

		private void Prototype_PrototypeCreated(Prototype newPrototype)
		{
		}

		private void PopulateTreeView()
		{
			List<Type> prototypes = Prototype.AvailablePrototypes();
			TreeViewItem parent = new TreeViewItem();
			parent.Header = "Prototype";
			treeEntities.Items.Add(parent);
			foreach (Type t in prototypes)
			{
				if (!t.IsAbstract)
				{
					TreeViewItem child = addNode(t.FullName.Replace("Factorio.Prototype.", ""), parent);
					child.Tag = t;
					child.ContextMenu = this.Resources["contextMenuType"] as ContextMenu;
					child.PreviewMouseRightButtonUp += Child_PreviewMouseRightButtonUp;

				}
			}
		}

		private TreeViewItem addNode(string values, TreeViewItem root)
		{
			var n = root;

			foreach (var val in values.Split('.'))
			{
				var isNew = true;

				foreach (var existingNode in n.Items)
				{
					if (((TreeViewItem)existingNode).Header.ToString() == val)
					{
						n = (TreeViewItem)existingNode;
						isNew = false;
					}
				}

				if (isNew)
				{
					var newNode = new TreeViewItem();
					newNode.Header = val;
					n.Items.Add(newNode);

					n = newNode;
				}
			}
			return n;
		}

		private void Child_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			TreeViewItem t = sender as TreeViewItem;
			t.IsSelected = true;
		}

		private void StartupCheck()
		{
			System.IO.Directory.Exists("");
			if (Properties.Settings.Default.ModDirectory == "")
			{
				Properties.Settings.Default.ModDirectory = "";
			}
		}

		private void mnuOpenMod_Click(object sender, RoutedEventArgs e)
		{

		}

		private void mnuSettings_Click(object sender, RoutedEventArgs e)
		{
			frmSettings settings = new frmSettings();
			settings.Show();
		}

		private void Mod_AllModsLoaded()
		{
			lblStatus.Content = "Ready";
			pbrStatus.Maximum = 100;
			pbrStatus.Value = 0;
			pbrStatus.Visibility = Visibility.Collapsed;
		}

		~frmMain()
		{
			Mod.ModLoaded -= Mod_ModLoaded;
			Mod.ModLoadFailed -= Mod_ModLoadFailed;
			Mod.AllModsLoaded -= Mod_AllModsLoaded;
		}

		private void Mod_ModLoaded(Mod m)
		{
			lblStatus.Content = m.Name;
			pbrStatus.Value++;
		}

		private void Mod_ModLoadFailed(string errorMessage)
		{

		}

		private void mnuViewCode_Click(object sender, RoutedEventArgs e)
		{
			Prototype p = (treeEntities.SelectedItem as TreeViewItem).Tag as Prototype;
			LayoutDocument doc = new LayoutDocument();

			Binding myBinding = new Binding();
			myBinding.Source = p;
			myBinding.Path = new PropertyPath("name");
			myBinding.Mode = BindingMode.TwoWay;
			myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
			BindingOperations.SetBinding(doc, LayoutDocument.TitleProperty, myBinding);

			TextBox t = new TextBox()
			{
				Margin = new Thickness(0),
				VerticalAlignment = VerticalAlignment.Stretch,
				HorizontalAlignment = HorizontalAlignment.Stretch
			};
			Binding textBinding = new Binding();
			textBinding.Source = p;
			textBinding.Path = new PropertyPath("LuaCode");
			textBinding.Mode = BindingMode.OneWay;
			textBinding.UpdateSourceTrigger = UpdateSourceTrigger.Explicit;
			BindingOperations.SetBinding(t, TextBox.TextProperty, textBinding);
			docWindowsExpressions.Add(t.GetBindingExpression(TextBox.TextProperty));

			doc.ContentId = p.name;
			doc.Content = t;
			this.DocPane.Children.Add(doc);
			doc.Closed += CodeDocClosed;
			doc.IsEnabled = true;
			doc.DockAsDocument();
		}

		private void CodeDocClosed(object sender, EventArgs e)
		{
			LayoutDocument doc = sender as LayoutDocument;
			BindingExpression b = (doc.Content as TextBox).GetBindingExpression(TextBox.TextProperty);
			docWindowsExpressions.Remove(b);
			doc.Closed -= CodeDocClosed;
		}

		private void mnuDelete_OnClick(object sender, RoutedEventArgs e)
		{
			TreeViewItem tv = treeEntities.SelectedItem as TreeViewItem;
			tv.PreviewMouseRightButtonUp -= Child_PreviewMouseRightButtonUp;
			Prototype p = tv.Tag as Prototype;
			for (int i = 0; i < DocPane.Children.Count; i++)
			{
				if (DocPane.Children[i] is LayoutDocument)
				{
					LayoutDocument doc = DocPane.Children[i] as LayoutDocument;
					if (doc.Content is TextBox)
					{
						var tb = doc.Content as TextBox;
						if (tb.GetBindingExpression(TextBox.TextProperty).ResolvedSource == p)
						{
							doc.Close();
							i--;
						}
					}
				}
			}
			BindingOperations.ClearAllBindings(tv);
			_mvm.selected_prototype = null;
			tv.Tag = null;
			(tv.Parent as TreeViewItem).Items.Remove(tv);
			//propertyGrid.SelectedObject = null;

		}

		private void mnuCreateNew_Click(object sender, RoutedEventArgs e)
		{
			TreeViewItem tv = treeEntities.SelectedItem as TreeViewItem;
			Type t = tv.Tag as Type;
			TreeViewItem newTv = new TreeViewItem();
			tv.Items.Add(newTv);
			newTv.PreviewMouseRightButtonUp += Child_PreviewMouseRightButtonUp;
			newTv.Selected += treeViewItemSelected;
			Prototype p = (Prototype)Activator.CreateInstance(t);
			p.name = "new-" + t.Name.ToLower();
			newTv.Tag = p;

			newTv.ContextMenu = this.Resources["contextMenuInstance"] as ContextMenu;
			Binding myBinding = new Binding();
			myBinding.Source = newTv.Tag;
			myBinding.Path = new PropertyPath("name");
			myBinding.Mode = BindingMode.TwoWay;
			myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
			BindingOperations.SetBinding(newTv, TreeViewItem.HeaderProperty, myBinding);

		}

		private void treeViewItemSelected(object sender, RoutedEventArgs e)
		{
			if (sender is TreeViewItem)
			{
				TreeViewItem tv = sender as TreeViewItem;
				if (tv.Tag is Prototype)
				{
					Prototype p = tv.Tag as Prototype;
					_mvm.selected_prototype = p;
				}
			}
		}
	}
}
