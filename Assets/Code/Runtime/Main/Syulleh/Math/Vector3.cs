namespace Syulleh.Math
{
	public struct Vector3<T>
	{
		public readonly T x, y, z;

		public Vector3(T x, T y, T z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
	}
}