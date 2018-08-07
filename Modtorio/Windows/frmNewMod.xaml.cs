using System;
using System.Collections.Generic;
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
using System.IO;

using Telerik.Windows.Controls;

namespace Modtorio
{
    /// <summary>
    /// Interaction logic for frmNewMod.xaml
    /// </summary>
    public partial class frmNewMod
    {
		Mod _newMod = new Mod();
        public frmNewMod()
        {
            InitializeComponent();
			Icon = frmMain.ShowInTaskbar(this, "New Mod");
			DataContext = _newMod;
			txtName.Focus();
        }

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}

		private void btnCreate_Click(object sender, RoutedEventArgs e)
		{
			if (CanCreate())
			{
				DirectoryInfo ModDirectory = new DirectoryInfo( Properties.Settings.Default.WorkingDirectory + _newMod.Name + "_" + _newMod.Version + "\\\\");
				if(!ModDirectory.Exists)
					ModDirectory.Create();
				StreamWriter sw = new StreamWriter(File.Create(ModDirectory.FullName + "\\info.json"));
				sw.WriteLine("{");
				sw.WriteLine("\t\"name\": \"" + _newMod.Name + "\",");
				sw.WriteLine("\t\"version\": \"" + _newMod.Version + "\",");
				sw.WriteLine("\t\"title\": \"" + _newMod.Title + "\",");
				sw.WriteLine("\t\"author\": \"" + _newMod.Author + "\",");
				//TODO: Add Contact to mods class.
				//sw.WriteLine("\t\"contact\": \"" + _newMod. + "\",");
				sw.WriteLine("\t\"homepage\": \"" + _newMod.Homepage + "\",");
				sw.WriteLine("\t\"factorio_version\": \"" + _newMod.FactorioVersion + "\",");
				//sw.WriteLine("\t\"dependencies\": \"" + _newMod. + "\",");
				sw.WriteLine("\t\"description\": \"" + _newMod.Description + "\"");
				sw.WriteLine("}");
				sw.Close();
				File.Create(ModDirectory.FullName + "\\control.lua").Close();

				File.Create(ModDirectory.FullName + "\\settings.lua").Close();
				File.Create(ModDirectory.FullName + "\\settings-updates.lua").Close();
				File.Create(ModDirectory.FullName + "\\settings-final-fixes.lua").Close();

				sw = new StreamWriter(File.Create(ModDirectory.FullName + "\\data.lua"));
				sw.WriteLine("require(\"prototypes.item-groups\")");
				sw.WriteLine("require(\"prototypes.item\")");
				sw.WriteLine("require(\"prototypes.fluid\")");
				sw.WriteLine("require(\"prototypes.recipe\")");
				sw.WriteLine("require(\"prototypes.entity\")");
				sw.WriteLine("require(\"prototypes.technology\")");
				sw.Close();
				File.Create(ModDirectory.FullName + "\\data-updates.lua").Close();
				File.Create(ModDirectory.FullName + "\\data-final-fixes.lua").Close();

				Directory.CreateDirectory(ModDirectory.FullName + "\\prototypes");
				File.Create(ModDirectory.FullName + "\\prototypes\\item-groups.lua").Close();
				File.Create(ModDirectory.FullName + "\\prototypes\\item.lua").Close();
				File.Create(ModDirectory.FullName + "\\prototypes\\fluid.lua").Close();
				File.Create(ModDirectory.FullName + "\\prototypes\\recipe.lua").Close();
				File.Create(ModDirectory.FullName + "\\prototypes\\entity.lua").Close();
				File.Create(ModDirectory.FullName + "\\prototypes\\technology.lua").Close();

				Directory.CreateDirectory(ModDirectory.FullName + "\\locale\\en");
				File.Create(ModDirectory.FullName + "\\locale\\en\\config.cfg").Close();

				Directory.CreateDirectory(ModDirectory.FullName + "\\graphics");
			}
		}

		private bool CanCreate()
		{
			if(Mod.Mods.Find(x=>x.Name == _newMod.Name) != null)
				return false;

			return true;
		}

		private void txtName_GotFocus(object sender, RoutedEventArgs e)
		{
			lblDescription.Text = "This is the internal name of your mod, it is used to identify your mod in code.";
		}

		private void txtAuthor_GotFocus(object sender, RoutedEventArgs e)
		{
			lblDescription.Text = "Your name!";
		}

		private void txtVersion_GotFocus(object sender, RoutedEventArgs e)
		{
			lblDescription.Text = "This is the version of your mod. This can be anything you want, provided it's a number. Some mods start at 0.0.1 or 0.1.0, while others follow Factorio versions and start at 0.16.0 (for Factorio version 0.16.X)";
		}

		private void txtTitle_GotFocus(object sender, RoutedEventArgs e)
		{
			lblDescription.Text = "The pretty title of your mod, this will be displayed on the mods screen and when you submit it to the mod portal.";
		}

		private void txtHomepage_GotFocus(object sender, RoutedEventArgs e)
		{
			lblDescription.Text = "The homepage of your mod, put a website here if you have one for the mod. Not required.";
		}

		private void txtDescription_GotFocus(object sender, RoutedEventArgs e)
		{
			lblDescription.Text = "A short description of your mod.";
		}

		private void txtDependencies_GotFocus(object sender, RoutedEventArgs e)
		{
			lblDescription.Text = "Any dependencies of your mod. Some form of \"base\" should always be here, so base gets loaded first.";
		}

		private void txtFactorioVersion_GotFocus(object sender, RoutedEventArgs e)
		{
			lblDescription.Text = "This tells the game what version the mod is for, this must match the version you're developing the mod for.";
		}
	}
}
