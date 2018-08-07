using Factorio.Prototype;
using Factorio.Prototype.Entity.EntityWithHealth.CraftingMachine;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Editing;
using Jesse.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using Telerik.Windows.Controls;
using Modtorio.Controls;
using System.Globalization;
using System.Windows.Markup;

namespace Modtorio
{
	/// <summary>
	/// Interaction logic for frmTest.xaml
	/// </summary>
	public partial class frmMain : RadWindow
	{
		List<BindingExpression> docWindowsExpressions = new List<BindingExpression>();
		MainViewModel _mvm = new MainViewModel();

		public frmMain()
		{
			InitializeComponent();
			Icon = ShowInTaskbar(this, "Modtorio");
			mnuModListPaneVisible.IsChecked = true;
			mnuModStructurePaneVisible.IsChecked = true;
			StartupCheck();
			DataContext = _mvm;
			Jesse.IO.File.saveFile = true;
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
				lblStatus.Content = "Error Reading Mods: Make sure the Mod Directory Setting is correct!";
				return;
			}
			pbrStatus.Maximum = Mod.modsFound;
			modList.ModSelectionChanged += ModList_ModSelectionChanged;
			modStructure.ModSelectionChanged += ModStructure_ModSelectionChanged;
		}

		private void ModStructure_ModSelectionChanged(Mod selectedMod)
		{
			modList.SelectedMod = selectedMod;
		}

		private void ModList_ModSelectionChanged(Mod selectedMod)
		{
			modStructure.SelectedMod = selectedMod;
		}

		private void PropertyGrid_TargetUpdated(object sender, DataTransferEventArgs e)
		{
			foreach (var b in docWindowsExpressions)
			{
				b.UpdateTarget();
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

		private void Mod_ModLoaded(Mod m)
		{
			lblStatus.Content = m.Name;
			pbrStatus.Value++;
		}

		private void Mod_ModLoadFailed(string errorMessage)
		{

		}

		private void RadDockMain_Close(object sender, Telerik.Windows.Controls.Docking.StateChangeEventArgs e)
		{
			//foreach(var p in e.Panes)
			//{
			//	if(p is RadCodeDocumentPane)
			//	{
			//		var d = p as RadCodeDocumentPane;
			//		BindingExpression b = d.Editor.TextArea.GetBindingExpression(TextArea);
			//		docWindowsExpressions.Remove(b);
			//	}
			//}
		}

		private void mnuNewMod_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
		{
			CreateMod();
		}

		private void CreateMod()
		{
			frmNewMod newMod = new frmNewMod();
			newMod.ShowDialog();
		}

		public static Image ShowInTaskbar(RadWindow control, string title)
		{
			control.Show();
			var window = control.ParentOfType<Window>();
			window.ShowInTaskbar = true;
			window.Title = title;
			var uri = new Uri("pack://application:,,,/Modtorio;component/AppIcon.ico");
			window.Icon = BitmapFrame.Create(uri);
			BitmapImage bitmapImage = new BitmapImage(uri);
			Image i = new Image();
			i.Source = bitmapImage;
			return i;
		}

		private void prototypes_PrototypeTypeAdded(Type t)
		{
			RadMenuItem menuItem = new RadMenuItem();
			menuItem.Header = "New " + t.Name;
			menuItem.Tag = t;
			menuItem.Click += AddPrototypeMenuItemClick;
			mnuAddPrototypes.Items.Add(menuItem);
		}

		private void AddPrototypeMenuItemClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
		{
			Activator.CreateInstance((sender as RadMenuItem).Tag as Type);
		}

		private void prototypes_ViewPrototypeCode(Prototype p)
		{
			RadCodeDocumentPane doc = new RadCodeDocumentPane();
			documentPaneGroup.AddItem(doc, Telerik.Windows.Controls.Docking.DockPosition.Center);
			doc.Editor.Text = p.GetLuaCode();
			Binding myBinding = new Binding();
			myBinding.Source = p;
			myBinding.Path = new PropertyPath("name");
			myBinding.Mode = BindingMode.TwoWay;
			myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
			BindingOperations.SetBinding(doc, RadDocumentPane.TitleProperty, myBinding);
		}

		private void prototypes_SelectedPrototypeChanged(Prototype p)
		{
			_mvm.selected_prototype = p;
		}

		private void modStructure_OpenFile(FileInfo file)
		{
			prototypes.ParseCodeFile(file);
			foreach(RadCodeDocumentPane d in documentPaneGroup.Items)
			{
				if(d.codeFile == file)
				{
					d.IsSelected = true;
					return;
				}
			}
			RadCodeDocumentPane doc = new RadCodeDocumentPane();
			documentPaneGroup.AddItem(doc, Telerik.Windows.Controls.Docking.DockPosition.Center);
			doc.OpenFile(file);
		}
	}

	public abstract class BaseConverter : MarkupExtension
	{
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return this;
		}
	}

	[ValueConversion(typeof(bool), typeof(bool))]
	public class InverseBooleanConverter : BaseConverter, IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture)
		{
			if (targetType != typeof(bool))
				throw new InvalidOperationException("The target must be a boolean");

			return !(bool)value;
		}

		public object ConvertBack(object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture)
		{
			return Convert(value, targetType, parameter, culture);
		}

		#endregion
	}

	[ValueConversion(typeof(object), typeof(string))]
	public class StringFormatConverter : BaseConverter, IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter,
						  System.Globalization.CultureInfo culture)
		{
			string format = parameter as string;
			if (!string.IsNullOrEmpty(format))
			{
				return string.Format(culture, format, value);
			}
			else
			{
				return value.ToString();
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter,
						System.Globalization.CultureInfo culture)
		{
			return null;
		}
	}
}
