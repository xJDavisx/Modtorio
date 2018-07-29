using System.Collections.Generic;

namespace Factorio.Prototype.Entity
{
	/// <summary>
	/// The collision mask is specified by list of string, every input is specification of one layer the object collides with.
	/// Layers are:
	///ground-tile,
	///water-tile,
	///resource-layer,
	///doodad-layer,
	///floor-layer,
	///item-layer,
	///ghost-layer,
	///object-layer,
	///player-layer,
	///train-layer,
	///layer-11,
	///layer-12,
	///layer-13,
	///layer-14, and
	///layer-15
	/// </summary>
	/// <seealso cref="Factorio.Prototype.ILUAType" />
	public class CollisionMask : ILUAType
	{
		/// <summary>
		/// Gets or sets the collision masks.
		/// </summary>
		/// <value>
		/// The collision mask.
		/// </value>
		public List<string> masks
		{
			get; set;
		}

		public enum CollisionMasks
		{
			item_layer,
			object_layer,
			player_layer,
			water_tile
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CollisionMask"/> struct.
		/// 
		/// </summary>
		/// <param name="mask">The mask. If left null, the collision_mask property defaults to item-layer, object-layer, player-layer, and water-tile</param>
		public CollisionMask(List<string> mask = null)
		{
			masks = mask;
			if (masks == null)
			{
				masks = new List<string>()
				{
					"item-layer",
					"object-layer",
					"player-layer",
					"water-tile"
				};
			}
		}

		public override string ToString()
		{
			string s = "Collision Mask: ";
			s += GetLuaValue();
			return s;
		}

		public string GetLuaValue()
		{
			string c = "{";

			for (int i = 0; i < masks.Count; i++)
			{
				if (i > 0)
				{
					c += ", ";
				}
				c += "\"" + masks[i] + "\"";
			}
			c += "}";
			return c;
		}
	}
}
