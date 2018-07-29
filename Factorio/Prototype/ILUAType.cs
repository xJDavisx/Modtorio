namespace Factorio.Prototype
{
	public interface ILUAType
	{
		/// <summary>
		/// Gets the lua code string that represents the value of this type.
		/// </summary>
		/// <returns></returns>
		string GetLuaValue();
	}
}
