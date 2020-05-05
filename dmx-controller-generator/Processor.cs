using System;
using System.Collections.Generic;
using System.IO;
using Fixtures;

namespace dmxcontrollergenerator {
	public class Processor : IDisposable {
		private readonly string m_fixtureName;
		private readonly string m_settingsFilePath;
		private readonly string m_proFilePath;

		private FileStream m_settingsFileStream;
		private FileStream m_proFileStream;

		private const char SettingsDivider = ',';
		private const char CommentChar = '#'; // Must be at the beginning of the line.

		private FileStream SettingsFile {
			get {
				if(m_settingsFileStream == null) {
					m_settingsFileStream = new FileStream(
							m_settingsFilePath,
							FileMode.Open,
							FileAccess.Read
						);
				}
				return m_settingsFileStream;
			}
		}

		private FileStream ProFile {
			get {
				if(m_proFileStream == null) {
					m_proFileStream = new FileStream(
							m_proFilePath,
							FileMode.Open,
							FileAccess.ReadWrite
						);
				}
				return m_proFileStream;
			}
		}

		public Processor(
			string fixtureName,
			string settingsFilePath,
			string proFilePath
		) {
			m_fixtureName = fixtureName;
			m_settingsFilePath = settingsFilePath;
			m_proFilePath = proFilePath;
		}

		public void ProcessFiles() {
			IEnumerable<SceneBank> sceneBanks = ReadSettingsFile();
			byte[] outputData = ProFileHandler.GenerateFileContents(sceneBanks, ProFile);
			WriteProFile(outputData);
		}

		private byte[] ReadProFile() {
			using(BinaryReader reader = new BinaryReader(ProFile)) {
				return reader.ReadBytes(Constants.ProFileLength);
			}
		}

		private void WriteProFile(byte[] data) {
			ProFile.Seek(0, SeekOrigin.Begin);
			using(BinaryWriter writer = new BinaryWriter(ProFile)) {
				writer.Write(data);
			}
		}

		private IEnumerable<SceneBank> ReadSettingsFile() {
			const int requiredChunks = 3,
				maxChunks = requiredChunks + 1;

			using(StreamReader reader = new StreamReader(SettingsFile)) {
				while(true) {
					string line = reader.ReadLine();
					if(line == null)
						break;

					if(string.IsNullOrWhiteSpace(line)
						|| line[0] == CommentChar)
						continue;

					// Split the string in 2.
					// First half is colour code (if valid); second is additioanl settings.
					string[] chunks = line.Split(new char[] { SettingsDivider }, maxChunks);

					if(chunks.Length < requiredChunks)
						throw new InvalidDataException(
								"Line must contain scene, bank, colour code, and optional settings: " + line
							);

					// Validate the components
					if(!byte.TryParse(chunks[0], out byte scene)
						|| scene < 1 || scene > Constants.NumScenes)
						throw new InvalidDataException("Invalid scene: " + chunks[0]);

					if(!byte.TryParse(chunks[1], out byte bank)
						|| bank < 1 || bank > Constants.NumBanks)
						throw new InvalidDataException("Invalid bank: " + chunks[1]);

					if(!Enum.TryParse(chunks[2], out ColourCode colour))
						throw new InvalidDataException("Invalid colour code: " + chunks[0]);


					// yield return a SceneBank
					string settings = chunks.Length > requiredChunks ? chunks[requiredChunks] : null;
					byte[] channels = GetChannels(colour, settings);
					yield return new SceneBank(scene, bank)
						.SetChannels(channels);
				}
			}
		}

		private byte[] GetChannels(ColourCode colour, string settings) {
			foreach(IFixture fixture in Constants.Fixtures) {
				// TODO: Add passing of settings
				if(fixture.TryGetChannelValues(m_fixtureName, colour, out byte[] channels))
					return channels;
			}

			// No matching fixtures
			throw new InvalidDataException("Invalid fixture: " + m_fixtureName);
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing) {
			if(!disposedValue) {
				if(disposing) {
					m_proFileStream.Dispose();
					m_settingsFileStream.Dispose();
				}

				disposedValue = true;
			}
		}

		public void Dispose() {
			Dispose(true);
		}
		#endregion
	}
}
