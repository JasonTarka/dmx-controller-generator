# ADJ DMX Operator Programmer

This program is for modifying the save files created by/used for the [ADJ DMX
Operator][product], allowing you to "program" it using a spreadsheet.

This came from planning scenes & banks in a spreadsheet, then manually
programming the controller, moving sliders & saving every scene individually.
This is tedious and error-prone, so I automated it.

[product]: https://www.adj.com/dmx-operator

## Disclaimer

- This is not an endorsement for any particular product.
- This is not endorsed by ADJ, or any other manufacturer.
- I am not responsible if using this code breaks anything.
- This may be able to edit files for similar models, but there has been no
  testing, so it may cause your device to explode, or otherwise no longer work.

## What it does

The controller lets you export the programmed scenes for a fixture to a USB
stick in a binary format, `FILE#.PRO`. This program will read in the values
you've configured and modify the `.PRO` file to include them. The DMX controller
can then read the modified file and update the scenes for this fixture.

## How to use

1. Export a series of fixture files (`FILE#.PRO`) from your DMX controller
2. Use your favourite spreadsheet software to plan your scenes
3. Export the scenes to a CSV file of the correct format
4. Run this program, specifying the fixture code, input file, and the directory
   containing `*.PRO` files to be modified

```
$ dmx-controller-generator.exe {fixture code} {settings.csv} {FILE1.PRO}
```

## Settings file format

Very simple format, allowing multiple fixtures to be programmed at once.

```csv
Scene,Bank,fixtureCodeOne,fixtureCodeTwo
4,15,Red,Blue
8,5,SoundActive|16,Aqua

# This is a comment.
# Blank lines are ignored.
# Comments can only start at the beginning of a line.
```

### Headers

The first non-empty, non-comment line is considered the file header. The first 2
columns are ignored (but must exist), and each additional column specifies the
name of the fixture to be programmed (must match a valid fixture code).

The first fixture will modify `FILE1.PRO`, the second `FILE2.PRO`, and so on.

### Scene & Bank

Each line starts by specifying the scene # and bank # to be programmed. Scenes
must be 1–8, and banks must be 1–30. 

### Colour Codes

These depend on which fixture you're programming, but they're all a single-word
code, which map to an `enum` in the code. If a code isn't supported, an error
will be thrown.

### Colour Code Parameters

Some fixtures support (or require) additional settings for certain colour codes.
For example, a sound active mode may have several patterns to choose from, and
requires that a pattern be specified.

These settings are specified as parameters after the colour code, separated by
`|`. For example, `SoundActive|12` will set the fixture to sound active mode
#12, or `Macro|red|129` may mean a red chase at half speed. Supported/required
parameters are detailed for each fixture below.

## Supported Fixtures

We only have a few different types of fixtures, so these are the only ones that
I've added support for.

### ADJ Mega PAR UV

PAR-like LED lights, with UV LED.

Code: `adjMegaPar`

#### Supported codes

- `Off`
- `Red`
- `Green`
- `Blue`
- `Orange`
- `Pink`
- `Aqua`
- `SoundActive|<mode>|<sensitivity>`
- `Macro|<type>|<mode>|<speed>`

#### Sound Active parameters

- **Format:** `SoundActive|<mode>|<sensitivity>`
- **"Mode:** A number between 1 and 16; each mode has pre-programmed colours it
  will switch between.
- **Sensitivity:** A number from 0 to 10; how sensitive the fixture will be to
  sound.

#### Macro parameters

- **Format:** `Macro|<type>|<mode>|<speed>`
- **Type:** Either `change` or `fade`
  - `change` is colour change mode, where the fixture will flip between various
    colours.
  - `fade` is colour fade mode, where the fixture will fade between various
    colours.
- **"Mode:** A number between 1 and 16; each mode has pre-programmed colours it
  will switch between.
- **Speed:** A number from 0 to 10; how quickly the fixture will change between
  colours.

### Silver PAR

Some generic silver LED PAR cans, where channel 1 is the master dimmer, and
channels 2–4 are red, green, and blue, respectively.

Code: `Silver`

#### Supported codes

- Off
- Red
- Green
- Blue
- Orange
- Pink
- Aqua

### ADJ Jellyfish

LED fixture with ~24 LEDs, with various cool sound active settings.

Code: `Jellyfish`

#### Supported codes

- Off
- SoundActive
