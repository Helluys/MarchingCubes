using Syulleh.Math;

using UnityEngine;


namespace Syulleh.MarchingCubes.Unity {
	public class Field3D : MonoBehaviour {
		[SerializeField] private Field3D<float> field;
		public Field3D<float> Field { get => field; set => field = value; }

		private void OnDrawGizmos () {
			if (Field == null)
				return;

			for (uint x = 0; x < Field.size.x; x++) {
				for (uint y = 0; y < Field.size.y; y++) {
					for (uint z = 0; z < Field.size.z; z++) {
						Gizmos.color = Field[x,y,z] > 0 ? Color.green : Color.red;
						Gizmos.DrawSphere(transform.TransformPoint(new Vector3(x, y, z)), Field[x, y, z]);
					}
				}
			}
		}
	}
}
