using System;
using System.IO;
using Generator;

namespace Scenes.Fixtures {

	/// <summary>
	/// A generic LED PAR can we have.
	/// Doesn't support any special functions, just basic RBG+Dimmer.
	/// </summary>
	public class SilverParCan : IFixture {

		private const byte
			Dimmer = 1,
			Red = 2,
			Green = 3,
			Blue = 4;

		public string FixtureName => "Silver";

		bool IFixture.TryGetFixture(string fixtureName, out IFixture fixture) {
			fixture = fixtureName == FixtureName ? this : null;
			return fixture != null;
		}

		byte[] IFixture.GetChannelValues(
			ColourCode colour,
			string[] settings
		) {
			// Create with +1 so the channel numberss in code match
			// what you'd use on a DMX controller. Trim the array later.
			byte[] channels = new byte[Constants.NumChannels + 1];
			const byte on = Constants.MaxVal;

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
					throw new InvalidDataException(
						colour + " is not supported for " + FixtureName
					);
			}

			byte[] corrected = new byte[Constants.NumChannels];
			Array.Copy(channels, 1, corrected, 0, corrected.Length);

			return corrected;
		}
	}
}
