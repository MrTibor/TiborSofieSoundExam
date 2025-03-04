using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public AudioClip doorSound; // Drag & drop sound here
    [Range(0f, 1f)] public float soundVolume = 1f; // Volume slider
    public float interactionDistance = 3f; // Distance to activate door

    private AudioSource audioSource;
    private Transform player;

    void Start()
    {
        // Find player safely (Ensure player is tagged "Player")
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Player not found! Make sure your player GameObject is tagged 'Player'.");
        }

        // Add an AudioSource to play the door sound
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        // Check if player exists and is within range
        if (player != null && Vector3.Distance(transform.position, player.position) <= interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.E)) // Press "E" to play door sound
            {
                PlayDoorSound();
            }
        }
    }

    void PlayDoorSound()
    {
        if (doorSound != null)
        {
            audioSource.PlayOneShot(doorSound, soundVolume);
        }
    }
}
