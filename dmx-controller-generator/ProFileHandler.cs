using System.Collections.Generic;
using System.IO;

namespace dmxcontrollergenerator {

	/// <summary>
	/// Read from &amp; modify *.PRO files
	/// </summary>
	public static class ProFileHandler {

		public static byte[] GenerateFileContents(
			IEnumerable<SceneBank> sceneBanks,
			Stream inputFile
		) {
			byte[] output;

			BinaryReader reader = new BinaryReader(inputFile);
			output = reader.ReadBytes(Constants.ProFileLength);

			foreach(SceneBank sbank in sceneBanks) {
				WriteSceneBank(sbank, output);
			}

			return output;
		}

		private static void WriteSceneBank(SceneBank sbank, byte[] output) {
			ushort offset = sbank.Offset;
			byte[] data = sbank.GenerateOutput();

			for(int i = 0; i < data.Length; i++) {
				output[offset + i] = data[i];
			}
		}

	}
}
