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

		public override string ToString() {
			string settings = string.Join(", ", Settings);

			return $"{{{Fixture.FixtureName} - {Colour} - [{settings}]}}";
		}
	}
}
