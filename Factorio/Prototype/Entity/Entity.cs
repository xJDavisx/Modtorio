using System.Collections.Generic;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Factorio.Prototype.Entity
{
	/// <summary>
	/// The common properties of all entities in the game. Entity is basically everything that can be on the map (except tiles). For in game script access to entity, take a look at LuaEntity
	/// </summary>
	/// <seealso cref="Factorio.Prototype.Prototype" />
	public abstract class Entity : Prototype
	{
		private List<IconData> _icons = new List<IconData>();
		private BoundingBox _collision_box = new BoundingBox();
		private CollisionMask _collision_mask = new CollisionMask();
		private BoundingBox _selection_box = new BoundingBox();
		private BoundingBox _drawing_box = new BoundingBox();
		private BoundingBox _sticker_box = new BoundingBox();
		private List<EntityPrototypeFlags> _flags = new List<EntityPrototypeFlags>();
		private Minable _minable = new Minable();
		private string _subgroup = "";
		private bool _allow_copy_paste = true;
		private bool _selectable_in_game = true;
		private byte _selection_priority = 50;
		private float _emissions_per_tick = 0;
		private float _shooting_cursor_size = 0;
		//private ??? _created_smoke -- no info on this yet. Web site incomplete.
		//TODO: private AutoplaceSpecification _autoplace https://wiki.factorio.com/Types/AutoplaceSpecification
		//TODO: private WorkingSound _working_sound https://wiki.factorio.com/Types/WorkingSound
		//private ?Trigger? _created_effect -- no info on this yet. Web site incomplete.
		private Sound _build_sound = new Sound();
		private Sound _mined_sound = new Sound();
		private Sound _vehicle_impact_sound = new Sound();
		private Sound _open_sound = new Sound();
		private Sound _close_sound = new Sound();
		private float _build_base_evolution_requirement = 0;
		private Vector _alert_icon_shift = new Vector();
		private float _alert_icon_scale = 1f;
		private string _fast_replaceable_group = "";
		//TODO: placeable_by, remains_when_mined, additional_pastable_entitie, tile_width, tile_height, map_color, friendly_map_color, enemy_map_color
		//https://wiki.factorio.com/Prototype/Entity#icons.2C_icon.2C_icon_size_.28IconSpecification.29
		//TODO: finish all the commenting on the public properties.

		public Entity() : base()
		{

		}

		/// <summary>
		///An icon is mandatory for entities that have at least one of these flags active: placeable-neutral, placeable-player, placeable-enemy.
		///The icon will be used in the editor building selection and the bonus gui.
		/// </summary>
		[Description("An icon is mandatory for entities that have at least one of these flags active: placeable-neutral, placeable-player, placeable-enemy.\r\n" +
		"The icon will be used in the editor building selection and the bonus gui.")]
		[Category("Mandatory")]
		[Mandatory]
		public List<IconData> icons
		{
			get
			{
				return _icons;
			}

			set
			{
				_icons = value;
				Notify("icons");
			}
		}

		/// <summary>
		///Default: Empty={{0, 0}, {0, 0}} it means no collisions.
		///Specification of the entity collision boundaries.
		///Empty collision box is used for smoke, projectiles, particles, explosions etc.
		///The {0,0} coordinate in the collision box will match the entity position.
		///It should be near the center of the collision box, to keep correct entity drawing order. It must include the {0,0} coordinate.
		///Note, that for buildings, it is custom to leave 0.1 wide border between the edge of the tile and the edge of the building, this lets the player move between the building and electric poles/inserters etc.
		/// </summary>
		[Description("Default: Empty={{0, 0}, {0, 0}} it means no collisions.\r\n" +
		"Specification of the entity collision boundaries.\r\n" +
		"Empty collision box is used for smoke, projectiles, particles, explosions etc.\r\n" +
		"The {0,0} coordinate in the collision box will match the entity position.\r\n" +
		"It should be near the center of the collision box, to keep correct entity drawing order. It must include the {0,0} coordinate.\r\n" +
		"Note, that for buildings, it is custom to leave 0.1 wide border between the edge of the tile and the edge of the building, this lets the player move between the building and electric poles/inserters etc.")]
		[Category("Optional")]
		[Optional]
		
		public BoundingBox collision_box
		{
			get
			{
				return _collision_box;
			}

			set
			{
				_collision_box = value;
				Notify("collision_box");
			}
		}

		/// <summary>
		///Default: Depends on entity type, if it is not defined in the entity type, it falls back to: {"item-layer", "object-layer", "player-layer", "water-tile"}
		///Two entities can collide only if they share a layer from the collision mask.
		/// </summary>
		[Description("Default: Depends on entity type, if it is not defined in the entity type, it falls back to: {\"item-layer\", \"object-layer\", \"player-layer\", \"water-tile\"}\r\n" +
		"Two entities can collide only if they share a layer from the collision mask.")]
		[Category("Optional")]
		[Optional]
		
		public CollisionMask collision_mask
		{
			get
			{
				return _collision_mask;
			}

			set
			{
				_collision_mask = value;
				Notify("collision_mask");
			}
		}

		/// <summary>
		///Default: Empty = {{0, 0}, {0, 0}}
		///Specification of the entity selection area. When empty the entity will have no selection area (and thus is not selectable).
		///The selection box is usualy a little bit bigger than the collision box, for tilable entities (like buildings) it should match the tile size of the building.
		/// </summary>
		[Description("Default: Empty = {{0, 0}, {0, 0}}\r\n" +
		"Specification of the entity selection area. When empty the entity will have no selection area (and thus is not selectable).\r\n" +
		"The selection box is usualy a little bit bigger than the collision box, for tilable entities (like buildings) it should match the tile size of the building.")]
		[Category("Optional")]
		[Optional]
		
		public BoundingBox selection_box
		{
			get
			{
				return _selection_box;
			}

			set
			{
				_selection_box = value;
				Notify("selection_box");
			}
		}

		/// <summary>
		///Default: Empty = {{0, 0}, {0, 0}} (selection_box is used instead)
		///Specification of space needed to see the whole entity.
		///This is used to calculate the correct zoom and positioning in the entity info gui.
		/// </summary>
		[Description("Default: Empty = {{0, 0}, {0, 0}} (selection_box is used instead)\r\n" +
		"Specification of space needed to see the whole entity.\r\n" +
		"This is used to calculate the correct zoom and positioning in the entity info gui.")]
		[Category("Optional")]
		[Optional]
		
		public BoundingBox drawing_box
		{
			get
			{
				return _drawing_box;
			}

			set
			{
				_drawing_box = value;
				Notify("drawing_box");
			}
		}

		/// <summary>
		///Default: The value of collision box.
		///Used to set the area of the entity that can have stickers on it, currently only used for units to specify the area where the green slow down stickers can appear. It is optional and the collision box is used when not specified.
		/// </summary>
		[Description("Default: The value of collision box.\r\n" +
		"Used to set the area of the entity that can have stickers on it, currently only used for units to specify the area where the green slow down stickers can appear. It is optional and the collision box is used when not specified.")]
		[Category("Optional")]
		[Optional]
		
		public BoundingBox sticker_box
		{
			get
			{
				return _sticker_box;
			}

			set
			{
				_sticker_box = value;
				Notify("sticker_box");
			}
		}

		/// <summary>
		///Default: nil
		/// </summary>
		[Description("Default: nil")]
		[Category("Optional")]
		[Optional]
		
		public List<EntityPrototypeFlags> flags
		{
			get
			{
				return _flags;
			}

			set
			{
				_flags = value;
				Notify("flags");
			}
		}

		/// <summary>
		///Default: {hardness = 0, minable = false, mining_time = 0}
		/// </summary>
		[Description("Default: {hardness = 0, minable = false, mining_time = 0}")]
		[Category("Optional")]
		[Optional]
		
		public Minable minable
		{
			get
			{
				return _minable;
			}

			set
			{
				_minable = value;
				Notify("minable");
			}
		}

		/// <summary>
		///The name of the subgroup this entity should be sorted into in the map editor building selection.
		/// </summary>
		[Description("The name of the subgroup this entity should be sorted into in the map editor building selection.")]
		[Category("Optional")]
		[Optional]
		public string subgroup
		{
			get
			{
				return _subgroup;
			}

			set
			{
				_subgroup = value;
				Notify("subgroup");
			}
		}


		[Category("Optional")]
		[Optional]
		public bool allow_copy_paste
		{
			get
			{
				return _allow_copy_paste;
			}

			set
			{
				_allow_copy_paste = value;
				Notify("allow_copy_paste");
			}
		}


		[Category("Optional")]
		[Optional]
		public bool selectable_in_game
		{
			get
			{
				return _selectable_in_game;
			}

			set
			{
				_selectable_in_game = value;
				Notify("selectable_in_game");
			}
		}

		/// <summary>
		///The entity with the higher number is selectable before the entity with the lower number.
		/// </summary>
		[Description("The entity with the higher number is selectable before the entity with the lower number.")]
		[Category("Optional")]
		[Optional]
		public byte selection_priority
		{
			get
			{
				return _selection_priority;
			}

			set
			{
				_selection_priority = value;
				Notify("selection_priority");
			}
		}

		/// <summary>
		///Amount of emissions created (positive number) or cleaned (negative number) every tick by the entity. This is passive, and it is independent concept of the emissions of machines, these are created actively depending on the power consumption. Currently used just for trees.
		/// </summary>
		[Description("Amount of emissions created (positive number) or cleaned (negative number) every tick by the entity. This is passive, and it is independent concept of the emissions of machines, these are created actively depending on the power consumption. Currently used just for trees.")]
		[Category("Optional")]
		[Optional]
		public float emissions_per_tick
		{
			get
			{
				return _emissions_per_tick;
			}

			set
			{
				_emissions_per_tick = value;
				Notify("emissions_per_tick");
			}
		}

		/// <summary>
		///The cursor size used when shooting at this entity.
		/// </summary>
		[Description("The cursor size used when shooting at this entity.")]
		[Category("Optional")]
		[Optional]
		public float shooting_cursor_size
		{
			get
			{
				return _shooting_cursor_size;
			}

			set
			{
				_shooting_cursor_size = value;
				Notify("shooting_cursor_size");
			}
		}


		[Category("Optional")]
		[Optional]
		
		public Sound build_sound
		{
			get
			{
				return _build_sound;
			}

			set
			{
				_build_sound = value;
				Notify("build_sound");
			}
		}


		[Category("Optional")]
		[Optional]
		
		public Sound mined_sound
		{
			get
			{
				return _mined_sound;
			}

			set
			{
				_mined_sound = value;
				Notify("mined_sound");
			}
		}


		[Category("Optional")]
		[Optional]
		
		public Sound vehicle_impact_sound
		{
			get
			{
				return _vehicle_impact_sound;
			}

			set
			{
				_vehicle_impact_sound = value;
				Notify("vehicle_impact_sound");
			}
		}


		[Category("Optional")]
		[Optional]
		
		public Sound open_sound
		{
			get
			{
				return _open_sound;
			}

			set
			{
				_open_sound = value;
				Notify("open_sound");
			}
		}


		[Category("Optional")]
		[Optional]
		
		public Sound close_sound
		{
			get
			{
				return _close_sound;
			}

			set
			{
				_close_sound = value;
				Notify("close_sound");
			}
		}


		[Category("Optional")]
		[Optional]
		public float build_base_evolution_requirement
		{
			get
			{
				return _build_base_evolution_requirement;
			}

			set
			{
				_build_base_evolution_requirement = value;
				Notify("build_base_evolution_requirement");
			}
		}


		[Category("Optional")]
		[Optional]
		
		public Vector alert_icon_shift
		{
			get
			{
				return _alert_icon_shift;
			}

			set
			{
				_alert_icon_shift = value;
				Notify("alert_icon_shift");
			}
		}


		[Category("Optional")]
		[Optional]
		public float alert_icon_scale
		{
			get
			{
				return _alert_icon_scale;
			}

			set
			{
				_alert_icon_scale = value;
				Notify("alert_icon_scale");
			}
		}

		/// <summary>
		///This allows you to replace an entity that's already placed, with a different one in your inventory. For example, replacing a burner drill with an electric drill.
		///The ones the game uses are:
		/// "underground-belt"
		/// "loader"
		/// "splitter"
		/// "transport-belt"
		/// "assembling-machine"
		/// "container"
		/// "long-handed-inserter"
		/// "inserter"
		/// "wall"
		/// "rail-signal"
		/// "container"
		/// "pipe"
		/// "furnace"
		/// "steam-engine"
		/// </summary>
		[Description("This allows you to replace an entity that's already placed, with a different one in your inventory. For example, replacing a burner drill with an electric drill.\r\n" +
		"The ones the game uses are:\r\n" +
		" \"underground-belt\"\r\n" +
		" \"loader\"\r\n" +
		" \"splitter\"\r\n" +
		" \"transport-belt\"\r\n" +
		" \"assembling-machine\"\r\n" +
		" \"container\"\r\n" +
		" \"long-handed-inserter\"\r\n" +
		" \"inserter\"\r\n" +
		" \"wall\"\r\n" +
		" \"rail-signal\"\r\n" +
		" \"container\"\r\n" +
		" \"pipe\"\r\n" +
		" \"furnace\"\r\n" +
		" \"steam-engine\"")]
		[Category("Optional")]
		[Optional]
		public string fast_replaceable_group
		{
			get
			{
				return _fast_replaceable_group;
			}

			set
			{
				_fast_replaceable_group = value;
				Notify("fast_replaceable_group");
			}
		}
	}
}
