using NUnit.Framework;
using System.IO;
using System.Reflection;
using dmxcontrollergenerator;
using System.Collections.Generic;

namespace dmxcontrollertests {
	[TestFixture]
	public class ProFileHandlerTests {

		[Test]
		public void GenerateOutput() {

			Assembly assembly = Assembly.GetExecutingAssembly();
			byte[] output;

			using(Stream stream = assembly.GetManifestResourceStream("testData.inputFile.PRO")) {
				output = ProFileHandler.GenerateFileContents(GetSceneBanks(), stream);
			}

			using(Stream stream = assembly.GetManifestResourceStream("testData.expectedOutput.PRO"))
			using(Stream outputStream = new MemoryStream(output)) {
				FileAssert.AreEqual(stream, outputStream);
			}
		}

		private IEnumerable<SceneBank> GetSceneBanks() {
			foreach(byte[][] data in testData) {
				byte scene = data[0][0],
					bank = data[0][1];

				byte[] dataChannels = data[1];
				byte[] channels = new byte[Constants.NumChannels];
				for(int i = 0; i < dataChannels.Length; i++) {
					channels[i] = dataChannels[i];
				}

				yield return new SceneBank(scene, bank)
					.SetChannels(channels);
			}
		}

		// Test data pulled from the example output file
		private readonly byte[][][] testData = {
			new byte[][] {
				new byte[] {1, 1},
				new byte[] {30,30,30}
			},
			new byte[][] {
				new byte[] {2, 1},
				new byte[] {0,0,45,0,0,45}
			},
			new byte[][] {
				new byte[] {1, 2},
				new byte[] {35,30,35,0,35}
			},
			new byte[][] {
				new byte[] {2, 2},
				new byte[] {0,0,0,50,50}
			},
			new byte[][] {
				new byte[] {1, 3},
				new byte[] {30,40,30,0,0,0,40,0,0,0,40,0,0,0,0,40}
			},
			new byte[][] {
				new byte[] {1, 4},
				new byte[] {180,180}
			},
			new byte[][] {
				new byte[] {1, 5},
				new byte[] {185,185}
			},
			new byte[][] {
				new byte[] {1, 6},
				new byte[] {190,190}
			},
			new byte[][] {
				new byte[] {1, 7},
				new byte[] {195,195}
			},
			new byte[][] {
				new byte[] {1, 8},
				new byte[] {200,200}
			},
			new byte[][] {
				new byte[] {1, 9},
				new byte[] {205,205}
			},
			new byte[][] {
				new byte[] {1, 10},
				new byte[] {210,210}
			},
			new byte[][] {
				new byte[] {1, 11},
				new byte[] {215,215}
			},
			new byte[][] {
				new byte[] {1, 12},
				new byte[] {220,220}
			},
			new byte[][] {
				new byte[] {1, 13},
				new byte[] {225,225}
			},
			new byte[][] {
				new byte[] {1, 14},
				new byte[] {230,230}
			},
			new byte[][] {
				new byte[] {8, 15},
				new byte[] {90,90}
			},
			new byte[][] {
				new byte[] {1, 16},
				new byte[] {15,15}
			},
			new byte[][] {
				new byte[] {8, 16},
				new byte[] {95,95}
			},
			new byte[][] {
				new byte[] {1, 17},
				new byte[] {20,20}
			},
			new byte[][] {
				new byte[] {8, 17},
				new byte[] {100,100}
			},
			new byte[][] {
				new byte[] {1, 18},
				new byte[] {25,25}
			},
			new byte[][] {
				new byte[] {1, 19},
				new byte[] {30,30}
			},
			new byte[][] {
				new byte[] {8, 19},
				new byte[] {105,105}
			},
			new byte[][] {
				new byte[] {1, 20},
				new byte[] {35,35}
			},
			new byte[][] {
				new byte[] {8, 20},
				new byte[] {110,110}
			},
			new byte[][] {
				new byte[] {1, 21},
				new byte[] {40,40}
			},
			new byte[][] {
				new byte[] {8, 21},
				new byte[] {115,115}
			},
			new byte[][] {
				new byte[] {1, 22},
				new byte[] {45,45}
			},
			new byte[][] {
				new byte[] {8, 22},
				new byte[] {120,120}
			},
			new byte[][] {
				new byte[] {1, 23},
				new byte[] {50,50}
			},
			new byte[][] {
				new byte[] {8, 23},
				new byte[] {125,125}
			},
			new byte[][] {
				new byte[] {1, 24},
				new byte[] {55,55}
			},
			new byte[][] {
				new byte[] {8, 24},
				new byte[] {130,130}
			},
			new byte[][] {
				new byte[] {1, 25},
				new byte[] {60,60}
			},
			new byte[][] {
				new byte[] {8, 25},
				new byte[] {135,135}
			},
			new byte[][] {
				new byte[] {1, 26},
				new byte[] {65,65}
			},
			new byte[][] {
				new byte[] {8, 26},
				new byte[] {140,140}
			},
			new byte[][] {
				new byte[] {1, 27},
				new byte[] {70,70}
			},
			new byte[][] {
				new byte[] {8, 27},
				new byte[] {145,145}
			},
			new byte[][] {
				new byte[] {1, 28},
				new byte[] {75,75}
			},
			new byte[][] {
				new byte[] {8, 28},
				new byte[] {150,150}
			},
			new byte[][] {
				new byte[] {1, 29},
				new byte[] {80,80}
			},
			new byte[][] {
				new byte[] {8, 29},
				new byte[] {155,155}
			},
			new byte[][] {
				new byte[] {1, 30},
				new byte[] {85,85}
			},
			new byte[][] {
				new byte[] {2, 30},
				new byte[] {0,0}
			},
			new byte[][] {
				new byte[] {8, 30},
				new byte[] {160,160}
			}
		};
	}

}
