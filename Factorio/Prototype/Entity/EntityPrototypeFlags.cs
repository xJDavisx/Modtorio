using System.Collections.Generic;

namespace Factorio.Prototype.Entity
{
	public class EntityPrototypeFlags
	{
		string _value = "";

		public EntityPrototypeFlags()
		{

		}

		public EntityPrototypeFlags(string value)
		{
			this._value = value;
		}

		public static implicit operator string(EntityPrototypeFlags s)
		{
			return s.ToString();
		}

		public static implicit operator EntityPrototypeFlags(string s)
		{
			return new EntityPrototypeFlags(s);
		}

		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (this._value != value)
				{
					this._value = value;
				}
			}
		}

		public override string ToString()
		{
			return _value.ToString();
		}
	}
}