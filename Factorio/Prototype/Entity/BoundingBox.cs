using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace Factorio.Prototype.Entity
{
	/// <summary>
	/// BoundingBox is set of two <seealso cref="Factorio.Prototype.Position" />s. BoundingBoxes are typically centered around the position of an entity.
	/// It is specified like this: {{-0.4, -0.4}, {0.4, 0.4}}
	/// The first position is assumed to be leftTop, the second position is assumed to be rightBottom.
	/// </summary>
	/// <seealso cref="Factorio.Prototype.ILUAType" />
	public class BoundingBox : ILUAType
	{
		[ExpandableObject]
		public Position TopLeft{get;set;}

		[ExpandableObject]
		public Position BottomRight
		{
			get; set;
		}

		public BoundingBox(Position topLeft, Position bottomRight)
		{
			TopLeft = topLeft == null ? new Position() : topLeft;
			BottomRight = bottomRight == null ? new Position() : bottomRight;
		}

		public BoundingBox()
		{
			TopLeft = new Position();
			BottomRight = new Position();
		}
		public override string ToString()
		{
			string s = "Bounding Box: ";
			s += GetLuaValue();
			return s;
		}

		public string GetLuaValue()
		{
			return "{" + TopLeft.GetLuaValue() + ", " + BottomRight.GetLuaValue() + "}";
		}
	}
}
