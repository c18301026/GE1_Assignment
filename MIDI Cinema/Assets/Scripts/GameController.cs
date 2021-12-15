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

	// Start is called before the first frame update
	void Start()
	{
		// Set up the MIDI player
		midiPlayer = FindObjectOfType<MidiFilePlayer>();

		// You can change the MIDI file by replacing this string with a title in the MidiDB folder, e.g., "Bach - Fugue".
		midiPlayer.MPTK_MidiName = "Heaven Can Wait";

		// Create graphics when a MIDI event occurs.
		midiPlayer.OnEventNotesMidi.AddListener(NotesToPlay);

		// Turn the flag that indicates the MIDI is playing to false when the MIDI stops playing.
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
	}

	// Create graphics when a MIDI event occurs.
	public void NotesToPlay(List<MPTKEvent> midiEvents)
	{

	}

	// Turn the flag that indicates the MIDI is playing to false when the MIDI stops playing.
	public void EndPlay(string name, EventEndMidiEnum reason)
	{
		midiIsPlaying = false;
	}

	// Processing style map function, i.e., map a value between one range to another range.
	public float mapf(float x, float min1, float max1, float min2, float max2)
	{
		// Find x as a percentage in a range between min1 & max1
		float percentage = Mathf.InverseLerp(min1, max1, x);

		// Return the corresponding value when you apply the percentage from before into another range
		return Mathf.Lerp(min2, max2, percentage);
	}
}