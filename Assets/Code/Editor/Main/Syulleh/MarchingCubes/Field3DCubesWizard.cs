using UnityEditor;
using UnityEngine;

namespace Syulleh.MarchingCubes.Unity {
	public class Field3DCubesWizard : ScriptableWizard {
		public MarchingCubesParameters parameters;

		[MenuItem("Syulleh/3D Field")]
		static void Display3DFieldWizard () {
			DisplayWizard<Field3DCubesWizard>("3D Field");
		}

		private void OnWizardCreate () {
			new GameObject("Field 3D").AddComponent<Field3D>().Field =
				MarchingCubes.CreateField(parameters);
		}
	}
}
