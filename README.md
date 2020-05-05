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

1. Export a fixture file (`FILE#.PRO`) from your DMX controller
2. Use your favourite spreadsheet software to plan your scenes
3. Export the scenes to a CSV file of the correct format
4. Run this program, specifying the fixture code, input file, and `*.PRO` file

```
$ dmx-controller-generator.exe {fixture code} {settings.csv} {FILE1.PRO}
```

### Settings file format

Very simple format:

```csv
# Scene number, Bank number, Colour code[, Optional settings]
4,15,Red
4,15,SoundActive,7

# This is a comment.
# Blank lines are ignored.
# Comments can only start at the beginning of a line.
```

## Colour Codes

These depend on which fixture you're programming, but they're all a single-word
code, which map to an `enum` in the code.

## Supported Fixtures

We only have a few different types of fixtures, so these are the only ones that
I've added support for.

### ADJ Mega PAR UV

PAR-like LED lights, with UV LED.

#### Supported codes

- Off
- Red
- Green
- Blue
- Orange
- Pink
- Aqua
- SoundActive

### Silver PAR

Some generic silver LED PAR cans.

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

#### Supported codes

- Off
- SoundActive
