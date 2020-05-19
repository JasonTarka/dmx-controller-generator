using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Generator;
using Scenes.Fixtures;

namespace Scenes.Settings {
	public static class SettingsReader {

		private static readonly char[] ColumnDividers = { ',', '\t' };
		private const char SettingsDivider = '|';
		private const char CommentChar = '#'; // Must be at the beginning of the line.

		public static IEnumerable<SettingsLine> ReadSettingsFile(
			StreamReader settingsFile
		) {
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
					string[] headers = line.Split(ColumnDividers);
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
				string[] columns = line.Split(ColumnDividers);
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
	}
}
