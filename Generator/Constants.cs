﻿using System.Collections.Generic;
using Scenes.Fixtures;

namespace Generator {
	public static class Constants {
		/// <summary>
		/// The total length of a *.PRO file, in bytes.
		/// </summary>
		public const int ProFileLength = 0x20200;

		// These refer to those available on my particular model.
		// Different models of ADJ DMX Controllers may have different numbers.
		public const byte MaxFixtures = 12;
		public const byte NumChannels = 16;
		public const byte NumScenes = 8;
		public const byte NumBanks = 30;

		/// <summary>
		/// Maximum value for a channel.
		/// </summary>
		public const byte MaxVal = 0xFF;

		public static readonly IEnumerable<IFixture> Fixtures = new IFixture[] {
				new AdjMegaPar(),
				new SilverParCan(),
				new Jellyfish()
			};

		public const byte MaxChases = 6;
		public const byte MaxScenesPerChase = 240;
	}
}
