using System;
namespace Fixtures {
	public interface IFixture {

		string FixtureName { get; }

		bool TryGetFixture(string fixtureName, out IFixture fixture);

		/// <summary>
		/// Generate the correct channel values that will cause
		/// the fixture to output the desired lighting.
		/// Note that the byte[] starts at 0, but DMX channels
		/// start at 1.
		/// </summary>
		/// <returns>
		/// The channel values.
		/// </returns>
		/// <param name="colour">Colour code.</param>
		/// <param name="settings">Any additional settings for the given colour code.</param>
		byte[] GetChannelValues(ColourCode colour, string[] settings);
	}
}
