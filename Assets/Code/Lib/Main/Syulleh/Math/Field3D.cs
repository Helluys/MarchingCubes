using System;

namespace Syulleh.Math {
	/// <summary>
	/// A generic 3D field.
	/// </summary>
	/// <typeparam name="T">the field value type</typeparam>
	[Serializable]
	public class Field3D<T> {
		/// <summary>
		/// A handle to a field value that allows navigating its neighbour values.
		/// </summary>
		public class FieldValue {
			private readonly Field3D<T> field;
			private readonly (uint x, uint y, uint z) coordinates;

			public FieldValue (Field3D<T> field, uint x, uint y, uint z) {
				this.field = field;
				coordinates = (x, y, z);
			}

			public uint X => coordinates.x;
			public uint Y => coordinates.y;
			public uint Z => coordinates.z;

#nullable enable
			public T Value => field[coordinates];
			public FieldValue? Right => GetOrDefault(coordinates.x + 1, coordinates.y, coordinates.z);
			public FieldValue? Left => GetOrDefault(coordinates.x - 1, coordinates.y, coordinates.z);
			public FieldValue? Back => GetOrDefault(coordinates.x, coordinates.y + 1, coordinates.z);
			public FieldValue? Front => GetOrDefault(coordinates.x, coordinates.y - 1, coordinates.z);
			public FieldValue? Top => GetOrDefault(coordinates.x, coordinates.y, coordinates.z + 1);
			public FieldValue? Bottom => GetOrDefault(coordinates.x, coordinates.y, coordinates.z - 1);

			private FieldValue? GetOrDefault (uint x, uint y, uint z) =>
				(x < 0 || x >= field.size.x || y < 0 || y >= field.size.y || z < 0 || z >= field.size.z)
					? default : new FieldValue(field, x, y, z);
#nullable disable
		}

		public delegate void Action (T value);
		public delegate T Evaluator (uint x, uint y, uint z);
		public delegate U Mapper<U> (uint x, uint y, uint z, FieldValue value);

		public readonly (uint x, uint y, uint z) size;
		private readonly T[,,] field;

		public T this[uint x, uint y, uint z] {
			get { return field[x, y, z]; }
			set { field[x, y, z] = value; }
		}

		public T this[(uint x, uint y, uint z) c] {
			get { return field[c.x, c.y, c.z]; }
			set { field[c.x, c.y, c.z] = value; }
		}

		public Field3D (uint sizeX, uint sizeY, uint sizeZ, Evaluator evaluator)
			: this((sizeX, sizeY, sizeZ), evaluator) { }

		public Field3D ((uint x, uint y, uint z) size, Evaluator evaluator) {
			this.size = size;
			this.field = new T[size.x, size.y, size.z];
			for (uint x = 0; x < size.x; x++) {
				for (uint y = 0; y < size.y; y++) {
					for (uint z = 0; z < size.z; z++) {
						field[x, y, z] = evaluator(x, y, z);
					}
				}
			}
		}

		public Field3D<U> Map<U> (Mapper<U> mapper) {
			return new Field3D<U>(size.x, size.y, size.z, (x, y, z) => mapper(x, y, z, new FieldValue(this, x, y, z)));
		}

		public void ForEach (Action action) {
			for (uint x = 0; x < size.x; x++) {
				for (uint y = 0; y < size.y; y++) {
					for (uint z = 0; z < size.z; z++) {
						action(field[x, y, z]);
					}
				}
			}
		}
	}
}
