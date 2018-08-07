using System.Collections.Generic;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Factorio.Prototype.Entity.EntityWithHealth
{
	/// <summary>
	/// The common properties of all entities with health in the game. Extends Entity
	/// </summary>
	/// <seealso cref="Factorio.Prototype.Entity.Entity" />
	public abstract class EntityWithHealth : Entity
	{
		private float _max_health = 10;
		private float _healing_per_tick = 0;
		private float _repair_speed_modifier = 1;
		private float _flammability = 0;
		private string _dying_explosion = "";
		private List<Loot> _loot = new List<Loot>();
		private List<Resistance> _resistances = new List<Resistance>();
		//TODO: private AttackReaction _attack_reaction : default is empty, so no rush. Info https://wiki.factorio.com/Types/AttackReaction
		private Sound _repair_sound = new Sound();
		private bool _alert_when_damaged = true;
		private bool _hide_resistances = true;
		private string _corpse = "";
		//TODO: private ?Sprite4Way? _integration_patch : info not complete.

		public EntityWithHealth():base()
		{

		}

		[Category("Optional")]
		[Description("")]
		[Optional]
		public float max_health
		{
			get
			{
				return _max_health;
			}

			set
			{
				_max_health = value;
			}
		}

		[Category("Optional")]
		[Description("")]
		[Optional]
		public float healing_per_tick
		{
			get
			{
				return _healing_per_tick;
			}

			set
			{
				_healing_per_tick = value;
			}
		}

		[Category("Optional")]
		[Description("")]
		[Optional]
		public float repair_speed_modifier
		{
			get
			{
				return _repair_speed_modifier;
			}

			set
			{
				_repair_speed_modifier = value;
			}
		}

		[Category("Optional")]
		[Description("")]
		[Optional]
		public float flammability
		{
			get
			{
				return _flammability;
			}

			set
			{
				_flammability = value;
			}
		}

		[Category("Optional")]
		[Description("")]
		[Optional]
		public string dying_explosion
		{
			get
			{
				return _dying_explosion;
			}

			set
			{
				_dying_explosion = value;
			}
		}

		[Category("Optional")]
		[Description("")]
		[Optional]
		public List<Loot> loot
		{
			get
			{
				return _loot;
			}

			set
			{
				_loot = value;
			}
		}

		[Category("Optional")]
		[Description("")]
		[Optional]
		public List<Resistance> resistances
		{
			get
			{
				return _resistances;
			}

			set
			{
				_resistances = value;
			}
		}

		[Category("Optional")]
		[Description("")]
		
		[Optional]
		public Sound repair_sound
		{
			get
			{
				return _repair_sound;
			}

			set
			{
				_repair_sound = value;
			}
		}

		[Category("Optional")]
		[Description("")]
		[Optional]
		public bool alert_when_damaged
		{
			get
			{
				return _alert_when_damaged;
			}

			set
			{
				_alert_when_damaged = value;
			}
		}

		[Category("Optional")]
		[Description("")]
		[Optional]
		public bool hide_resistances
		{
			get
			{
				return _hide_resistances;
			}

			set
			{
				_hide_resistances = value;
			}
		}

		[Category("Optional")]
		[Description("")]
		[Optional]
		public string corpse
		{
			get
			{
				return _corpse;
			}

			set
			{
				_corpse = value;
			}
		}
		//TODO: fill out this class. Info: https://wiki.factorio.com/Prototype/EntityWithHealth
	}
}
