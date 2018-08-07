using System;

namespace Factorio.Prototype.Entity
{
	/// <summary>
	/// This type is used to produce sound from in-game entities, ambient sound, and music.
	/// </summary>
	/// <seealso cref="Factorio.Prototype.ILUAType" />
	public class Sound : ILUAType
	{
		/// <summary>
		/// Used to let the game know what audio file you want it to use.
		/// </summary>
		/// <value>
		/// The filename.
		/// </value>
		[Mandatory]
		public string filename
		{
			get; set;
		}

		/// <summary>
		/// Decides how loud the audio is.
		/// </summary>
		/// <value>
		/// The volume.
		/// </value>
		[Mandatory]
		public double volume
		{
			get; set;
		}

		public Sound(string filename, double volume)
		{
			this.filename = filename;
			this.volume = volume;
		}

		public Sound()
		{
		}

		public override string ToString()
		{
			string s = "Sound: ";
			s += GetLuaValue().Replace("\r\n","").Replace("\t", "");
			return s;
		}

		public string GetLuaValue()
		{
			if (filename == null || filename == "")
			{
#if DEBUG
				filename = "__base__/sound/machine-open.ogg";

#endif
#if !DEBUG
				throw new Exception("filename can't be null or empty.");
#endif
			}
			return "\r\n\t\t\t{\r\n\t\t\t\tfilename = \"" + filename + "\",\r\n\t\t\t\tvolume = " + volume.ToString() + "\r\n\t\t\t}";
		}
	}
}
