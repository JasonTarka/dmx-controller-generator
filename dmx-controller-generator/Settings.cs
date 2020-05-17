using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fixtures;

namespace dmxcontrollergenerator {
	public static class Settings {

		private const char ColumnDivider = ',';
		private const char SettingsDivider = '|';
		private const char CommentChar = '#'; // Must be at the beginning of the line.

		public static IEnumerable<SettingsLine> ReadSettingsFile(
			StreamReader settingsFile
		) {
			// This method is protected, and a slightly more complicated return than necessary,
			// to make it much easier to test this part in isolation.

			IFixture[] fixtures = null;
			const int ignoredHeaders = 2,
				minHeaders = ignoredHeaders + 1;
			int lineNum = 0,
				numColumns = -1,
				numFixtures = -1;

			while(true) {
				string line = settingsFile.ReadLine();
				if(line == null)
					break;

				lineNum++;
				if(string.IsNullOrWhiteSpace(line)
					|| line[0] == CommentChar)
					continue;

				if(fixtures == null) {
					// Attempt to parse this as a header line naming the fixtures
					string[] headers = line.Split(ColumnDivider);
					numColumns = headers.Length;
					if(numColumns < minHeaders) throw new InvalidDataException(
							"Header line must contain columns for scene, bank, then one or more fixture names"
						);

					numFixtures = numColumns - ignoredHeaders;
					fixtures = new IFixture[numFixtures];
					for(int i = 0; i < fixtures.Length; i++) {
						fixtures[i] = GetFixture(headers[i + ignoredHeaders]);
					}
					continue;
				}

				// Split the string into columns
				string[] columns = line.Split(new char[] { ColumnDivider });
				if(columns.Length != numColumns) throw new InvalidDataException(
					$"Line {lineNum}: Invalid number of columns. Expected: {numColumns}; Actual: {columns.Length}.");

				// Validate the components
				if(!byte.TryParse(columns[0], out byte scene)
					|| scene < 1 || scene > Constants.NumScenes)
					throw new InvalidDataException("Invalid scene: " + columns[0]);

				if(!byte.TryParse(columns[1], out byte bank)
					|| bank < 1 || bank > Constants.NumBanks)
					throw new InvalidDataException("Invalid bank: " + columns[1]);

				// Each additional column needs to be parsed individually
				columns = columns.Skip(2).ToArray();
				FixtureConfig[] configs = new FixtureConfig[numFixtures];

				for(int i = 0; i < columns.Length; i++) {
					if(string.IsNullOrWhiteSpace(columns[i])) throw new InvalidDataException(
						$"Line {lineNum}: Columns cannot be empty.");
					string[] args = columns[i].Split(SettingsDivider);

					if(!Enum.TryParse(args[0], out ColourCode colour)) throw new InvalidDataException(
						$"Line {lineNum}: Invalid colour code: {args[0]}");

					configs[i] = new FixtureConfig(fixtures[i], colour, args.Skip(1).ToArray());
				}

				yield return new SettingsLine(scene, bank, configs);
			}
		}

		private static IFixture GetFixture(string fixtureName) {
			foreach(IFixture attempt in Constants.Fixtures) {
				if(attempt.TryGetFixture(fixtureName, out IFixture fixture)) {
					return fixture;
				}
			}
			throw new InvalidDataException($"Invalid fixture name: {fixtureName}");
		}

		#region Subclasses

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
				SettingsLine other = obj as SettingsLine;

				return other != null
					&& this.ToString() == other.ToString();
			}

			public override string ToString() {
				//string fixtureStrings = "";
				//foreach(FixtureConfig fixture in Fixtures) {
				//	fixtureStrings += $"\n{fixture.ToString()}";
				//}
				string fixtureStrings = string.Join(
						"\n",
						Fixtures.Select(
							x => $"\t{x?.ToString()}"
						).ToArray()
					);
				return $"{{Scene: {Scene}; Bank: {Bank}; Fixtures:\n{fixtureStrings}\n}}";
			}
		}

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
				FixtureConfig other = obj as FixtureConfig;
				return other != null
					&& this.ToString() == other.ToString();
			}

			public override string ToString() {
				string settings = string.Join(", ", Settings);

				return $"{{{Fixture.FixtureName} - {Colour} - [{settings}]}}";
			}
		}

		#endregion
	}
}
