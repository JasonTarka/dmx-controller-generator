using System.Collections.Generic;
using Scenes;
using Generator;
using System.IO;

namespace Chases {
	public class Chase {

		const short CHASE_SIZE = 0x100;

		private IList<SceneBank> m_scenes = new List<SceneBank>();

		public byte Number { get; }

		public Chase(byte number) {
			Number = number;
		}

		public Chase AddScene(byte scene, byte bank) {
			if(m_scenes.Count >= Constants.MaxScenesPerChase) throw new InvalidDataException(
				$"Chases can contain a maximum {Constants.MaxScenesPerChase} scenes.");

			m_scenes.Add(new SceneBank(scene, bank));
			return this;
		}

		public byte[] GenerateOutput() {
			byte[] output = new byte[CHASE_SIZE];

			// First byte is a count of how many scenes are included.
			output[0] = (byte)m_scenes.Count;

			// Scenes in the chase are stored as Scene/Bank numbers, in
			// order of appearance.
			// They start on the 3rd byte (0x02)
			for(int i = 0; i < m_scenes.Count; i++) {
				output[i + 2] = m_scenes[i].Number;
			}

			// There doesn't appear to be a checksum, or if there is,
			// it's not needed.
			return output;
		}

	}
}
