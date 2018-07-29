using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Jesse.IO;
using Factorio.Prototype;

namespace Modtorio
{
	public delegate void ModLoadedHandler(Mod m);
	public delegate void ModLoadFailedHandler(string errorMessage);
	public delegate void AllModsLoadedHandler();
	public class Mod
	{
		static List<Mod> mods = new List<Mod>();
		public static event ModLoadedHandler ModLoaded;
		public static event ModLoadFailedHandler ModLoadFailed;
		public static event AllModsLoadedHandler AllModsLoaded;
		public static int modsFound = 0;


		bool enabled = false;
		string name, author, version, title, homepage, description, factorio_version;
		List<string> dependencies; //TODO: need to compare if the required dependent mod is installed and enabled.

		private List<Prototype> prototypes = new List<Prototype>();

		/// <summary>
		/// List of mods in the current Factorio Mod Directory.
		/// </summary>
		public static List<Mod> Mods
		{
			get
			{
				return mods;
			}

			set
			{
				mods = value;
			}
		}
		static BackgroundWorker bgwLoad;

		public Mod()
		{
		}

		public string Name
		{
			get
			{
				return name;
			}

			set
			{
				name = value;
			}
		}
		public string Author
		{
			get
			{
				return author;
			}

			set
			{
				author = value;
			}
		}
		public string Version
		{
			get
			{
				return version;
			}

			set
			{
				version = value;
			}
		}
		public string Title
		{
			get
			{
				return title;
			}

			set
			{
				title = value;
			}
		}
		public string Homepage
		{
			get
			{
				return homepage;
			}

			set
			{
				homepage = value;
			}
		}
		public string Description
		{
			get
			{
				return description;
			}

			set
			{
				description = value;
			}
		}
		public string FactorioVersion
		{
			get
			{
				return factorio_version;
			}

			set
			{
				factorio_version = value;
			}
		}
		public bool Enabled
		{
			get
			{
				return enabled;
			}

			set
			{
				enabled = value;
			}
		}

		public static int NumModsLoaded
		{
			get
			{
				return mods.Count;
			}
		}

		public static void LoadMods()
		{
			Mods.Clear();
			bgwLoad = new BackgroundWorker();
			bgwLoad.WorkerReportsProgress = true;
			var zipFiles = (new System.IO.DirectoryInfo(Properties.Settings.Default.ModDirectory)).GetFiles().Where(x => x.Extension.ToLower() == ".zip");
			modsFound = zipFiles.Count();
			File.Log("Mods found: " + modsFound);
			bgwLoad.DoWork += bgwLoad_DoWork;
			bgwLoad.ProgressChanged += BgwLoad_ProgressChanged;
			bgwLoad.RunWorkerCompleted += BgwLoad_RunWorkerCompleted;
			bgwLoad.RunWorkerAsync(zipFiles);
		}

		private static void BgwLoad_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			try
			{
				Mod m = e.UserState as Mod;
				ModLoaded?.Invoke(m);
			}
			catch
			{
				try
				{
					string s = e.UserState as string;
					ModLoadFailed?.Invoke(s);
				}
				catch
				{
					Jesse.IO.File.Log("Unknown type passed to bgwLoad.ReportProgress in Mod.cs");
				}
			}
		}

		private static void BgwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			bgwLoad.DoWork -= bgwLoad_DoWork;
			bgwLoad.RunWorkerCompleted -= BgwLoad_RunWorkerCompleted;
			bgwLoad.Dispose();
			bgwLoad = null;
			AllModsLoaded?.Invoke();
		}

		private static void bgwLoad_DoWork(object sender, DoWorkEventArgs e)
		{
			ModList mlst = JsonConvert.DeserializeObject<ModList>(new System.IO.FileInfo(Properties.Settings.Default.ModDirectory +
				"\\mod-list.json").OpenText().ReadToEnd());
			IEnumerable<System.IO.FileInfo> zipFiles = (IEnumerable<System.IO.FileInfo>)e.Argument;
			foreach (System.IO.FileInfo fi in zipFiles)
			{
				Mod m = null;
				try
				{
					Jesse.IO.File.Log("Trying to load info on zip file " + fi.Name);
					var z = new ZipArchive(fi.OpenRead());
					ZipArchiveEntry info = z.Entries.Single(x => x.FullName.ToLower() == fi.Name.ToLower().Replace(".zip", "") + "/info.json");
					System.IO.StreamReader sr = new System.IO.StreamReader(info.Open(), true);
					string s = sr.ReadToEnd();
					sr.Close();
					m = JsonConvert.DeserializeObject<Mod>(s);
					m.Enabled = mlst.mods.Single(x => x.name == m.Name).enabled;
				}
				catch (Exception ex)
				{
					string message = "Failed to load mod file " + fi.FullName + ". Message: " + ex.Message;
					bgwLoad.ReportProgress(0, message);
					File.Log(message);
					continue;
				}
				Mods.Add(m);
				bgwLoad.ReportProgress(0, m);
				File.Log("Loaded mod info for mod " + m.Name);
				System.Threading.Thread.Sleep(10);
			}
		}
	}
	/* Example info.json file:
    {
	    "name":"Laser_Beam_Turrets",
	    "author":"Klonan",
	    "version":"0.1.9",
	    "title":"Laser beam turrets",
	    "homepage":"",
	    "description":"Makes laser turrets shoot beams",
        "dependencies": ["base", "? bobwarfare >= 0.13.0"],
	    "factorio_version": "0.15"
    }
    */
}
