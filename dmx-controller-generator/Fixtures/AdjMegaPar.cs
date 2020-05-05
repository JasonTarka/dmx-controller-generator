using System;
using dmxcontrollergenerator;

namespace Fixtures {
	/// <summary>
	/// ADJ Mega PAR Profile Plus, with UV LED.
	/// </summary>
	public class AdjMegaPar : IFixture {

		const byte Red = 1,
			Green = 2,
			Blue = 3,
			UV = 4,
			OffOnStrobe = 5,
			Dimmer = 6,
			ModeSelector = 7, // dimming, colour macro,
		 					  // colour change, colour fade, sound active.
			MacroProgram = 8, // Macro/change/fade/SA mode. Mostly in
		 					  // 15-point blocks
			SpeedSensitivity = 9;

		public string FixtureName => "adjMegaPar";

		bool IFixture.TryGetChannelValues(
			string fixtureName,
			ColourCode colour,
			out byte[] channels
		) {
			channels = null;
			if(fixtureName != FixtureName) return false;

			channels = new byte[Constants.NumChannels+1];
			byte on = Constants.MaxVal;

			// Values that are needed in almost every case.
			channels[OffOnStrobe] = on;
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
				case ColourCode.SoundActive:
					throw new NotImplementedException(
						"Sound Active is not yet implemented for " + fixtureName
					);
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
