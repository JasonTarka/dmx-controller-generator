using System;

namespace dmxcontrollergenerator {

	/// <summary>
	/// A bank within a scene, where channel values are set, and the points do matter.
	/// </summary>
	public class SceneBank {

		private const byte NUM_CHANNELS = Constants.NumChannels;
		private const byte NUM_SCENES = 8;
		private const byte NUM_BANKS_PER_SCENE = 30;
		private const ushort OFFSET_START = 0x300;
		private const ushort SBANK_SIZE = 0x100;

		private const ushort CHECKSUM_OFFSET = 0xC1;

		// There are 2 runs of 0xFF in the middle of the banks.
		// These are their lengths.
		private const ushort FIRST_FF_RUN_LENGTH = 0x100;
		private const ushort SECOND_FF_RUN_LENGTH = 0x80;

		public byte Scene { get; }
		public byte Bank { get; }
		public byte[] Channels { get;  private set; }
		public ushort Offset { get; }
		public ushort Checksum { get; private set; }

		/// <summary>
		/// The number of channels that have been set (are not 0).
		/// </summary>
		public byte ChannelCount { get; private set; }

		public SceneBank(byte sceneNum, byte bankNum) {
			Scene = sceneNum;
			Bank = bankNum;
			Offset = DetermineOffset();
		}

		/// <summary>
		/// Sets the channels.
		/// </summary>
		/// <returns>This object.</returns>
		/// <param name="channels">Channels, as a 16-</param>
		public SceneBank SetChannels(byte[] channels) {
			// Enforce the 16-channel requirement
			if(channels.Length != Constants.NumChannels) {
				throw new ArgumentException(
					"Channels must have exactly " + NUM_CHANNELS + " elements",
					nameof(channels)
					);
			}

			Channels = channels;

			ushort checksum = 0;
			byte channelsSet = 0;
			for(ushort i = 0; i < NUM_CHANNELS; i++) {
				if(channels[i] > 0) {
					checksum |= (ushort)(1 << i);
					channelsSet++;
				}
			}
			Checksum = checksum;
			ChannelCount = channelsSet;

			return this;
		}

		public byte[] GenerateOutput() {
			byte[] output = new byte[SBANK_SIZE];

			// First byte is a count of channels set to a non-zero value
			output[0] = ChannelCount;

			// Next 16 bytes are the values of the channels
			for(int i = 1; i <= Channels.Length; i++) {
				output[i] = Channels[i - 1];
			}

			// Checksum is some random offset value (who picks 193 bytes?).
			// Checksum is 2 bytes, but stored in reverse order.

			output[CHECKSUM_OFFSET] = (byte)(0xFF & Checksum);
			output[CHECKSUM_OFFSET + 1] = (byte)(Checksum >> 8);

			return output;
		}

		private ushort DetermineOffset() {
			int bankOffset = (Bank - 1) * NUM_SCENES
			                    + (Scene - 1);
			int byteOffset = bankOffset * SBANK_SIZE
			                    + OFFSET_START;

			// These magic numbers map to the first SBank after the FF runs.
			if(Bank > 6 || (Bank == 6 && Scene >= 2)) {
				byteOffset += FIRST_FF_RUN_LENGTH;
			}

			if(Bank > 11 || (Bank == 11 && Scene >= 4)) {
				byteOffset += SECOND_FF_RUN_LENGTH;
			}

			return (ushort)byteOffset;
		}
	}
}

