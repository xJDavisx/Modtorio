namespace Factorio.Prototype.Entity
{
	/// <summary>
	/// A Color object used by prototypes.
	/// </summary>
	/// <seealso cref="Factorio.Prototype.ILUAType" />
	public class Color : ILUAType
	{

		/// <summary>
		/// Gets or sets the red component of the color. Between 0 and 1.
		/// </summary>
		/// <value>
		/// The red component.
		/// </value>
		[Optional]
		public float r
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the green component of the color. Between 0 and 1.
		/// </summary>
		/// <value>
		/// The green component.
		/// </value>
		[Optional]
		public float g
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the blue component of the color. Between 0 and 1.
		/// </summary>
		/// <value>
		/// The blue component.
		/// </value>
		[Optional]
		public float b
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the alpha component of the color. Between 0 and 1.
		/// </summary>
		/// <value>
		/// The alpha component.
		/// </value>
		[Optional]
		public float a
		{
			get;
			set;
		}

		/// <summary>
		/// Returns a <see cref="Factorio.Prototype.Color"/> using standard 0 - 255 RGBA values.
		/// </summary>
		/// <param name="r">The red component.</param>
		/// <param name="g">The green component.</param>
		/// <param name="b">The blue component.</param>
		/// <param name="a">The alpha component.</param>
		/// <returns><see cref="Factorio.Prototype.Color"/></returns>
		static Color FromRGBA255(int r, int g, int b, int a = 255)
		{
			return new Color((float)r / 255f, (float)g / 255f, (float)b / 255f, (float)a / 255f);
		}

		/// <summary>
		/// Creates a <see cref="Factorio.Prototype.Color"/> object with components ranging from 0f to 1f. All are optional and default to 0f, except alpha which defaults to 1f (fully opaque).
		/// </summary>
		/// <param name="r">The red component.</param>
		/// <param name="g">The green component.</param>
		/// <param name="b">The blue component.</param>
		/// <param name="a">The alpha component.</param>
		public Color(float r = 0, float g = 0, float b = 0, float a = 1)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}

		public Color()
		{
			this.r = 0;
			this.g = 0;
			this.b = 0;
			this.a = 1;
		}
		
		public override string ToString()
		{
			string s = "Color: ";
			s += GetLuaValue();
			return s;
		}

		public string GetLuaValue()
		{
			string c = "{";
			if (r != 0)
			{
				c += "r=" + r.ToString() + ", ";
			}
			if (g != 0)
			{
				c += "g=" + g.ToString() + ", ";
			}
			if (b != 0)
			{
				c += "b=" + b.ToString() + ", ";
			}
			if (a != 1)
			{
				c += "a=" + a.ToString() + ", ";
			}
			if (c.Contains(", "))
			{
				c = c.Remove(c.LastIndexOf(", "));
			}
			c += "}";
			return c;
		}
	}
}
