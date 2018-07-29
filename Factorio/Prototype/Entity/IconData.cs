using System;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Factorio.Prototype.Entity
{
	/// <summary>
	/// Data of one icon "layer" for the icons property of the Types/IconSpecification https://wiki.factorio.com/Types/IconSpecification.
	/// </summary>
	/// <seealso cref="Factorio.Prototype.ILUAType" />
	public class IconData : ILUAType
	{
		private string _icon;

		/// <summary>
		/// The path to the icon. Slash (/) is always used as directory delimiter.
		/// </summary>
		/// <value>
		/// The icon.
		/// </value>
		public string icon
		{
			get
			{
				return _icon;
			}
			set
			{
				_icon = value.Replace("\\", "/");
			}
		}

		/// <summary>
		/// Mandatory if icon_size is not specified outside of icons. The size of the square icon, in pixels, e.g. 32 for a 32px by 32px icon.
		/// 
		/// </summary>
		/// <value>
		/// The size of the icon.
		/// </value>
		public Int16 icon_size
		{
			get;
			set;
		}

		[ExpandableObject]
		/// <summary>
		/// Tint of the icon.
		/// Default: {r=0, g=0, b=0, a=1}
		/// </summary>
		/// <value>
		/// The tint.
		/// </value>
		public Color tint
		{
			get;
			set;
		} = new Color();


		[ExpandableObject]
		/// <summary>
		/// Used to offset the icon "layer" from the overall icon.
		/// Default: {0, 0}
		/// </summary>
		/// <value>
		/// The shift.
		/// </value>
		public Vector shift
		{
			get;
			set;
		} = new Vector();

		/// <summary>
		/// Values different than 1 specify the scale of the icon on default gui scale. Scale 2 means that the icon will be 2 times bigger on screen (and more pixelated).
		/// Default: 1
		/// </summary>
		/// <value>
		/// The scale.
		/// </value>
		public float scale
		{
			get;set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IconData"/> struct.
		/// </summary>
		public IconData()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IconData"/> struct.
		/// </summary>
		/// <param name="iconPath">The path to the icon. Slash (/) is always used as directory delimiter.</param>
		/// <param name="size">Mandatory if icon_size is not specified outside of icons. The size of the square icon, in pixels, e.g. 32 for a 32px by 32px icon.</param>
		/// <param name="tint"> Tint of the icon. Default: {r=0, g=0, b=0, a=1}</param>
		/// <param name="shift">Used to offset the icon "layer" from the overall icon. Default: {0, 0}</param>
		/// <param name="scale">/// Values different than 1 specify the scale of the icon on default gui scale. Scale 2 means that the icon will be 2 times bigger on screen (and more pixelated). Default: 1</param>
		public IconData(string iconPath, Int16 size, Color tint = null, Vector shift = null, float scale = 1)
		{
			_icon = iconPath.Replace("\\", "/");
			icon_size = size;
			this.tint = tint == null ? new Color() : tint;
			this.shift = shift == null ? new Vector() : shift;
			this.scale = scale;
		}

		public string GetLuaValue()
		{
			return "\t\t\t\t{\r\n" +
				"\t\t\t\t\ticon = \"" + icon + "\",\r\n" +
				"\t\t\t\t\ticon_size = " + icon_size.ToString() + ",\r\n" +
				"\t\t\t\t\ttint = " + tint.GetLuaValue() + ",\r\n" +
				"\t\t\t\t\tshift = " + shift.GetLuaValue() + ",\r\n" +
				"\t\t\t\t\tscale = " + scale.ToString() + "\r\n\t\t\t\t}";
		}
	}
}
