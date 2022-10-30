using UnityEditor;

using UnityEngine;

namespace Syulleh.MarchingCubes.Unity {
	public class MarchingCubesWizard : ScriptableWizard {
		public float threshold;
		public Material material;
		public Field3D field;

		[MenuItem("Syulleh/Marching Cubes")]
		static void DisplayMCWizard () {
			DisplayWizard<MarchingCubesWizard>("Marching Cubes");
		}

		private void OnWizardCreate () {
			MarchingCubes.Create(field.Field, threshold, material);
		}
	}
}
