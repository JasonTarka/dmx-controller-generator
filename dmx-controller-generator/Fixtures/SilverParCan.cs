using System;
using dmxcontrollergenerator;

namespace Fixtures {

	/// <summary>
	/// A generic LED PAR can we have.
	/// Doesn't support any special functions, just basic RBG+Dimmer.
	/// </summary>
	public class SilverParCan : IFixture {

		private const byte Dimmer = 1,
			Red = 2,
			Green = 3,
			Blue = 4;

		public string FixtureName => "Silver";

		bool IFixture.TryGetChannelValues(
			string fixtureName,
			ColourCode colour,
			out byte[] channels
		) {
			channels = null;
			if(fixtureName != FixtureName) return false;

			channels = new byte[Constants.NumChannels + 1];
			byte on = Constants.MaxVal;

			// Values that are needed in almost every case.
			channels[Dimmer] = on; // leave it always on, even when dark

			switch(colour) {
				case ColourCode.Red:
					channels[Red] = on;
					break;
				case ColourCode.Green:
					channels[Green] = on;
					break;
				case ColourCode.Blue:
					channels[Blue] = on;
					break;
				case ColourCode.Orange:
					channels[Red] = on;
					channels[Green] = on;
					break;
				case ColourCode.Pink:
					channels[Red] = on;
					channels[Blue] = on;
					break;
				case ColourCode.Aqua:
					channels[Blue] = on;
					channels[Green] = on;
					break;
				case ColourCode.Off:
					// Do nothing.
					// The relevant channels are already initialized to 0.
					break;
				default:
					throw new ArgumentException(
						colour + " is not supported for " + fixtureName,
						nameof(colour)
					);
			}

			byte[] corrected = new byte[Constants.NumChannels];
			Array.Copy(channels, 1, corrected, 0, corrected.Length);

			channels = corrected;
			return true;
		}
	}
}
