using System.Collections.Generic;
using Fixtures;

namespace Settings {
	public class FixtureConfig {

		public FixtureConfig(
			IFixture fixture,
			ColourCode colour,
			string[] settings
		) {
			Fixture = fixture;
			Colour = colour;
			Settings = settings;
		}

		public IFixture Fixture { get; }
		public ColourCode Colour { get; }
		public string[] Settings { get; }

		public override bool Equals(object obj) {
			return obj is FixtureConfig other
				&& this.ToString() == other.ToString();
		}

		public override int GetHashCode() {
			var hashCode = 1296990655;
			hashCode = hashCode * -1521134295 + EqualityComparer<IFixture>.Default.GetHashCode(Fixture);
			hashCode = hashCode * -1521134295 + Colour.GetHashCode();
			hashCode = hashCode * -1521134295 + EqualityComparer<string[]>.Default.GetHashCode(Settings);
			return hashCode;
		}

		public override string ToString() {
			string settings = string.Join(", ", Settings);

			return $"{{{Fixture.FixtureName} - {Colour} - [{settings}]}}";
		}
	}
}
