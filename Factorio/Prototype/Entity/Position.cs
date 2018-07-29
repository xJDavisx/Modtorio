namespace Factorio.Prototype.Entity
{
	public class Position : ILUAType
	{
		public float x
		{
			get; set;
		}
		public float y
		{
			get; set;
		}

		public Position(float x = 0, float y = 0)
		{
			this.y = y;
			this.x = x;
		}

		public Position()
		{
			this.y = 0;
			this.x = 0;
		}

		public string GetLuaValue()
		{
			return "{x = " + x.ToString() + ", y = " + y.ToString() + "}";
		}
	}
}
