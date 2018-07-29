using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;
using Factorio.Prototype.Entity.EntityWithHealth;
using Factorio.Prototype.Entity;
using System.ComponentModel;

namespace Factorio.Prototype
{
	/// <summary>
	/// The abstract base for all prototypes. All prototypes inherit from this prototype.
	/// </summary>
	public abstract class Prototype : ILUAConvertable, INotifyPropertyChanged
	{
		public static event PrototypeCreatedHandler PrototypeCreated;
		public static event PrototypeDestroyedHandler PrototypeDestroyed;

		private static List<Prototype> _instances = new List<Prototype>();

		private string _type;
		private string _name;
		private string _order;
		private string _localised_name;
		private string _localised_description;

		public Prototype()
		{
			_instances.Add(this);
			_type = "";
			_name = "";
			_order = "";
			_localised_name = "";
			_localised_description = "";
			PrototypeCreated?.Invoke(this);
		}

		~Prototype()
		{
			_instances.Remove(this);
			PrototypeDestroyed?.Invoke(this);
		}

		public static List<Prototype> GetInstances()
		{
			return _instances;
		}

		#region Public Properties

		[Category("Mandatory")]
		[Description("Specification of the type of the prototype.\r\nFor a list of all available types and their properties, see prototype definitions https://wiki.factorio.com/Prototype_definitions \r\nFor a list of all types used in vanilla, see data.raw https://wiki.factorio.com/Data.raw.")]
		[Mandatory]
		/// <summary>
		/// Specification of the type of the prototype.
		/// For a list of all available types and their properties, see prototype definitions https://wiki.factorio.com/Prototype_definitions.
		/// For a list of all types used in vanilla, see data.raw https://wiki.factorio.com/Data.raw.
		/// </summary>
		/// <value>
		/// The type.
		/// </value>
		public string type
		{
			get
			{
				return _type;
			}

			protected set
			{
				_type = value;
			}
		}

		[Category("Mandatory")]
		[Description("Unique textual identification of the prototype.\r\n" 
			+ "For a list of all names used in vanilla, see data.raw https://wiki.factorio.com/Data.raw. \r\n"
			+ "May not contain a \".\" (period), may not exceed a length of 200 characters.")]
		[Mandatory]
		/// <summary>
		/// Unique textual identification of the prototype.
		/// For a list of all names used in vanilla, see data.raw https://wiki.factorio.com/Data.raw.
		/// May not contain a "." (period), may not exceed a length of 200 characters.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string name
		{
			get
			{
				return _name;
			}

			set
			{
				if (value.Contains("."))
				{
					throw new Exception("The name of a Prototype cannot contain a \".\".");
				}
				_name = CapStringLength(value, 200);
				Notify("name");
			}
		}

		[Category("Optional")]
		[Description("Used to order items in inventory, recipes and GUI's.\r\n"
			+ "May not exceed a length of 200 characters.")]
		[Optional]
		/// <summary>
		/// Used to order items in inventory, recipes and GUI's.
		/// May not exceed a length of 200 characters.
		/// </summary>
		/// <value>
		/// The order.
		/// </value>
		public string order
		{
			get
			{
				return _order;
			}

			set
			{
				_order = CapStringLength(value, 200);
				Notify("order");
			}
		}

		[Category("Optional")]
		[Description("")]
		[Optional]
		/// <summary>
		/// Gets or sets the name of the localised.
		/// </summary>
		/// <value>
		/// The localised name.
		/// </value>
		public string localised_name
		{
			get
			{
				return _localised_name;
			}

			set
			{
				_localised_name = value;
				Notify("localised_name");
			}
		}

		[Category("Optional")]
		[Description("")]
		[Optional]
		/// <summary>
		/// Gets or sets the localised description.
		/// </summary>
		/// <value>
		/// The localised description.
		/// </value>
		public string localised_description
		{
			get
			{
				return _localised_description;
			}

			set
			{
				_localised_description = value;
				Notify("localised_description");
			}
		}

