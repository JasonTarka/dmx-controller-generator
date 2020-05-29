using NUnit.Framework;
using System.IO;
using System.Reflection;

namespace Chases {
	[TestFixture]
	public class ChaseTests {
		[Test]
		public void GenerateOutput() {
			Chase chase = new Chase(1);

			// The test data was pulled from a chase that
			// included scene 3, banks 1-30.
			for(byte i = 1; i <= 30; i++) {
				chase.AddScene(3, i);
			}

			using(Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("testData.chaseWithScenes"))
			using(BinaryReader reader = new BinaryReader(stream)) {
				byte[] expectedOutput = reader.ReadBytes(0x100);

				byte[] output = chase.GenerateOutput();
				CollectionAssert.AreEqual(expectedOutput, output);
			}
		}
	}
}
