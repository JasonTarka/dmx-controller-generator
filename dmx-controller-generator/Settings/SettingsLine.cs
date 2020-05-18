using System.Linq;

namespace Settings {
	public class SettingsLine {
		public SettingsLine(
			byte scene,
			byte bank,
			FixtureConfig[] fixtures
		) {
			Scene = scene;
			Bank = bank;
			Fixtures = fixtures;
		}

		public byte Scene { get; }
		public byte Bank { get; }
		public FixtureConfig[] Fixtures { get; }

		public override bool Equals(object obj) {
			return obj is SettingsLine other
				&& this.ToString() == other.ToString();
		}

		public override string ToString() {
			string fixtureStrings = string.Join(
					"\n",
					Fixtures.Select(
						x => $"\t{x?.ToString()}"
					).ToArray()
				);
			return $"{{Scene: {Scene}; Bank: {Bank}; Fixtures:\n{fixtureStrings}\n}}";
		}
	}
}
