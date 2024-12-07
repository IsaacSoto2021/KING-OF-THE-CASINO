using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading_UI : MonoBehaviour
{
    public AudioSource audioSource;  // The AudioSource to play/stop the sound
    public string triggerTag = "Player"; // Tag to identify which object triggers the event
    private bool isAudioPlaying = false;  // To track if the audio is currently playing
    private float audioPlayTime = 3f;    // Time duration to play the audio (in seconds)
    private float timer = 0f;             // Timer to track the play duration

    // This method is called when another object enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is tagged as the triggerTag (default "Player")
        if (other.CompareTag(triggerTag))
        {
            // Start playing the audio if it's not already playing
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();  // Play the currently assigned audio clip in the AudioSource
                isAudioPlaying = true;  // Set the audio as playing
                timer = audioPlayTime;  // Set the timer to 10 seconds
                Debug.Log("Audio started because the player entered the trigger zone.");
            }
        }
    }

    // This method is called when another object exits the trigger zone
    private void OnTriggerExit(Collider other)
    {
        // Check if the object exiting the trigger is tagged as the triggerTag (default "Player")
        if (other.CompareTag(triggerTag))
        {
            // We do not stop the audio here; it will keep playing for the remaining time
            Debug.Log("Player exited the trigger zone. Audio will continue for 10 seconds.");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // If the audio is playing, count down the timer
        if (isAudioPlaying)
        {
            timer -= Time.deltaTime;  // Decrease the timer by the time passed since last frame

            // If the timer reaches 0, stop the audio
            if (timer <= 0f)
            {
                audioSource.Stop();  // Stop the audio
                isAudioPlaying = false;  // Set the audio as not playing
                Debug.Log("Audio stopped after 3 seconds.");
            }
        }
    }
}
