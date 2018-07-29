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
		public uint volume
		{
			get; set;
		}

		public Sound(string filename, uint volume)
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
			s += GetLuaValue().Replace("\r\n","");
			return s;
		}

		public string GetLuaValue()
		{
			if (filename == null || filename == "")
			{
#if DEBUG
				filename = "__MODNAME__/debug_sample.ogg";

#endif
#if !DEBUG
				throw new Exception("filename can't be null or empty.");
#endif
			}
			return "{\r\nfilename = \"" + filename + "\",\r\nvolume = " + volume.ToString() + "\r\n}";
		}
	}
}
