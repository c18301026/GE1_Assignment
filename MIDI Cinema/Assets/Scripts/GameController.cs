using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiPlayerTK;

public class GameController : MonoBehaviour
{
	// Variables that control the player's movement
	public float speed = 10;
	public float rotationSpeed = 100;
	public float horizontalInput, verticalInput;

	// Variables relating to the MPTK MIDI player
	public MidiFilePlayer midiPlayer;
	public bool midiIsPlaying = false;

	// Arbitrary number for the max number of channels, but the average MIDI file doesn't have more than this many anyways
	int maxChannels = 31;

	// Variables related to the note visuals
	public GameObject CubeNote;
	public float noteHue;

	// Colour modes: 0 = same colour, 1 = based on track, 2 = randomised
	public int colourMode = 0;

	// Start is called before the first frame update
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

	// Update is called once per frame
	void Update()
	{
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

		// Press Y for uniform colouring
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
	}

	// Create graphics when a MIDI event occurs.
	public void NotesToPlay(List<MPTKEvent> midiEvents)
	{
		foreach(MPTKEvent midiEvent in midiEvents)
		{
			if(midiEvent.Command == MPTKCommand.NoteOn)
			{
				// Notes start at the far right side of the screen & move towards the left
				float noteXPos = 26;

				// 21 = MIDI number for the lowest note on a piano
				// 108 = MIDI number for the highest note on a piano
				// 25 = height of the wall
				float noteYPos = mapf(midiEvent.Value, 21, 108, 0, 25);

				// Different tracks (channels) are in different lanes
				float noteZPos = mapf(midiEvent.Channel, 0, maxChannels, 5, 38);
				Vector3 noteSpawnPos = new Vector3(noteXPos, noteYPos, noteZPos);

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

				// Create the visual for the note
				var noteVisual = Instantiate(CubeNote, noteSpawnPos, Quaternion.identity);

				// Only relevant to colour modes 1 & 2
				if(colourMode != 0)
				{
					noteVisual.GetComponent<Renderer>().material.color = Color.HSVToRGB(noteHue, 1, 1);
				}
			}
		}
	}

	// Turn the flag that indicates the MIDI is playing to false when the MIDI stops playing
	public void EndPlay(string name, EventEndMidiEnum reason)
	{
		midiIsPlaying = false;
	}

	// Processing style map function, i.e., map a value between one range to another range
	public float mapf(float x, float min1, float max1, float min2, float max2)
	{
		// Find x as a percentage in a range between min1 & max1
		float percentage = Mathf.InverseLerp(min1, max1, x);

		// Return the corresponding value when you apply the percentage from before into another range
		return Mathf.Lerp(min2, max2, percentage);
	}
}