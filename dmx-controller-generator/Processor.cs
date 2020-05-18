using System.Collections.Generic;
using System.IO;
using System;
using Settings;

namespace dmxcontrollergenerator {
	public class Processor {

		private readonly string m_settingsFilePath;
		private readonly string m_proFileDir;

		public Processor(
			string settingsFilePath,
			string proFileDir
		) {
			m_settingsFilePath = settingsFilePath;
			m_proFileDir = proFileDir;
		}

		public void ProcessFiles() {
			using(FileStream settingsFile = new FileStream(m_settingsFilePath, FileMode.Open))
			using(StreamReader settingsReader = new StreamReader(settingsFile)) {
				Console.WriteLine($"Reading settings file: {m_settingsFilePath}");
				IEnumerable<SettingsLine> lines = SettingsReader.ReadSettingsFile(settingsReader);

				IEnumerable<SceneBank>[] sceneBanks = ConvertToSceneBanks(lines);

				for(int i = 0; i < sceneBanks.Length; i++) {
					UpdateProFile(sceneBanks[i], i);
				}
			}
		}

		/// <summary>
		/// Converts to a list of scene banks per fixture.
		/// </summary>
		/// <returns>
		/// List of scene banks, where each element in the main array corresponds to a fixture number.
		/// </returns>
		/// <param name="lines">Lines from the settings file.</param>
		private IEnumerable<SceneBank>[] ConvertToSceneBanks(
			IEnumerable<SettingsLine> lines
		) {
			IList<SceneBank>[] sceneBanks = null;
			int numFixtures = -1;
			foreach(SettingsLine line in lines) {
				FixtureConfig[] fixtures = line.Fixtures;
				if(sceneBanks == null) {
					numFixtures = fixtures.Length;
					sceneBanks = new IList<SceneBank>[numFixtures];
				}

				for(int i = 0; i < numFixtures; i++) {
					if(sceneBanks[i] == null)
						sceneBanks[i] = new List<SceneBank>();

					byte[] channels = fixtures[i].Fixture.GetChannelValues(
							fixtures[i].Colour,
							fixtures[i].Settings
						);
					SceneBank sBank = new SceneBank(line.Scene, line.Bank)
						.SetChannels(channels);
					sceneBanks[i].Add(sBank);
				}
			}
			return sceneBanks;
		}

		private void UpdateProFile(
			IEnumerable<SceneBank> sceneBanks,
			int index
		) {
			string filePath = Path.Combine(
					m_proFileDir,
					string.Format(Constants.ProFilePattern, index + 1)
				);

			if(!File.Exists(filePath)) throw new FileNotFoundException(
				$"Cannot open {filePath}: File does not exist!");

			Console.WriteLine($"Updating {filePath}");

			byte[] contents;
			using(FileStream stream = File.OpenRead(filePath)) {
				contents = ProFileHandler.GenerateFileContents(sceneBanks, stream);
			}

			using(BinaryWriter writer = new BinaryWriter(File.OpenWrite(filePath))) {
				writer.Write(contents);
			}
		}

	}
}
