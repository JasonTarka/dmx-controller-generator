using dmxcontrollergenerator;
using NUnit.Framework;
using System.IO;

namespace dmxcontrollertests {
	[TestFixture()]
	public class SceneBankTests {
		[TestCase(1, 1, ExpectedResult = 0x300)]
		[TestCase(5, 1, ExpectedResult = 0x700)]
		[TestCase(8, 1, ExpectedResult = 0xA00)]
		[TestCase(1, 2, ExpectedResult = 0xB00)]
		[TestCase(1, 6, ExpectedResult = 0x2B00)]
		[TestCase(2, 6, ExpectedResult = 0x2D00)]
		[TestCase(5, 9, ExpectedResult = 0x4800)]
		[TestCase(3, 11, ExpectedResult = 0x5600)]
		[TestCase(4, 11, ExpectedResult = 0x5780)]
		[TestCase(5, 13, ExpectedResult = 0x6880)]
		[TestCase(8, 30, ExpectedResult = 0xF380)]
		public ushort Offset(byte scene, byte bank) {
			return new SceneBank(scene, bank).Offset;
		}

		[TestCase(new byte[] { 112,1,1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, ExpectedResult = 7)]
		[TestCase(new byte[] { 1, 6, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, ExpectedResult = 23)]
		[TestCase(new byte[] { 1, 1, 5, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1 }, ExpectedResult = 33863)]
		[TestCase(new byte[] { 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, ExpectedResult = 36)]
		[TestCase(new byte[] { 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, ExpectedResult = 24)]
		public ushort Checksum(byte[] channelValues) {
			return new SceneBank(1, 1)
				.SetChannels(channelValues)
				.Checksum;
		}

		[TestCase]
		public void GenerateOutput() {
			// Channel values & expected binary output are taken
			// from one scene-bank from a real export.
			SceneBank sbank = new SceneBank(1, 1)
				.SetChannels(new byte[] {
					30, 40, 30, // 1-3
					0, 0, 0, // 4-6
					40, // 7
					0, 0, 0, // 8-10
					40, // 11
					0, 0, 0, 0, // 12-15
					40 // 16
				});

			using(Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("testData.sixChannelScene"))
			using(BinaryReader reader = new BinaryReader(stream)) {
				byte[] expectedOutput = reader.ReadBytes(0x100);

				byte[] result = sbank.GenerateOutput();

				CollectionAssert.AreEqual(expectedOutput, result);
			}
		}
	}
}
