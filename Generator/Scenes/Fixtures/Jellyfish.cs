﻿using System;
using System.IO;
using Generator;

namespace Scenes.Fixtures {

	/// <summary>
	/// ADJ Jellyfish in basic 3-channel mode.
	/// </summary>
	public class Jellyfish : IFixture {
		public string FixtureName => "Jellyfish";

		private const byte
			Mode = 1,
			ColourOrChaseMode = 2,
			Speed = 3; // Only matters in chase and strobe

		private const byte
			ColourChangeMode = 10,
			StrobeMode = 130,
			SoundActiveMode = 255;

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
