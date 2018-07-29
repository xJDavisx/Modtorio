using Factorio.Prototype.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Factorio.Prototype
{
    public class Item : Prototype
    {
		private uint _stack_size = 50;
		private List<string> _flags = new List<string>();
		private string _place_result = "";
		private string _placed_as_equipment_result = "";
		private string _subgroup = "";
		private bool _stackable = true;
		private bool _primary_place_result_item = false;
		private bool _can_be_mod_opened = false;
		private uint _default_request_amount = 0;
		private string _fuel_category = "";
		private string _burnt_result = "";
		private string _fuel_value = "";
		private double _fuel_acceleration_multiplier = 1;
		private double _fuel_top_speed_multiplier = 1;
		private double _fuel_emissions_multiplier = 1;
		private Color _fuel_glow_color = new Color();

		public Item()
		{
			type="item";
		}

		/// <summary>
		///Count of items of the same name that can be stored in one inventory slot. Must be 1 when stackable is false.
		/// </summary>
		[Description("Count of items of the same name that can be stored in one inventory slot. Must be 1 when stackable is false.")]
		[Mandatory]
		[Category("Mandatory")]
		public uint stack_size
		{
			get
			{
				return _stack_size;
			}

			set
			{
				_stack_size = value;
			}
		}

		/// <summary>
		///Specifies some properties of the item.
		///Possible values are:
		///goes-to-quickbar:           Item is moved to quick bar by default
		///goes-to-main-inventory:  Item is moved to main inventory by default
		///hidden:                           Item will not appear in lists of all items such as those for logistics requests, filters, etc.
		/// </summary>
		[Description("Specifies some properties of the item.\r\n" +
		"Possible values are:\r\n" +
		"goes-to-quickbar:           Item is moved to quick bar by default\r\n" +
		"goes-to-main-inventory:  Item is moved to main inventory by default\r\n" +
		"hidden:                           Item will not appear in lists of all items such as those for logistics requests, filters, etc.")]
		[Mandatory]
		[Category("Mandatory")]
		public List<string> flags
		{
			get
			{
				return _flags;
			}

			set
			{
				_flags = value;
			}
		}

		/// <summary>
		///Name of prototype/Entity that can be built using this item
		/// </summary>
		[Description("Name of prototype/Entity that can be built using this item")]
		[Optional]
		[Category("Optional")]
		public string place_result
		{
			get
			{
				return _place_result;
			}

			set
			{
				_place_result = value;
			}
		}


		[Optional]
		[Category("Optional")]
		public string placed_as_equipment_result
		{
			get
			{
				return _placed_as_equipment_result;
			}

			set
			{
				_placed_as_equipment_result = value;
			}
		}

		/// <summary>
		///Default: "other"
		///Empty text of subgroup is not allowed. (You can ommit the definition to get the default "other").
		/// </summary>
		[Description("Default: \"other\"\r\n" +
		"Empty text of subgroup is not allowed. (You can ommit the definition to get the default \"other\").")]
		[Optional]
		[Category("Optional")]
		public string subgroup
		{
			get
			{
				return _subgroup;
			}

			set
			{
				_subgroup = value;
			}
		}

		/// <summary>
		///Default: true
		/// </summary>
		[Description("Default: true")]
		[Optional]
		[Category("Optional")]
		public bool stackable
		{
			get
			{
				return _stackable;
			}

			set
			{
				_stackable = value;
			}
		}

		/// <summary>
		///Default: false
		/// </summary>
		[Description("Default: false")]
		[Optional]
		[Category("Optional")]
		public bool primary_place_result_item
		{
			get
			{
				return _primary_place_result_item;
			}

			set
			{
				_primary_place_result_item = value;
			}
		}

		/// <summary>
		///Default: false
		/// </summary>
		[Description("Default: false")]
		[Optional]
		[Category("Optional")]
		public bool can_be_mod_opened
		{
			get
			{
				return _can_be_mod_opened;
			}

			set
			{
				_can_be_mod_opened = value;
			}
		}

		/// <summary>
		///Default: The stack size of this item.
		/// </summary>
		[Description("Default: The stack size of this item.")]
		[Optional]
		[Category("Optional")]
		public uint default_request_amount
		{
			get
			{
				return _default_request_amount;
			}

			set
			{
				_default_request_amount = value;
			}
		}

		/// <summary>
		///Must exist when a fuel_value is defined. Name of one of the fuel categories.
		/// </summary>
		[Description("Must exist when a fuel_value is defined. Name of one of the fuel categories.")]
		[Optional]
		[Category("Optional")]
		public string fuel_category
		{
			get
			{
				return _fuel_category;
			}

			set
			{
				_fuel_category = value;
			}
		}

		/// <summary>
		///The item that is the result when this item gets burned as fuel.
		/// </summary>
		[Description("The item that is the result when this item gets burned as fuel.")]
		[Optional]
		[Category("Optional")]
		public string burnt_result
		{
			get
			{
				return _burnt_result;
			}

			set
			{
				_burnt_result = value;
			}
		}

		/// <summary>
		///Default: "0J"
		///Mandatory when fuel_acceleration_multiplier, fuel_top_speed_multiplier, fuel_emissions_multiplier, or fuel_glow_color are used. Amount of energy it gives when used as fuel.
		/// </summary>
		[Description("Default: \"0J\"\r\n" +
		"Mandatory when fuel_acceleration_multiplier, fuel_top_speed_multiplier, fuel_emissions_multiplier, or fuel_glow_color are used. Amount of energy it gives when used as fuel.")]
		[Optional]
		[Category("Optional")]
		public string fuel_value
		{
			get
			{
				return _fuel_value;
			}

			set
			{
				_fuel_value = value;
			}
		}

		/// <summary>
		///Default: 1.0
		/// </summary>
		[Description("Default: 1.0")]
		[Optional]
		[Category("Optional")]
		public double fuel_acceleration_multiplier
		{
			get
			{
				return _fuel_acceleration_multiplier;
			}

			set
			{
				_fuel_acceleration_multiplier = value;
			}
		}

		/// <summary>
		///Default: 1.0
		/// </summary>
		[Description("Default: 1.0")]
		[Optional]
		[Category("Optional")]
		public double fuel_top_speed_multiplier
		{
			get
			{
				return _fuel_top_speed_multiplier;
			}

			set
			{
				_fuel_top_speed_multiplier = value;
			}
		}

		/// <summary>
		///Default: 1.0
		/// </summary>
		[Description("Default: 1.0")]
		[Optional]
		[Category("Optional")]
		public double fuel_emissions_multiplier
		{
			get
			{
				return _fuel_emissions_multiplier;
			}

			set
			{
				_fuel_emissions_multiplier = value;
			}
		}

		/// <summary>
		///Default: {r=0, g=0, b=0, a=1}
		///Colors the glow of the burner energy source when this fuel is burned.
		/// </summary>
		[Description("Default: {r=0, g=0, b=0, a=1}\r\n" +
		"Colors the glow of the burner energy source when this fuel is burned.")]
		[Optional]
		[Category("Optional")]
		[ExpandableObject]
		public Color fuel_glow_color
		{
			get
			{
				return _fuel_glow_color;
			}

			set
			{
				_fuel_glow_color = value;
			}
		}
	}
}
