using System;
using System.Reflection;

namespace dmxcontrollergenerator {
	class MainClass {
		private const int RequiredArgs = 3;

		public static int Main( string[] args ) {
			if(args.Length < RequiredArgs) {
				ShowUsage();
				return -1;
			}

			string fixtureName = args[0],
				settingsFile = args[1],
				proFile = args[2];

			using(Processor processor = new Processor(fixtureName, settingsFile, proFile)) {
				processor.ProcessFiles();
			}

			return 0;
		}

		private static void ShowUsage() {
			Console.WriteLine("Expected usage:");
			Console.WriteLine("dmx-controller-generator {fixture code} {program file} {PRO file}");
		}
	}
}
