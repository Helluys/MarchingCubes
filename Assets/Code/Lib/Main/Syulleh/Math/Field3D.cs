using System;
using System.Runtime.Serialization;

namespace Syulleh.Math {
	/// <summary>
	/// A generic 3D field.
	/// </summary>
	/// <typeparam name="T">the field value type</typeparam>
	public class Field3D<T> {
		/// <summary>
		/// A handle to a field value that allows navigating its neighbour values.
		/// </summary>
		public class FieldValue {
			private readonly Field3D<T> field;
			private readonly (int x, int y, int z) coordinates;

			/// <summary>
			/// 
			/// </summary>
			/// <param name="field"></param>
			/// <param name="x"></param>
			/// <param name="y"></param>
			/// <param name="z"></param>
			public FieldValue (Field3D<T> field, int x, int y, int z) {
				this.field = field;
				coordinates = (x, y, z);
			}

			public (int x, int y, int z) Coordinates => coordinates;
			public int X => coordinates.x;
			public int Y => coordinates.y;
			public int Z => coordinates.z;

			public T Value => field[coordinates];
			public FieldValue Right => new(field, coordinates.x + 1, coordinates.y, coordinates.z);
			public FieldValue Left => new(field, coordinates.x - 1, coordinates.y, coordinates.z);
			public FieldValue Back => new(field, coordinates.x, coordinates.y + 1, coordinates.z);
			public FieldValue Front => new(field, coordinates.x, coordinates.y - 1, coordinates.z);
			public FieldValue Top => new(field, coordinates.x, coordinates.y, coordinates.z + 1);
			public FieldValue Bottom => new(field, coordinates.x, coordinates.y, coordinates.z - 1);

			public T SafeValue => ValueOr(default);
			public T ValueOr(T def) => field.Contains(coordinates) ? field[coordinates] : def;
		}

		public delegate void Action (T value);
		public delegate T Evaluator (int x, int y, int z);
		public delegate U Mapper<U> (FieldValue value);

		private readonly T[,,] values;

		/// <summary>
		/// The size of the field. Values are indexed in [0; Size-1] on each coordinate.
		/// </summary>
		public (int x, int y, int z) Size => (values.GetLength(0), values.GetLength(1), values.GetLength(2));

		/// <summary>
		/// Accessor for a value indexed by natural coordinates.
		/// </summary>
		/// <param name="x">the X coordinate</param>
		/// <param name="y">the Y coordinate</param>
		/// <param name="z">the Z coordinate</param>
		/// <returns>the value of the field at location (x,y,z)</returns>
		public T this[int x, int y, int z] {
			get { return values[x, y, z]; }
			set { values[x, y, z] = value; }
		}

		/// <summary>
		/// Accessor for a value indexed by triplet of natural coordinates.
		/// </summary>
		/// <param name="x">the X coordinate</param>
		/// <param name="y">the Y coordinate</param>
		/// <param name="z">the Z coordinate</param>
		/// <returns>the value of the field at location (x,y,z)</returns>
		public T this[(int x, int y, int z) c] {
			get { return values[c.x, c.y, c.z]; }
			set { values[c.x, c.y, c.z] = value; }
		}

		/// <summary>
		/// Constructs a field by evaluating a function in each point.
		/// </summary>
		/// <param name="sizeX">the field X size</param>
		/// <param name="sizeY">the field Y size</param>
		/// <param name="sizeZ">the field Z size</param>
		/// <param name="evaluator">the field evaluation function</param>
		public Field3D (int sizeX, int sizeY, int sizeZ, Evaluator evaluator)
			: this((sizeX, sizeY, sizeZ), evaluator) { }

		/// <summary>
		/// Constructs a field by evaluating a function in each point.
		/// </summary>
		/// <param name="size">the field size</param>
		/// <param name="evaluator">the field evaluation function</param>
		public Field3D ((int x, int y, int z) size, Evaluator evaluator) {
			values = new T[size.x, size.y, size.z];
			for (int x = 0; x < size.x; x++) {
				for (int y = 0; y < size.y; y++) {
					for (int z = 0; z < size.z; z++) {
						values[x, y, z] = evaluator(x, y, z);
					}
				}
			}
		}

		/// <summary>
		/// Applies the given action for each field value.
		/// </summary>
		/// <param name="action">the action</param>
		public void ForEach (Action<FieldValue> action) {
			for (int x = 0; x < Size.x; x++) {
				for (int y = 0; y < Size.y; y++) {
					for (int z = 0; z < Size.z; z++) {
						action(new FieldValue(this, x, y, z));
					}
				}
			}
		}

		/// <summary>
		/// Returns a field of which values are the result of the <paramref name="mapper"/> function applied on this field values.
		/// </summary>
		/// <typeparam name="U">the return field type</typeparam>
		/// <param name="mapper">the field value mapping function</param>
		/// <returns>the mapped field</returns>
		public Field3D<U> Map<U> (Mapper<U> mapper) {
			return new Field3D<U>(Size.x, Size.y, Size.z, (x, y, z) => mapper(new FieldValue(this, x, y, z)));
		}

		public bool Contains ((int x, int y, int z) coordinates) => Contains(coordinates.x, coordinates.y, coordinates.z);
		public bool Contains (int x, int y, int z) =>
				(x >= 0 && x < Size.x && y >= 0 && y < Size.y && z >= 0 && z < Size.z);
	}
}
