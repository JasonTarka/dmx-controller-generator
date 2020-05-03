using System;
using System.Reflection;

namespace dmxcontrollergenerator {
	class MainClass {
		public static void Main( string[] args ) {
			if(args.Length < 2) {
				throw new TargetParameterCountException("You must specify an input file, and a *.PRO file to modify.");
			}

			string inputFile = args[0],
				outputFile = args[1];

		}
	}
}
