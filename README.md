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
2. Install the Maestro - Midi Player Tool Kit (free version) plugin (https://assetstore.unity.com/packages/tools/audio/maestro-midi-player-tool-kit-free-107994).
3. Download and open up MIDI Cinema.
4. Go to the menu tools > MPTK > Midi File Setup > Add a Midi File (https://paxstellar.fr/setup-mptk-add-midi-files-v2/).
5. Open up the GameController.cs script.
6. In the Start() function, locate the line ```midiPlayer.MPTK_MidiName = "Heaven Can Wait";```
7. Simply replace the string with the title of the MIDI file you added ```midiPlayer.MPTK_MidiName = "My MIDI File Title";```

# How it works
## Basic movement, scene and camera setup
The game is mostly controlled by the GameController object. Actions such as changing the camera's view and playing/stopping the MIDI are contained in the GameController.cs script:
```C#
// Player input
horizontalInput = Input.GetAxis("Horizontal");
verticalInput = Input.GetAxis("Vertical");

// The camera view can rotate horizontally or move back & forth
transform.Rotate(0, horizontalInput * rotationSpeed * Time.deltaTime, 0);
transform.Translate(0, 0, verticalInput * speed * Time.deltaTime);

// Respawn if you fall into the void
if(transform.position.y < -50)
{
	transform.position = new Vector3(0, 30, -9);
}

// Press space to play the MIDI file
if(Input.GetKeyDown(KeyCode.Space) && midiIsPlaying == false)
{
	midiPlayer.MPTK_Play();
	midiIsPlaying = true;
}

// Press Q to stop playing the MIDI file
if(Input.GetKeyDown(KeyCode.Q))
{
	midiPlayer.MPTK_Stop();
}

// Press R to reset your position & rotation
if(Input.GetKeyDown(KeyCode.R))
{
	transform.position = new Vector3(0, 0.5f, -9);
	transform.rotation = Quaternion.identity;
}
```

The scene is made of a cube (the GameController object) and 5 planes: 1 for the ground and 4 walls to prevent the player from falling into the void. To make the camera follow the player, it was set up to be a child of the GameController so that its position and rotation is always relative to the GameController.

## MPTK MidiFile prefab
The MPTK MidiFilePlayer prefab was attached to the GameObject. This prefab's main purpose is to play MIDI files, but it also provides real-time access to the MIDI events, e.g., note on, and this is how the visuals get generated. By default, the prefab is set to "Play At Startup" in the inspector, so this was unchecked since the player is meant to decide when to play the MIDI using the space key and when to stop playing using the Q key. Instead, a boolean flag "midiIsPlaying" is used to determine when to play the file. Most of the setup for this prefab can be found in the Start() method:
```C#
void Start()
{
	// Set up the MIDI player
	midiPlayer = FindObjectOfType<MidiFilePlayer>();

	// You can change the MIDI file by replacing this string with a title in the MidiDB folder, e.g., "Bach - Fugue"
	midiPlayer.MPTK_MidiName = "Heaven Can Wait";

	// Create graphics when a MIDI event occurs
	midiPlayer.OnEventNotesMidi.AddListener(NotesToPlay);

	// Turn the flag that indicates the MIDI is playing to false when the MIDI stops playing
	midiPlayer.OnEventEndPlayMidi.AddListener(EndPlay);
}
```

## Processing-style map function
Processing is a software sketchbook and it had a useful map() function that allowed you to map a value in one range into another range. Unity had no native equivalent, but it could be imitated by the use of the Mathf.InverseLerp() and Mathf.Lerp() functions.
```C#
// Processing style map function, i.e., map a value between one range to another range
public float mapf(float x, float min1, float max1, float min2, float max2)
{
	// Find x as a percentage in a range between min1 & max1
	float percentage = Mathf.InverseLerp(min1, max1, x);

	// Return the corresponding value when you apply the percentage from before into another range
	return Mathf.Lerp(min2, max2, percentage);
}
```
This function will be useful for generating the visuals in real-time.

## Visuals
The visuals are generated in the NotesToPlay() function. The MIDI events are passed to this function as a list since MIDI files tend to have more than one event, i.e., more than one note. The x-position for a note's spawn point will always be 26 which is the point just beyond the wall to the right.

The y-position for the spawn point is more complicated and it requires the use of the mapf() function. For the sake of simplicity, consider a MIDI file containing only one instrument, e.g., guitar. When the guitar plays a high note, then a note will spawn across the screen in response. The spawn position of this note will be directly correlated with the pitch of the instrument. So a high note played will spawn a note visual that has a large, positive y-value, i.e., high, and a low note played will spawn a note visual that has a y-value closer to 0 (closer to the ground), i.e., low. The lowest MIDI note is 0 and the highest MIDI note is 127. However, 21 and 108 were chosen as the first range in the mapf() function since they correspond with the lowest and highest keys on a piano keyboard and you rarely get music that plays notes beyond these pitches. The mapf() function will ensure that the y-position of the notes do not go beyond 0 or 25 (height of the wall).

The z-position for the spawn point will also require the use of the mapf() function. When you deal with MIDI files that have more than one track, then you may have trouble keeping track of what instrument is playing what. For this reason, the notes of each track is displayed in their own lane. The mapf() function ensures that the tracks fit within the width of the game's room. maxChannels (maximum number of channels/tracks) has been set to 31, which is an arbitrary number, but the average MIDI file does not contain this many anyways:
```C#
// Notes start at the far right side of the screen & move towards the left
float noteXPos = 26;

// 21 = MIDI number for the lowest note on a piano
// 108 = MIDI number for the highest note on a piano
// 25 = height of the wall
float noteYPos = mapf(midiEvent.Value, 21, 108, 0, 25);

// Different tracks (channels) are in different lanes
float noteZPos = mapf(midiEvent.Channel, 0, maxChannels, 5, 38);
Vector3 noteSpawnPos = new Vector3(noteXPos, noteYPos, noteZPos);
```

The visuals get generated using the Instantiate() function:
```C#
// Create the visual for the note
var noteVisual = Instantiate(CubeNote, noteSpawnPos, Quaternion.identity);
```

## Changing the colours and shapes
Changing the colours and/or shapes is achieved by pressing the Y/U/I/O/P keys to change some flag variables. These are handled in the Update() function.
```C#
// Press Y for blue coloured notes only
if(Input.GetKeyDown(KeyCode.Y))
{
	colourMode = 0;
}

// Press U for track based colouring
if(Input.GetKeyDown(KeyCode.U))
{
	colourMode = 1;
}

// Press I for randomised colouring
if(Input.GetKeyDown(KeyCode.I))
{
	colourMode = 2;
}

// Press O for cubes
if(Input.GetKeyDown(KeyCode.O))
{
	shapeStyle = 0;
}

// "It's spherical!"
if(Input.GetKeyDown(KeyCode.P))
{
	shapeStyle = 1;
}
```

In the NotesToPlay() function:
```C#
// Colouring the notes based on their track number.
if(colourMode == 1)
{
	// Even numbered tracks get the 1st half of hue range & odd numbered tracks get the 2nd half
	if(midiEvent.Channel % 2 == 0)
	{
		noteHue = mapf(midiEvent.Channel, 0, maxChannels, 0, 0.49f);
	}
	else
	{
		noteHue = mapf(midiEvent.Channel, 0, maxChannels, 0.5f, 1);
	}
}
else if(colourMode == 2)
{
	// Random colours
	noteHue = Random.Range(0f, 1f);
}

// Notes can be cube shaped or sphere shaped
if(shapeStyle == 0)
{
	// Create the visual for the note
	var noteVisual = Instantiate(CubeNote, noteSpawnPos, Quaternion.identity);

	// Only relevant to colour modes 1 & 2
	if(colourMode != 0)
	{
		noteVisual.GetComponent<Renderer>().material.color = Color.HSVToRGB(noteHue, 1, 1);
	}
}
else
{
	// Create the visual for the note
	var noteVisual = Instantiate(SphereNote, noteSpawnPos, Quaternion.identity);

	// Only relevant to colour modes 1 & 2
	if(colourMode != 0)
	{
		noteVisual.GetComponent<Renderer>().material.color = Color.HSVToRGB(noteHue, 1, 1);
	}
}
```

## Displaying/hiding the help text
Displaying/hiding the help text is achieved by pressing the H key to toggle on/off a flag variable. In the Update() function:
```C#
// Hide/show the help text
if(Input.GetKeyDown(KeyCode.H) && !helpOn)
{
	helpText.SetActive(true);
	helpOn = true;
} 
else if(Input.GetKeyDown(KeyCode.H) && helpOn)
{
	helpText.SetActive(false);
	helpOn = false;
}
```

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

# References
- Unity Basics by Imphenzia: https://www.youtube.com/watch?v=pwZpJzpE2lQ
- Unity Basics by Code Monkey: https://www.youtube.com/watch?v=E6A4WvsDeLE
- C# Scripting in Unity by Charger Games: https://youtube.com/playlist?list=PLytjVIyAOStrwNhGKtfCBsCLNyZYWOYBO
- MPTK Quick Start: https://paxstellar.fr/setup-mptk-quick-start-v2/
- MPTK Documentation: https://mptkapi.paxstellar.com/index.html
- Stephen Malinowski's YouTube Channel: https://www.youtube.com/c/smalin
- Stephen Malinowski's Music Animation Machine MIDI Player: http://www.musanim.com/Player/

# What I am most proud of in the assignment
- I managed to finish this in 1-2 days.
- I learned the MPTK package in a short amount of time.
- I transcribed 2 entire songs by ear using MuseScore. In fact, these transcriptions were the main reason why I wanted to create a MIDI visualisation program.
- I managed to mimic Processing's map function using Mathf.InverseLerp() and Mathf.Lerp().
- I am proud of the project's overall outcome. It is not a perfect clone of Stephen Malinowski's Music Animation Machine and there were many other features that I would have added if I had more time, e.g., ripple effect when the notes hit the wall on the left, GUI for selecting MIDI files from a list instead of manually changing the name in the script, but in my opinion, the project is wonderful enough as it is and the generated animations are quite pretty.

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
