using UnityEngine;

public class BoatWheelRotation : MonoBehaviour
{
    public float rotationDuration = 2.5f; // Time for each 80-degree rotation
    public float rotationStep = 80f;      // How much to rotate each press
    public float interactionDistance = 2.5f; // Max distance to rotate the wheel
    public AudioClip rotationSound;       // Drag & drop sound here
    [Range(0f, 1f)] public float soundVolume = 1f; // Volume slider

    private bool isRotating = false;
    private float rotationTime = 0f;
    private float startRotation;
    private float targetRotation;
    private AudioSource audioSource;
    private Transform player; // Reference to player

    void Start()
    {
        startRotation = transform.localEulerAngles.z; // Store initial rotation
        targetRotation = startRotation; // Keep track of target rotation

        // Find the player safely to prevent NullReferenceException
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform; // Assign player transform
        }
        else
        {
            Debug.LogWarning("Player not found! Make sure your player GameObject is tagged 'Player'.");
        }

        // Add an AudioSource if not already present
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        // Only allow interaction if player exists and is near the wheel
        if (player != null && Vector3.Distance(transform.position, player.position) <= interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.E) && !isRotating)
            {
                StartRotation();
            }
        }

        if (isRotating)
        {
            RotateWheel();
        }
    }

    void StartRotation()
    {
        isRotating = true;
        rotationTime = 0f;
        startRotation = transform.localEulerAngles.z; // Start from current rotation
        targetRotation = startRotation + rotationStep; // Increase rotation by 80°

        if (rotationSound != null)
        {
            audioSource.PlayOneShot(rotationSound, soundVolume);
        }
    }

    void RotateWheel()
    {
        rotationTime += Time.deltaTime;
        float progress = rotationTime / rotationDuration;

        // Smoothly rotate from current position to the new target rotation
        float newZ = Mathf.Lerp(startRotation, targetRotation, progress);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, newZ);

        // Stop rotating once finished
        if (progress >= 1f)
        {
            isRotating = false;
        }
    }
}
