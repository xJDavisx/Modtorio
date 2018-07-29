using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factorio.Prototype.Entity.EntityWithHealth.CraftingMachine
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="Factorio.Prototype.Entity.EntityWithHealth.EntityWithHealth" />
	public abstract class CraftingMachine : EntityWithHealth
	{
		private string _energy_usage = "";
		private double _crafting_speed = 1;
		private List<string> _crafting_categories = new List<string>();
		public CraftingMachine() : base()
		{
		}

		
		[Category("Mandatory")]
		[Mandatory]
		public string energy_usage
		{
			get
			{
				return _energy_usage;
			}

			set
			{
				_energy_usage = value;
			}
		}


		[Category("Optional")]
		[Optional]
		public double crafting_speed
		{
			get
			{
				return _crafting_speed;
			}

			set
			{
				_crafting_speed = value;
			}
		}


		[Category("Optional")]
		[Mandatory]
		public List<string> crafting_categories
		{
			get
			{
				return _crafting_categories;
			}

			set
			{
				_crafting_categories = value;
			}
		}
		//TODO: fill out this class. Info: https://wiki.factorio.com/Prototype/CraftingMachine
	}
}
