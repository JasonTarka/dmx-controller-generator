using System;
namespace Fixtures {
	public interface IFixture {

		string FixtureName { get; }

		/// <summary>
		/// Determine if the name &amp; code is correct for this fixture, and
		/// if they are, generate the correct channel values that will cause
		/// the fixture to output the desired lighting.
		/// </summary>
		/// <returns>
		/// The channel values if the name and code are correct, otherwise null.
		/// </returns>
		/// <param name="fixtureName">Fixture name.</param>
		/// <param name="colour">Colour code.</param>
		bool TryGetChannelValues(string fixtureName, ColourCode colour, out byte[] channels);
	}
}
