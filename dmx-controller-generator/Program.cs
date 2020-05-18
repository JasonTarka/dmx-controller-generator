using System;
using System.IO;

namespace dmxcontrollergenerator {
	class MainClass {
		private const int RequiredArgs = 2;

		public static int Main( string[] args ) {
			if(args.Length < RequiredArgs) {
				ShowUsage();
				return -1;
			}

			string settingsFile = args[0],
				proFilePath = args[1];

			Processor processor = new Processor(settingsFile, proFilePath);
			try {
				processor.ProcessFiles();
			} catch(Exception ex) {
				if(ex is InvalidDataException
					|| ex is FileNotFoundException
				) {
					// Intended to be shown to the user. No stack trace needed.
					Console.Error.WriteLine(ex.Message);
					return -2;
				}

				Console.Error.WriteLine(ex.Message);
				Console.Error.WriteLine(ex.StackTrace);
				return -3;
			}

			return 0;
		}

		private static void ShowUsage() {
			string thisFile = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
			Console.WriteLine("Expected usage:");
			Console.WriteLine($"{thisFile} {{program file}} {{*.PRO file}}");
		}
	}
}
