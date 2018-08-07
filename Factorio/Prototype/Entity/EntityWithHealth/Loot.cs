namespace Factorio.Prototype.Entity.EntityWithHealth
{
	/// <summary>
	/// The loot is generated when the entity is killed.
	/// </summary>
	/// <seealso cref="Factorio.Prototype.ILUAType" />
	public class Loot : ILUAType
	{
		//more info: https://wiki.factorio.com/Types/Loot
		/// <summary>
		/// The item the spawn on death.
		/// </summary>
		/// <value>
		/// The item.
		/// </value>
		public string item {get;set; } = "";
		public double probability { get; set; } = 1;
		public double count_min { get; set; } = 1;
		public double count_max { get; set; } = 1;

		public Loot()
		{

		}


		public override string ToString()
		{
			string s = "Loot: ";
			s += GetLuaValue().Replace("\r\n", "").Replace("\t", "");
			return s;
		}

		public string GetLuaValue()
		{

			return "\t\t\t\t{\r\n" +
				"\t\t\t\t\tcount_max = " + count_max.ToString() + ",\r\n" +
				"\t\t\t\t\tcount_min = " + count_min.ToString() + ",\r\n" +
				"\t\t\t\t\titem  = \"" + item + "\",\r\n" +
				"\t\t\t\t\tprobability = " + probability.ToString() + "\r\n\t\t\t\t}";
		}
	}
}
