namespace Factorio.Prototype.Entity
{
	/// <summary>
	/// A vector is a two-element array containing the x and y components. Unlike <see cref="Factorio.Prototype.Position"/>s, vectors don't use the x, y keys. Positive x goes towards east, positive y goes towards south.
	/// </summary>
	/// <seealso cref="Factorio.Prototype.ILUAType" />
	public class Vector : ILUAType
	{
		public float x
		{
			get;
			set;
		}
		public float y
		{
			get;set;
		}

		public Vector(float x = 0f, float y = 0f)
		{
			this.x = x;
			this.y = y;
		}

		public string GetLuaValue()
		{
			return "{" + x.ToString() + ", " + y.ToString() + "}";
		}

		public override string ToString()
		{
			return GetLuaValue();
		}
	}
}
