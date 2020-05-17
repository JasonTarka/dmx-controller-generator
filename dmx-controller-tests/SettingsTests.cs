using NUnit.Framework;
using dmxcontrollergenerator;
using System.IO;
using System.Reflection;
using Fixtures;
using System.Linq;

namespace dmxcontrollertests {
	[TestFixture]
	public class SettingsTests {

		private static readonly IFixture
			adjMegaPar = new AdjMegaPar(),
			silverPar = new SilverParCan(),
			jellyfish = new Jellyfish();

		private static readonly string[] empty = new string[0];

		private static readonly Settings.SettingsLine[] m_expectedSettings = {
				new Settings.SettingsLine(1, 1, new Settings.FixtureConfig[] {
					new Settings.FixtureConfig(adjMegaPar, ColourCode.Red, empty),
					new Settings.FixtureConfig(adjMegaPar, ColourCode.Green, empty),
					new Settings.FixtureConfig(silverPar, ColourCode.Blue, empty),
					new Settings.FixtureConfig(jellyfish, ColourCode.SoundActive, empty),
				}),
				new Settings.SettingsLine(1, 9, new Settings.FixtureConfig[] {
					new Settings.FixtureConfig(adjMegaPar, ColourCode.Green, empty),
					new Settings.FixtureConfig(adjMegaPar, ColourCode.Red, empty),
					new Settings.FixtureConfig(silverPar, ColourCode.Green, empty),
					new Settings.FixtureConfig(jellyfish, ColourCode.Off, empty),
				}),
				new Settings.SettingsLine(7, 5, new Settings.FixtureConfig[] {
					new Settings.FixtureConfig(adjMegaPar, ColourCode.SoundActive, new string[]{ "12" }),
					new Settings.FixtureConfig(adjMegaPar, ColourCode.SoundActive, new string[]{ "8" }),
					new Settings.FixtureConfig(silverPar, ColourCode.Orange, empty),
					new Settings.FixtureConfig(jellyfish, ColourCode.SoundActive, empty),
				}),
				new Settings.SettingsLine(7, 10, new Settings.FixtureConfig[] {
					new Settings.FixtureConfig(adjMegaPar, ColourCode.SoundActive, new string[]{ "5" }),
					new Settings.FixtureConfig(adjMegaPar, ColourCode.SoundActive, new string[]{ "8" }),
					new Settings.FixtureConfig(silverPar, ColourCode.Aqua, empty),
					new Settings.FixtureConfig(jellyfish, ColourCode.SoundActive, empty),
				})
			};

		[Test]
		public void ReadSettingsFile() {
			Settings.SettingsLine[] lines;

			Assembly assembly = Assembly.GetExecutingAssembly();
			using(Stream stream = assembly.GetManifestResourceStream("testData.settings.csv"))
			using(StreamReader reader = new StreamReader(stream)) {
				lines = Settings.ReadSettingsFile(reader).ToArray();
			}

			Assert.AreEqual(m_expectedSettings.Length, lines.Length, "Incorrect number of lines");
			for(int i = 0; i < m_expectedSettings.Length; i++) {
				Assert.AreEqual(m_expectedSettings[i], lines[i], $"Incorrect line at index {i}");
			}
		}
	}
}
