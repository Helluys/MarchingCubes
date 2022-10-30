using System;
using System.Collections.Generic;

using Syulleh.Math;

using UnityEngine;


namespace Syulleh.MarchingCubes.Unity {
	public class Field3D : MonoBehaviour, ISerializationCallbackReceiver {
		private Field3D<float> field;
		public Field3D<float> Field { get => field; set => field = value; }

		#region debugging
		private void OnDrawGizmosSelected () {
			if (Field == null)
				return;

			for (int x = 0; x < Field.Size.x; x++) {
				for (int y = 0; y < Field.Size.y; y++) {
					for (int z = 0; z < Field.Size.z; z++) {
						Gizmos.color = Field[x, y, z] * Color.green + (1 - Field[x, y, z]) * Color.red;
						Gizmos.DrawSphere(transform.TransformPoint(new Vector3(x, y, z)), .1f);
					}
				}
			}
		}
		#endregion debugging

		#region serialization
		[Serializable]
		private class SerializableField {
			public int sizeX, sizeY, sizeZ;
			public List<float> values;
		}
		[SerializeField, HideInInspector] SerializableField serializableField;

		public void OnBeforeSerialize () {
			serializableField = new SerializableField() {
				sizeX = field.Size.x,
				sizeY = field.Size.y,
				sizeZ = field.Size.z,
				values = new List<float>()
			};
			field.ForEach(v => serializableField.values.Add(v.Value));
		}

		public void OnAfterDeserialize () {
			int index = 0;
			float[,,] values = new float[serializableField.sizeX, serializableField.sizeY, serializableField.sizeZ];
			for (int x = 0; x < serializableField.sizeX; x++) {
				for (int y = 0; y < serializableField.sizeY; y++) {
					for (int z = 0; z < serializableField.sizeZ; z++) {
						values[x, y, z] = serializableField.values[index++];
					}
				}
			}
			field = new Field3D<float>(serializableField.sizeX, serializableField.sizeY, serializableField.sizeZ,
										(x, y, z) => values[x, y, z]);
		}
		#endregion serialization
	}
}
