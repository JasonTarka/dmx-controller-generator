using System;
using System.IO;
using System.Collections.Generic;
using Generator;
using System.Linq;

namespace Common {
	public class SettingsParser {

		private static readonly char[] ColumnDividers = { ',', '\t' };
		private const char SettingsDivider = '|';
		private const char CommentChar = '#'; // Must be at the beginning of the line.

		private readonly StreamReader m_settingsFile;
		private readonly IEnumerable<string[]> m_lines;

		private bool m_headerTaken;

		public SettingsParser(
			StreamReader settingsFile
		) {
			m_settingsFile = settingsFile;
			m_lines = ReadLines();
		}

		public string[] GetHeader() {
			if(m_headerTaken) throw new InvalidOperationException(
				"Cannot read header more than once.");

			m_headerTaken = true;
			return m_lines.Take(1).First();
		}

		public IEnumerable<string[]> GetLines() {
			if(!m_headerTaken) throw new InvalidOperationException(
				"Must read the header before reading the remaining lines.");

			return m_lines;
		}

		private IEnumerable<string[]> ReadLines() {
			while(true) {
				string line = m_settingsFile.ReadLine();
				if(line == null)
					break;

				// TODO: Keep track of line numbers, and add a method for throwing
				//       errors with the line number.

				// Skip empty lines & comments
				if(line.IsNullOrWhitespace()
					|| line[0] == CommentChar)
					continue;

				yield return line.Split(ColumnDividers);
			}
		}
	}
}
