using System;
namespace dmxcontrollergenerator {
	public static class Constants {
		/// <summary>
		/// The total length of a *.PRO file, in bytes.
		/// </summary>
		public const int ProFileLength = 0x20200;

		// These refer to those available on my particular model.
		// Different models of ADJ DMX Controllers may have different numbers.
		public const byte NumChannels = 16;
		public const byte NumScenes = 8;
		public const byte NumBanks = 30;
	}
}
