namespace Factorio.Prototype.Entity
{
	/// <summary>
	/// The Minable component of the Entity Prototype. More info: https://wiki.factorio.com/Types/MinableProperties
	/// </summary>
	/// <seealso cref="Factorio.Prototype.ILUAType" />
	public class Minable : ILUAType
	{
		/// <summary>
		/// How long it takes to mine this object.
		/// Influenced by hardness - higher hardness means longer mining time
		/// </summary>
		/// <value>
		/// The mining time.
		/// </value>
		public float mining_time
		{
			get; set;
		}

		/// <summary>
		/// An axe will lose this much durability when the item is mined.
		/// An axe is not able to mine an object with a hardness equal or greater than its mining power.
		/// Influences mining time - higher hardness means longer mining time
		/// </summary>
		/// <value>
		/// The hardness.
		/// </value>
		public float hardness
		{
			get; set;
		}

		/// <summary>
		/// Which item is dropped when this is mined.
		/// Needs to be the name of an Item Prototype.
		/// </summary>
		/// <value>
		/// The result.
		/// </value>
		public string result
		{
			get; set;
		}

		/// <summary>
		/// How many of result are dropped
		/// </summary>
		/// <value>
		/// The count.
		/// </value>
		public int count
		{
			get; set;
		}

		/// <summary>
		/// Which set of particles to use. Built in particle name are : wooden-particle, coal-particle, stone-particle, shell-particle, iron-ore-particle, and copper-ore-particle.
		/// </summary>
		/// <value>
		/// The mining particle.
		/// </value>
		public string mining_particle
		{
			get; set;
		}

		public Minable(string result, float mining_time = 1, float hardness = 0.5f, int count = 1, string mining_particle = "")
		{
			this.mining_time = mining_time;
			this.hardness = hardness;
			this.result = result;
			this.count = count;
			this.mining_particle = mining_particle;
		}

		public Minable()
		{
		}


		public override string ToString()
		{
			string s = "Minable: ";
			s += GetLuaValue();
			return s;
		}

		public string GetLuaValue()
		{
			return "{ mining_time = " + mining_time.ToString() + ", hardness = " + hardness.ToString() + ", result = \"" + result + "\", count = " + count.ToString() + ", mining_particle = \"" + mining_particle + "\"}";
		}
	}
}
