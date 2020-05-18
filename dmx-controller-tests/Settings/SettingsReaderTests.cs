using NUnit.Framework;
using System.IO;
using System.Reflection;
using Fixtures;
using System.Linq;
using Settings;

namespace Settings {
	[TestFixture]
	public class SettingsReaderTests {

		private static readonly IFixture
			adjMegaPar = new AdjMegaPar(),
			silverPar = new SilverParCan(),
			jellyfish = new Jellyfish();

		private static readonly string[] empty = new string[0];

		private static readonly SettingsLine[] m_expectedSettings = {
				new SettingsLine(1, 1, new FixtureConfig[] {
					new FixtureConfig(adjMegaPar, ColourCode.Red, empty),
					new FixtureConfig(adjMegaPar, ColourCode.Green, empty),
					new FixtureConfig(silverPar, ColourCode.Blue, empty),
					new FixtureConfig(jellyfish, ColourCode.SoundActive, empty),
				}),
				new SettingsLine(1, 9, new FixtureConfig[] {
					new FixtureConfig(adjMegaPar, ColourCode.Green, empty),
					new FixtureConfig(adjMegaPar, ColourCode.Red, empty),
					new FixtureConfig(silverPar, ColourCode.Green, empty),
					new FixtureConfig(jellyfish, ColourCode.Off, empty),
				}),
				new SettingsLine(7, 5, new FixtureConfig[] {
					new FixtureConfig(adjMegaPar, ColourCode.SoundActive, new string[]{ "12" }),
					new FixtureConfig(adjMegaPar, ColourCode.SoundActive, new string[]{ "8" }),
					new FixtureConfig(silverPar, ColourCode.Orange, empty),
					new FixtureConfig(jellyfish, ColourCode.SoundActive, empty),
				}),
				new SettingsLine(7, 10, new FixtureConfig[] {
					new FixtureConfig(adjMegaPar, ColourCode.SoundActive, new string[]{ "5" }),
					new FixtureConfig(adjMegaPar, ColourCode.SoundActive, new string[]{ "8" }),
					new FixtureConfig(silverPar, ColourCode.Aqua, empty),
					new FixtureConfig(jellyfish, ColourCode.SoundActive, empty),
				})
			};

		[Test]
		public void ReadSettingsFile() {
			SettingsLine[] lines;

			Assembly assembly = Assembly.GetExecutingAssembly();
			using(Stream stream = assembly.GetManifestResourceStream("testData.settings.csv"))
			using(StreamReader reader = new StreamReader(stream)) {
				lines = SettingsReader.ReadSettingsFile(reader).ToArray();
			}

			Assert.AreEqual(m_expectedSettings.Length, lines.Length, "Incorrect number of lines");
			for(int i = 0; i < m_expectedSettings.Length; i++) {
				Assert.AreEqual(m_expectedSettings[i], lines[i], $"Incorrect line at index {i}");
			}
		}
	}
}