		#endregion

		/// <summary>
		/// Generates the lua code for this prototype.
		/// </summary>
		/// <returns>lua code for this prototype.</returns>
		public string GetLuaCode()
		{
			string code = "data:extend(\r\n\t{\r\n\t\t{\r\n";
			foreach (PropertyInfo p in GetType().GetProperties().Where(x => x.CustomAttributes.Where(c => c.AttributeType == typeof(MandatoryAttribute)).Count() > 0))
			{
				code += "\t\t\t" + ReadPropertyValue(p);
			}
			foreach (PropertyInfo p in GetType().GetProperties().Where(x => x.CustomAttributes.Where(c => c.AttributeType == typeof(OptionalAttribute)).Count() > 0))
			{
				code += "\t\t\t" + ReadPropertyValue(p);
			}
			code += "\t\t}\r\n\t}\r\n)";
			return code;
		}

		[Browsable(false)]
		public string LuaCode
		{
			get
			{
				return GetLuaCode();
			}
		}

		private string ReadPropertyValue(PropertyInfo p)
		{
			string code = "";
			object val = p.GetValue(this);
			code += p.Name + "=";
			if (val != null)
			{
				if (val.GetType().GetInterfaces().Contains(typeof(ILUAType)))
				{
					code += ((ILUAType)val).GetLuaValue();
				}
				//TODO: find a way to get rid of this duplicated code ⇩⇩
				else if (val.GetType() == typeof(List<IconData>))
				{
					code += "\r\n\t\t\t{\r\n";
					List<IconData> icons = val as List<IconData>;
					foreach (var i in icons)
					{
						code += i.GetLuaValue() + ",\r\n";
					}
					code += "\t\t\t}";
				}
				else if (val.GetType() == typeof(List<Loot>))
				{
					code += "\r\n\t\t\t{\r\n";
					List<Loot> loot = val as List<Loot>;
					foreach (var i in loot)
					{
						code += i.GetLuaValue() + ",\r\n";
					}
					code += "\t\t\t}";
				}
				else if (val.GetType() == typeof(List<Resistance>))
				{
					code += "\r\n\t\t\t{\r\n";
					List<Resistance> loot = val as List<Resistance>;
					foreach (var i in loot)
					{
						code += i.GetLuaValue() + ",\r\n";
					}
					code += "\t\t\t}";
				}
				else if (val.GetType() == typeof(List<string>))
				{
					code += "\r\n\t\t\t{\r\n";
					List<string> strs = val as List<string>;
					foreach (var i in strs)
					{
						code += "\t\t\t\t\"" + i + "\",\r\n";
					}
					code += "\t\t\t}";
				}
				else if (val.GetType() == typeof(string))
				{
					code += "\"" + val.ToString() + "\"";
				}
				else if (val.GetType() == typeof(bool))
				{
					code += val.ToString().ToLower();
				}
				else
				{
					//last chance, just call the ToString() method and hope it works... 
					code += val.ToString();
				}
				code += ",\r\n";
			}
			else
			{
				code += "nil,\r\n";
			}

			return code;
		}

		private string LuaLineFromType()
		{
			return "";
		}

		public override string ToString()
		{
			return name + " - " + GetType().Name;
		}

		public static List<Type> AvailablePrototypes()
		{
			return TypeHelper.TypeHelpers.GetDerivedTypes(typeof(Prototype), Assembly.GetAssembly(typeof(Prototype)));
		}

		protected string CapStringLength(string value, int maxLength)
		{
			if (value.Length > maxLength)
			{
				value = value.Substring(0, maxLength);
			}
			return value;
		}


		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		protected void Notify(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion INotifyPropertyChanged implementation
	}

	public delegate void PrototypeCreatedHandler(Prototype newPrototype);
	public delegate void PrototypeDestroyedHandler(Prototype destroyedPrototype);

	internal class OptionalAttribute : Attribute
	{
	}

	internal class MandatoryAttribute : Attribute
	{
	}
}
