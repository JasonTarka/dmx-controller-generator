using System.IO;
using System.Reflection;
using NUnit.Framework;
using System.Linq;

namespace Chases.Settings {
	[TestFixture]
	public class SettingsReaderTests {

		private static readonly Chase[] m_expectedSettings = {
			new Chase(1)
				.AddScene(4,1)
				.AddScene(4,2)
				.AddScene(4,3),
			new Chase(3)
				.AddScene(6,18)
				.AddScene(8,2)
		};

		[Test]
		public void ReadSettingsFile() {
			Chase[] chases;

			Assembly assembly = Assembly.GetExecutingAssembly();
			using(Stream stream = assembly.GetManifestResourceStream("testData.chaseSettings.csv"))
			using(StreamReader reader = new StreamReader(stream)) {
				chases = SettingsReader.ReadSettingsFile(reader).ToArray();
			}

			Assert.AreEqual(m_expectedSettings.Length, chases.Length, "Incorrect number of lines");
			for(int i = 0; i < m_expectedSettings.Length; i++) {
				Assert.AreEqual(m_expectedSettings[i], chases[i], $"Incorrect line at index {i}");
			}
		}
	}
}
