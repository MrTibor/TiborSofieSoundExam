using UnityEngine;

public class SailSound : MonoBehaviour
{
    public AudioClip sailSound; // Drag & drop sound here
    [Range(0f, 1f)] public float maxVolume = 0.3f; // Max volume when closest
    public float maxDistance = 10f; // Distance at which volume is 0

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

        // Add an AudioSource to play the sail sound
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = true; // Keeps playing
        if (sailSound != null)
        {
            audioSource.clip = sailSound;
            audioSource.volume = 0f; // Start with no sound until adjusted
            audioSource.Play();
        }
    }

    void Update()
    {
        AdjustSailVolume();
    }

    void AdjustSailVolume()
    {
        if (player != null && sailSound != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            // Calculate volume (closer = higher volume, further = lower)
            float volume = Mathf.Clamp(1 - (distance / maxDistance), 0.05f, maxVolume);

            audioSource.volume = volume;
        }
    }
}
