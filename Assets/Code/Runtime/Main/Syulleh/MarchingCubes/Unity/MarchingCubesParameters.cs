using UnityEngine;

namespace Syulleh.MarchingCubes.Unity {
	[CreateAssetMenu(fileName = "Marching cubes parameters", menuName = "Marching cubes/parameters")]
	public class MarchingCubesParameters : ScriptableObject {
		#region properties
		/// <summary>
		/// The size and position of the perlin noise sampling.
		/// </summary>
		public Bounds Bounds { get { return bounds; } set { bounds = value; } }

		/// <summary>
		/// The resolution of the perlin noise sampling, that is the number of sample points along each axis.
		/// </summary>
		public Vector3Int Resolution { get { return resolution; } set { resolution = value; } }

		/// <summary>
		/// The constant term of the field value.
		/// </summary>
		public float FieldConstant { get => fieldConstant; set => fieldConstant = value; }
		/// <summary>
		/// The noise factor of the field value.
		/// </summary>
		public float FieldNoise { get => fieldNoise; set => fieldNoise = value; }

		/// <summary>
		/// The linear factors (for each axis) of the field value.
		/// </summary>
		public Vector3 FieldLinear { get => fieldLinear; set => fieldLinear = value; }

		/// <summary>
		/// The field threshold value where the mesh surface is generated;
		/// </summary>
		public float Threshold { get => threshold; set => threshold = value; }
		#endregion properties

		#region backing fields
		[SerializeField] private Bounds bounds;
		[SerializeField] private Vector3Int resolution;

		[SerializeField] private float fieldConstant;
		[SerializeField] private float fieldNoise;
		[SerializeField] private Vector3 fieldLinear;
		[SerializeField] private float threshold;
		#endregion backing fields
	}
}