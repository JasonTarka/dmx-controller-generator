using System;
using dmxcontrollergenerator;
using System.IO;

namespace Fixtures {
	/// <summary>
	/// ADJ Mega PAR Profile Plus, with UV LED.
	/// </summary>
	public class AdjMegaPar : IFixture {

		private const byte
			Red = 1,
			Green = 2,
			Blue = 3,
			UV = 4,
			OffOnStrobe = 5,
			Dimmer = 6,
			ModeSelector = 7, // dimming, colour macro,
		 					  // colour change, colour fade, sound active.
			MacroProgram = 8, // Macro/change/fade/SA mode. Mostly in
		 					  // 16-point blocks
			SpeedSensitivity = 9;

		private const byte
			SoundActiveMode = 255,
			ColourChangeMode = 104,
			ColourFadeMode = 155;

		private const byte On = Constants.MaxVal;

		public string FixtureName => "adjMegaPar";

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
			byte[] channels = new byte[Constants.NumChannels+1];

			// Values that are needed in almost every case.
			channels[OffOnStrobe] = On;
			channels[Dimmer] = On; // leave it always on, even when dark

			switch(colour) {
				case ColourCode.Red:
					channels[Red] = On;
					break;
				case ColourCode.Green:
					channels[Green] = On;
					break;
				case ColourCode.Blue:
					channels[Blue] = On;
					break;
				case ColourCode.Orange:
					channels[Red] = On;
					channels[Green] = On;
					break;
				case ColourCode.Pink:
					channels[Red] = On;
					channels[Blue] = On;
					break;
				case ColourCode.Aqua:
					channels[Blue] = On;
					channels[Green] = On;
					break;
				case ColourCode.SoundActive:
					SetSoundActive(channels, settings);
					break;
				case ColourCode.Macro:
					SetMacroMode(channels, settings);
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

		private void SetSoundActive(byte[] channels, string[] settings) {
			string name = $"{FixtureName}: {ColourCode.SoundActive}";
			if(settings.Length != 2) throw new InvalidDataException(
				$"{name}: requires 2 numeric parameters. {settings.Length} given.");

			if(!byte.TryParse(settings[0], out byte mode)
				|| mode < 1 || mode > 16)
				throw new InvalidDataException($"{name}: Mode must be between 1 and 16." +
					$" {settings[0]} given.");

			if(!byte.TryParse(settings[1], out byte sensitivity)
				|| sensitivity < 1 || sensitivity > 10)
				throw new InvalidDataException($"{name}: Sensitivity must be between 1 and 10." +
					$" {settings[1]} given.");

			channels[ModeSelector] = SoundActiveMode;
			channels[MacroProgram] = (byte)((mode - 1) * 16 + 1);
			channels[SpeedSensitivity] = (byte)(sensitivity * 25 + 1);
		}

		private void SetMacroMode(byte[] channels, string[] settings) {
			const string fade = "fade",
				change = "change";

			string name = $"{FixtureName}: {ColourCode.Macro}";
			if(settings.Length != 3) throw new InvalidDataException(
				$"{name}: requires 3 parameters. {settings.Length} given.");

			string type = settings[0];

			if(!byte.TryParse(settings[1], out byte mode)
				|| mode < 1 || mode > 16)
				throw new InvalidDataException($"{name}: Mode must be between 1 and 16." +
					$" {settings[1]} given.");

			if(!byte.TryParse(settings[2], out byte speed)
				|| speed < 1 || speed > 10)
				throw new InvalidDataException($"{name}: Speed must be between 1 and 10." +
					$" {settings[2]} given.");

			switch(type) {
				case fade:
					channels[ModeSelector] = ColourFadeMode;
					break;
				case change:
					channels[ModeSelector] = ColourChangeMode;
					break;
				default:
					throw new InvalidDataException(
						$"{name}: Type must be '{change}' or '{fade}'. '{type}' given.");
			}

			channels[MacroProgram] = (byte)((mode - 1) * 16 + 1);
			channels[SpeedSensitivity] = (byte)(speed * 25 + 1);
		}
	}
}
