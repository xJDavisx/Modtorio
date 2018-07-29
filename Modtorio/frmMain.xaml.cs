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

namespace Modtorio
{
	/// <summary>
	/// Interaction logic for frmMain.xaml
	/// </summary>
	public partial class frmMain : Window
	{
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
			try
			{
				Mod.LoadMods();
			}
			catch
			{
				return;
			}
			pbrStatus.Maximum = Mod.modsFound;
			foreach (Type t in Prototype.AvailablePrototypes())
			{
				if (!t.IsAbstract)
					cboTypeList.Items.Add(new ComboBoxItem()
					{
						Tag = t,
						Content = t.Name
					}
					);
			}
		}

		private void StartupCheck()
		{
			System.IO.Directory.Exists("");
			if (Properties.Settings.Default.ModDirectory == "")
			{
				Properties.Settings.Default.ModDirectory = "";
			}
		}

		private void btnCreate_Click(object sender, RoutedEventArgs e)
		{
			_mvm.created_prototypes.Add((Prototype)Activator.CreateInstance(((Type)((ComboBoxItem)cboTypeList.SelectedItem).Tag)));
		}

		private void lstCreatedTypes_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			if(lstCreatedTypes.SelectedIndex >= 0)
			_mvm.selected_prototype = _mvm.created_prototypes[lstCreatedTypes.SelectedIndex];
		}

		private void cboTypeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cboTypeList.SelectedIndex >= 0)
			{
				btnCreate.IsEnabled = true;
			}
			else
			{
				btnCreate.IsEnabled = false;
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
			Prototype p = _mvm.created_prototypes[lstCreatedTypes.SelectedIndex];
			LayoutDocument doc = new LayoutDocument();
			doc.Title = p.name + " - " + p.GetType().Name;
			doc.ContentId = p.name + " - " + p.GetType().Name;
			doc.Content = new TextBox(){
				Text = p.GetLuaCode(),
				Margin = new Thickness(0),
				VerticalAlignment = VerticalAlignment.Stretch,
				HorizontalAlignment = HorizontalAlignment.Stretch
			};
			this.DocPane.Children.Add(doc);
			doc.IsEnabled = true;
			doc.DockAsDocument();
		}

		private void mnuDelete_OnClick(object sender, RoutedEventArgs e)
		{
			_mvm.created_prototypes.RemoveAt(lstCreatedTypes.SelectedIndex);
		}
	}
}
