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
using System.Windows.Shapes;
using System.IO;
using System.ComponentModel;

namespace Modtorio
{
	/// <summary>
	/// Interaction logic for frmSettings.xaml
	/// </summary>
	public partial class frmSettings : Window
	{
		public frmSettings()
		{
			InitializeComponent();
		}

		private void btnBrowseModDir_Click(object sender, RoutedEventArgs e)
		{
			Gat.Controls.OpenDialogView openDialog = new Gat.Controls.OpenDialogView();
			Gat.Controls.OpenDialogViewModel vm = (Gat.Controls.OpenDialogViewModel)openDialog.DataContext;
			vm.IsDirectoryChooser = true;
			vm.Show();

			Properties.Settings.Default.ModDirectory = vm.SelectedFilePath;
			Properties.Settings.Default.Save();
		}

		private void btnBrowseBackupDir_Click(object sender, RoutedEventArgs e)
		{
			Gat.Controls.OpenDialogView openDialog = new Gat.Controls.OpenDialogView();
			Gat.Controls.OpenDialogViewModel vm = (Gat.Controls.OpenDialogViewModel)openDialog.DataContext;
			vm.IsDirectoryChooser = true;
			vm.Show();

			Properties.Settings.Default.BackupDirectory = vm.SelectedFilePath;
			Properties.Settings.Default.Save();
		}
	}
}
