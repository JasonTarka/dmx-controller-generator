using NUnit.Framework;
using System;

namespace Fixtures {
	[TestFixture]
	public class AdjMegaParTests {

		[TestCase(
			ColourCode.SoundActive,
			new string[] { "12", "3" },
			//							  1  2  3  4    5    6    7    8   9
			ExpectedResult = new byte[] { 0, 0, 0, 0, 255, 255, 255, 177, 76, 0, 0, 0, 0, 0, 0, 0 },
			TestName = "Sound active, mode 12, sensitivity 3/10"
		)]
		[TestCase(
			ColourCode.SoundActive,
			new string[] { "1", "10" },
			//							  1  2  3  4    5    6    7   8    9
			ExpectedResult = new byte[] { 0, 0, 0, 0, 255, 255, 255, 1, 251, 0, 0, 0, 0, 0, 0, 0 },
			TestName = "Sound active, mode 1, sensitivity 10/10"
		)]
		[TestCase(
			ColourCode.Macro,
			new string[] { "change", "9", "1" },
			//							  1  2  3  4    5    6    7    8   9
			ExpectedResult = new byte[] { 0, 0, 0, 0, 255, 255, 104, 129, 26, 0, 0, 0, 0, 0, 0, 0 },
			TestName = "Colour change, mode 9, speed 1/10"
		)]
		[TestCase(
			ColourCode.Macro,
			new string[] { "fade", "16", "5" },
			//							  1  2  3  4    5    6    7    8    9
			ExpectedResult = new byte[] { 0, 0, 0, 0, 255, 255, 155, 241, 126, 0, 0, 0, 0, 0, 0, 0 },
			TestName = "Colour fade, mode 16, speed 5/10"
		)]
		public byte[] GetChannelValues_SpecialValues(
			ColourCode colour,
			string[] parameters
		) {
			IFixture fixture = new AdjMegaPar();
			return fixture.GetChannelValues(colour, parameters);
		}
	}
}
