using System.Collections.Generic;
using System.ComponentModel;

namespace Factorio.Prototype.Entity.EntityWithHealth.CraftingMachine
{
	/// <summary>
	/// An assembling machine - like the assembling machines 1/2/3 in the game, but you can use your own recipe categories.
	/// </summary>
	/// <seealso cref="Factorio.Prototype.Entity.EntityWithHealth.CraftingMachine.CraftingMachine" />
	public sealed class AssemblingMachine : CraftingMachine
	{
		private byte _ingredient_count = 1;
		private string _fixed_recipe = "";

		public AssemblingMachine() : base()
		{
			type = "assembling-machine";
		}

		/// <summary>
		/// Sets the maximum number of ingredients this machine can craft with. Any recipe with more ingredients than this will be unavailable in this machine.
		/// This only counts item ingredients, not fluid ingredients! This means if ingredient count is 2, and the recipe has 2 item ingredients and 1 fluid ingredient, it can still be crafted in the machine.
		/// </summary>
		/// <value>
		/// The ingredient count.
		/// </value>
		[Category("Mandatory")]
		[Description("Sets the maximum number of ingredients this machine can craft with. Any recipe with more ingredients than this will be unavailable in this machine.\r\n"
			+ "This only counts item ingredients, not fluid ingredients! This means if ingredient count is 2, and the recipe has 2 item ingredients and 1 fluid ingredient, it can still be crafted in the machine.")]
		[Mandatory]
		public byte ingredient_count
		{
			get
			{
				return _ingredient_count;
			}

			set
			{
				_ingredient_count = value;
			}
		}

		/// <summary>
		/// The preset recipe of this machine. This machine does not show a recipe selection if this is set. The base game uses this for the rocket silo.
		/// </summary>
		/// <value>
		/// The fixed recipe.
		/// </value>
		[Category("Optional")]
		[Description("The preset recipe of this machine. This machine does not show a recipe selection if this is set. The base game uses this for the rocket silo.")]
		[Optional]
		public string fixed_recipe
		{
			get
			{
				return _fixed_recipe;
			}

			set
			{
				_fixed_recipe = value;
			}
		}
	}
}
