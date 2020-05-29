using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Generator;

namespace Scenes {

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

		private readonly IList<byte[]> m_channels = new List<byte[]>();

		public byte Scene { get; }
		public byte Bank { get; }
		public ushort Offset { get; }

		/// <summary>
		/// The absolute number of this scene, where
		/// S1B1 is 0, S2B1 is 1, S3B1 is 3, etc.
		/// </summary>
		public byte Number {
			get => (byte)((Bank - 1) * Constants.NumScenes + (Scene - 1));
		}

		/// <summary>
		/// The number of channels that have been set (are not 0)
		/// for the first fixture.
		/// </summary>
		public byte ChannelCount { get; private set; }

		public SceneBank(byte sceneNum, byte bankNum) {
			Scene = sceneNum;
			Bank = bankNum;
			Offset = DetermineOffset();
		}

		/// <summary>
		/// Append a set of channel values to the scene.
		/// </summary>
		/// <returns>This object.</returns>
		/// <param name="channels">Channel values to add.</param>
		public SceneBank AddChannels(byte[] channels) {
			// Enforce the 16-channel requirement
			if(channels.Length != Constants.NumChannels) throw new ArgumentException(
					$"Channels must have exactly {NUM_CHANNELS} elements",
					nameof(channels)
				);

			// Enforce the max fixture limit
			if(m_channels.Count >= Constants.MaxFixtures) throw new InvalidDataException(
				$"A maximum {Constants.MaxFixtures} fixtures are supported.");

			m_channels.Add(channels);
			return this;
		}

		public byte[] GenerateOutput() {
			byte[] output = new byte[SBANK_SIZE];

			// Fixture channels run as a contiguous block.
			byte[] channelValues = m_channels.SelectMany(x => x).ToArray();

			// First byte is a count of all channels set to a non-zero value.
			// Note: The additional loop keeps this method settings bytes in order.
			output[0] = (byte)channelValues.Count(x => x > 0);

			// Next 16 bytes are the values of the channels
			for(int i = 0; i < channelValues.Length; i++) {
				output[1+i] = channelValues[i];
			}

			// Checksums immediately follow the channel values.
			// If we don't set all fixtures, it's still in the same place.
			for(int i = 0; i < m_channels.Count; i++) {
				int index = CHECKSUM_OFFSET + (i * 2);
				byte[] checksum = GetChecksumBytes(m_channels[i]);
				output[index] = checksum[0];
				output[index + 1] = checksum[1];
			}

			return output;
		}

		/// <summary>
		/// Get the checksum bytes for a set of channels.
		/// The checksum is stored as 2 bytes in reverse order.
		/// </summary>
		/// <returns>The checksum bytes.</returns>
		/// <param name="channels">Channel values to checksum.</param>
		public static byte[] GetChecksumBytes(byte[] channels) {
			ushort checksum = 0;
			for(ushort i = 0; i < NUM_CHANNELS; i++) {
				if(channels[i] > 0) {
					checksum |= (ushort)(1 << i);
				}
			}

			byte[] bytes = new byte[2];
			bytes[0] = (byte)(0xFF & checksum);
			bytes[1] = (byte)(checksum >> 8);
			return bytes;
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

