using System;
using System.Collections.Generic;
using System.IO;
using Common;
using System.Collections;
using Scenes;

namespace Chases.Settings {
	public static class SettingsReader {
		public static IEnumerable<Chase> ReadSettingsFile(
			StreamReader settingsFile
		) {
			SettingsParser parser = new SettingsParser(settingsFile);

			// Header doesn't matter, but it must exist.
			parser.GetHeader();

			IDictionary<byte, Chase> chases = new Dictionary<byte, Chase>();
			foreach(string[] line in parser.GetLines()) {
				if(!byte.TryParse(line[0], out byte chaseNum)) throw new Exception();

				if(!byte.TryParse(line[1], out byte sceneNum)
					|| !byte.TryParse(line[2], out byte bankNum)
				) throw new Exception();

				SceneBank sbank = new SceneBank(sceneNum, bankNum);

				Chase chase = chases.ContainsKey(chaseNum)
					? chases[chaseNum]
					: new Chase(chaseNum);

				chases[chaseNum] = chase.AddScene(sceneNum, bankNum);
			}

			return chases.Values;
		}
	}
}
