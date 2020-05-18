using System.Collections.Generic;
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

		public override int GetHashCode() {
			var hashCode = 1133551653;
			hashCode = hashCode * -1521134295 + Scene.GetHashCode();
			hashCode = hashCode * -1521134295 + Bank.GetHashCode();
			hashCode = hashCode * -1521134295 + EqualityComparer<FixtureConfig[]>.Default.GetHashCode(Fixtures);
			return hashCode;
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
