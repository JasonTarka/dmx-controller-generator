using System;
using dmxcontrollergenerator;

namespace Fixtures {

	/// <summary>
	/// ADJ Jellyfish in basic 3-channel mode.
	/// </summary>
	public class Jellyfish : IFixture {
		public string FixtureName => "Jellyfish";

		private byte Mode = 1,
			ColourOrChaseMode = 2,
			Speed = 3; // Only matters in chase and strobe

		private byte ColourChangeMode = 10,
			StrobeMode = 130,
			SoundActiveMode = 255;

		byte[] IFixture.GetChannelValues(string fixtureName, ColourCode colour) {
			if(fixtureName != FixtureName) return null;

			byte[] channels = new byte[Constants.NumChannels + 1];

			switch(colour) {
				case ColourCode.SoundActive:
					channels[Mode] = SoundActiveMode;
					// TODO: Determine if support for more modes
					// 		 is required.
					channels[ColourOrChaseMode] = 0xFF;
					break;
				case ColourCode.Off:
					// Do nothing.
					// The relevant channels are already initialized to 0.
					break;
				// TODO: Support strobe & colour modes.
				default:
					throw new ArgumentException(
						colour + " is not supported for " + fixtureName,
						nameof(colour)
					);
			}

			byte[] corrected = new byte[Constants.NumChannels];
			Array.Copy(channels, 1, corrected, 0, corrected.Length);

			return channels;
		}
	}
}
