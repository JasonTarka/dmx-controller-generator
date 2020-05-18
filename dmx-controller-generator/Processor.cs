using System.Collections.Generic;
using System.IO;
using System;
using Settings;
using System.Linq;

namespace dmxcontrollergenerator {
	public class Processor {

		private readonly string m_settingsFilePath;
		private readonly string m_proFilePath;

		public Processor(
			string settingsFilePath,
			string proFilePath
		) {
			m_settingsFilePath = settingsFilePath;
			m_proFilePath = proFilePath;
		}

		public void ProcessFiles() {
			using(FileStream settingsFile = new FileStream(m_settingsFilePath, FileMode.Open))
			using(StreamReader settingsReader = new StreamReader(settingsFile)) {
				Console.WriteLine($"Reading settings file: {m_settingsFilePath}");
				IEnumerable<SettingsLine> lines = SettingsReader.ReadSettingsFile(settingsReader);

				IEnumerable<SceneBank> sceneBanks = ConvertToSceneBanks(lines);
				UpdateProFile(sceneBanks);
			}
		}

		private IEnumerable<SceneBank> ConvertToSceneBanks(
			IEnumerable<SettingsLine> lines
		) => lines.Select(line => {
				SceneBank sbank = new SceneBank(line.Scene, line.Bank);
				line.Fixtures.ForEach(fix => {
					byte[] channels = fix.Fixture.GetChannelValues(fix.Colour, fix.Settings);
					sbank.AddChannels(channels);
				});
				return sbank;
			});

		private void UpdateProFile(
			IEnumerable<SceneBank> sceneBanks
		) {
			if(!File.Exists(m_proFilePath)) throw new FileNotFoundException(
				$"Cannot open {m_proFilePath}: File does not exist!");

			Console.WriteLine($"Updating {m_proFilePath}");

			byte[] contents;
			using(FileStream stream = File.OpenRead(m_proFilePath)) {
				contents = ProFileHandler.GenerateFileContents(sceneBanks, stream);
			}

			using(BinaryWriter writer = new BinaryWriter(File.OpenWrite(m_proFilePath))) {
				writer.Write(contents);
			}
		}

	}
}
