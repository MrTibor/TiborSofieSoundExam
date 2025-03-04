using UnityEngine;

public class RadioPlay : MonoBehaviour
{
    public AudioClip radioSound; // Drag & drop sound here
    [Range(0f, 1f)] public float soundVolume = 1f; // Volume slider
    public float interactionDistance = 3f; // Distance to activate radio

    private AudioSource audioSource;
    private Transform player;
    private bool isPlaying = false; // Track radio state

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

        // Add an AudioSource to play the radio sound
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = true; // Keeps playing until stopped
    }

    void Update()
    {
        // Check if player exists and is within range
        if (player != null && Vector3.Distance(transform.position, player.position) <= interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.E)) // Press "E" to toggle radio
            {
                ToggleRadio();
            }
        }
    }

    void ToggleRadio()
    {
        if (!isPlaying)
        {
            // Start playing
            if (radioSound != null)
            {
                audioSource.clip = radioSound;
                audioSource.volume = soundVolume;
                audioSource.Play();
                isPlaying = true;
            }
        }
        else
        {
            // Stop playing
            audioSource.Stop();
            isPlaying = false;
        }
    }
}
