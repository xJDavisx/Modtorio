namespace Factorio.Prototype.Entity.EntityWithHealth
{
	/// <summary>
	/// Resistances to certain types of attacks from enemy, and physical damage.
	/// </summary>
	/// <seealso cref="Factorio.Prototype.ILUAType" />
	public class Resistance : ILUAType
	{

		/// <summary>
		/// Specification of the type. Built in values are: "physical", "explosion", "acid", and "fire".
		/// </summary>
		/// <value>
		/// The type.
		/// </value>
		public string type{get;set;} = "";

		/// <summary>
		/// The amount of resistance the type of incoming damage deals. (Lower is better)
		/// </summary>
		/// <value>
		/// The decrease.
		/// </value>
		public double decrease { get; set; } = 0;

		/// <summary>
		/// The percentage of resistance the type of resistance has. (Higher is better)
		/// </summary>
		/// <value>
		/// The percent.
		/// </value>
		public double percent { get; set; } = 0;

		public Resistance()
		{

		}

		public override string ToString()
		{
			string s = "Resistance: ";
			s += GetLuaValue();
			return s;
		}

		public string GetLuaValue()
		{
			return "\t\t\t\t{\r\n" +
				"\t\t\t\t\ttype = \"" + type + "\",\r\n" +
				"\t\t\t\t\tdecrease = " + decrease.ToString() + ",\r\n" +
				"\t\t\t\t\tpercent = " + percent.ToString() + "\r\n\t\t\t\t}";
		}
	}
}
