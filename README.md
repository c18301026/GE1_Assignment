# Project Title: MIDI Cinema

Name: Rianlee Gabriel Pineda

Student Number: C18301026

Class Group: TU856 (DT228)

# Description of the project
MIDI Cinema is a system made using Unity that allows a user listen to their own MIDI files while viewing visuals generated in response to the data from the MIDI file. The user may also customise the visuals of the animation itself, e.g., the colours, shapes and style. This project makes use of Thierry Bachmann's Maestro - Midi Player Tool Kit (free version), a Unity plugin/package that allows you to handle MIDI data.

The inspiration for this project idea is Stephen Malinowski's Music Animation Machine. Here is an example of Malinowski's work:

[![YouTube](http://img.youtube.com/vi/yq3HEMaIppo/0.jpg)](https://www.youtube.com/watch?v=yq3HEMaIppo)

# Instructions for use
## Controls
- 'H' key: hide/show this help text
- Space key: play the MIDI file
- Left/right (or 'A'/'D') keys: rotate camera view horizontally
- Up/down (or 'W'/'S') keys: move camera view forward/backwards
- 'R' key: reset camera position & angle
- 'Q' key: stop playing the MIDI file
- 'Y' key: colour the notes blue
- 'U' key: colour the notes based on track number
- 'I' key: colour the notes randomly
- 'O' key: display cuboid visuals
- 'P' key: display spherical visuals

## How do I play and visualise my own custom MIDI files?
1. Download Unity (version 2020.3.19f1) if you haven't already.
2. Download and open up MIDI Cinema.
3. Install the Maestro - Midi Player Tool Kit (free version) plugin (https://assetstore.unity.com/packages/tools/audio/maestro-midi-player-tool-kit-free-107994).
4. Go to the menu tools > MPTK > Midi File Setup > Add a Midi File (https://paxstellar.fr/setup-mptk-add-midi-files-v2/).
5. Open up the GameController.cs script.
6. In the Start() function, locate the line: midiPlayer.MPTK_MidiName = "Heaven Can Wait";
7. Simply replace the string with the title of the MIDI file you added: midiPlayer.MPTK_MidiName = "My MIDI File Title";

# How it works
Most of the program is controlled by the GameController object. Most of the control, e.g., movement, play MIDI, is contained in the script GameController.cs. The GameController object is technically a cube in the "game", but it is really just a way for the camera to move around and view the visuals at different angles if the player wishes.

As mentioned earlier, this project makes use of Thierry Bachmann's Maestro - Midi Player Tool Kit (MPTK), so it was necessary for the MidiPlayerTK namespace to be added. The code in this plugin essentially lets you handle MIDI in Unity in many different ways ranging from using MIDI for simple background music to more involved projects where MIDI events can trigger certain actions, e.g., rhythm games.

MPTK provides a prefab called MidiFilePlayer, which was essential to the success of this project. This prefab is self-explanatory; it plays MIDI files. It also allows you to get real time access to the MIDI events, e.g., note on, and this is how the visuals on the screen are generated. For the sake of simplicity, lets deal with a MIDI file containing only one instrument, e.g., guitar. When the guitar plays a high note, then a note graphic (can be a coloured cube or a sphere) will spawn across the screen in response. The spawn point/position of this note will be directly correlated with the pitch of the instrument. So a high note played will spawn a note visual that has a large, positive y-value, i.e., high, and a low note played will spawn a note visual that has a y-value closer to 0 (closer to the ground), i.e., low.

When you deal with MIDI files that have more than one track, then you may have trouble keeping track of what instrument is playing what. For this reason, each tracks' notes are displayed in their own lane and you also have the option of colouring the notes based on their track. Both of these tasks required values in ranges to be mapped into another range, e.g., making the tracks fit within the width of the game room. In fact, even the spawn point of the note visuals also needed something like this. Processing has an in-built map function that does this exactly and so I tried to mimic this functionality by writing my own function called mapf() which makes use of linear interpolation.

# List of classes/assets in the project and whether made yourself or modified or if its from a source, please give the reference

| Class/asset | Source |
|-----------|-----------|
| GameController.cs (Scripts folder) | Created by me |
| NoteBehaviour.cs (Scripts folder) | Created by me |
| PlainBlackMaterial (Materials folder) | Created by me |
| PlainBlueMaterial (Materials folder) | Created by me |
| CubeNote (Prefabs folder) | Created by me |
| SphereNote (Prefabs folder) | Created by me |
| SampleScene (Scenes folder) | Created by me |
| Heaven Can Wait MIDI file | Composed by Kai Hansen; transcribed & exported to MIDI by me using MuseScore |
| Seikai wa Hitotsu! Janai!! MIDI file | Composed by Akihiko Yamaguchi; transcribed & exported to MIDI by me using MuseScore |
| Carolina Shout MIDI file | Composed by James Price Johnson; MIDI file created by John E. Roache; available at: http://www.johnroachemusic.com/c_shout.html |
| Bach - Fugue MIDI file | Composed by Johann Sebastian Bach; already provided by the MPTK plugin as one of the example MIDI files |
| MPTK plugin (almost everything in the MidiPlayer folder) | Created by Thierry Bachmann; available at: https://assetstore.unity.com/packages/tools/audio/maestro-midi-player-tool-kit-free-107994 |

# Video Demos
## Gamma Ray - Heaven Can Wait
For fans of rock/metal music:

[![YouTube](http://img.youtube.com/vi/v6uiwrtFwBc/0.jpg)](https://www.youtube.com/watch?v=v6uiwrtFwBc)

## Johann Sebastian Bach - Fugue in F minor, BWV 881
For fans of Baroque music:

[![YouTube](http://img.youtube.com/vi/UGS5At3wi8o/0.jpg)](https://www.youtube.com/watch?v=UGS5At3wi8o)

## James Price Johnson - Carolina Shout
For fans of jazz music:

[![YouTube](http://img.youtube.com/vi/8bvENPZRXyw/0.jpg)](https://www.youtube.com/watch?v=8bvENPZRXyw)

## Milky Holmes - Seikai wa Hitotsu! Janai!!
For fans of Japanese pop music:

[![YouTube](http://img.youtube.com/vi/D9Ju5y88Kaw/0.jpg)](https://www.youtube.com/watch?v=D9Ju5y88Kaw)
