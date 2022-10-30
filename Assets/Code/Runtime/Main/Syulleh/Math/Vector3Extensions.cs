using UnityEngine;

namespace Main.Syulleh.Math {
	public static class VectorExt {
		/// <summary>
		/// Returns the element-wise inverse of this vector:
		/// <code>new Vector3(1f / v.x, 1f / v.y, 1f / v.z)</code>
		/// </summary>
		/// <param name="v">this vector</param>
		/// <returns>the element-wise inverse of this vector</returns>
		public static Vector3 Inverse (this Vector3 v) => new(1f / v.x, 1f / v.y, 1f / v.z);

		/// <summary>
		/// Returns the element-wise inverse of this vector:
		/// <code>new Vector3(1f / v.x, 1f / v.y, 1f / v.z)</code>
		/// </summary>
		/// <param name="v">this vector</param>
		/// <returns>the element-wise inverse of this vector</returns>
		public static Vector3 Inverse (this Vector3Int v) => new(1f / v.x, 1f / v.y, 1f / v.z);
	}
}