using System.Collections.Generic;

namespace Factorio.Prototype.Entity
{
	/// <summary>
	/// Entity flags. More Info: https://wiki.factorio.com/Types/EntityPrototypeFlags
	/// </summary>
	/// <seealso cref="Factorio.Prototype.ILUAType" />
	public class EntityPrototypeFlags : ILUAType
	{
		public List<string> flags
		{
			get; set;
		}

		public EntityPrototypeFlags(List<string> flags = null)
		{
			this.flags = flags;
			if(this.flags == null)
			{
				this.flags = new List<string>();
			}
		}

		public string GetLuaValue()
		{
			string c = "{";

			for (int i = 0; i < flags.Count; i++)
			{
				if (i > 0)
				{
					c += ", ";
				}
				c += "\"" + flags[i] + "\"";
			}
			c += "}";
			return c;
		}
	}
}