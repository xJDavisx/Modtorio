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
using Telerik.Windows.Controls;
using System.Windows.Forms;

namespace Modtorio
{
	/// <summary>
	/// Interaction logic for frmSettings.xaml
	/// </summary>
	public partial class frmSettings : RadWindow
	{
		public frmSettings()
		{
			InitializeComponent();
			Icon = frmMain.ShowInTaskbar(this, "Settings");
		}

		private void btnBrowseModDir_Click(object sender, RoutedEventArgs e)
		{
			FolderBrowserDialog fb = new FolderBrowserDialog();
			fb.SelectedPath = Properties.Settings.Default.ModDirectory;
			fb.Description = "Open the Factorio Mods Directory so Modtorio knows what mods to load.";
			if(fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				Properties.Settings.Default.ModDirectory = txtModDirectory.Text = fb.SelectedPath;
			}
			Properties.Settings.Default.Save();
		}

		private void btnBrowseBackupDir_Click(object sender, RoutedEventArgs e)
		{
			FolderBrowserDialog fb = new FolderBrowserDialog();
			fb.SelectedPath = Properties.Settings.Default.BackupDirectory;
			fb.Description = "The Backup Directory is where mods will be backed up to before being edited by Modtorio.";
			if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				Properties.Settings.Default.BackupDirectory = txtBackupDirectory.Text = fb.SelectedPath;
			}
			Properties.Settings.Default.Save();
		}

		private void btnBrowseWorkingDir_Click(object sender, RoutedEventArgs e)
		{
			FolderBrowserDialog fb = new FolderBrowserDialog();
			fb.SelectedPath = Properties.Settings.Default.WorkingDirectory;
			fb.Description = "The Backup Directory is where mods will be backed up to before being edited by Modtorio.";
			if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				Properties.Settings.Default.WorkingDirectory = txtWorkingDirectory.Text = fb.SelectedPath;
			}
			Properties.Settings.Default.Save();
		}
	}
}
