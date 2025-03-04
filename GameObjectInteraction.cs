using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameObjectInteraction : MonoBehaviour
{
    public AudioClip interactionSound; // Drag & drop sound here
    [Range(0f, 1f)] public float soundVolume = 1f; // Volume slider
    public float interactionDistance = 3f; // Distance to interact
    public GameObject object1; // First object to appear
    public GameObject object2; // Second object to appear
    public float visibilityDuration = 3f; // Time objects stay visible
    private float lastInteractionTime = -15f; // Tracks time since last interaction
    public float interactionCooldown = 15f; // Cooldown in seconds

    private AudioSource audioSource;
    private Transform player;
    private bool isBlackingOut = false;
    private GameObject blackoutScreen;

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

        // Add an AudioSource to play the interaction sound
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Ensure objects start as invisible
        if (object1 != null) object1.SetActive(false);
        if (object2 != null) object2.SetActive(false);

        // Create the blackout screen UI dynamically
        CreateBlackoutScreen();
    }

    void Update()
    {
        // Check if player exists and is within range
        if (player != null && Vector3.Distance(transform.position, player.position) <= interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.E) && Time.time >= lastInteractionTime + interactionCooldown)
            {
                lastInteractionTime = Time.time; // Reset interaction timer
                StartCoroutine(BlackoutScreen());
                Interact();
            }
        }
    }

    void Interact()
    {
        // Play sound
        if (interactionSound != null)
        {
            audioSource.PlayOneShot(interactionSound, soundVolume);
        }

        // Show objects and start timer to hide them
        if (object1 != null) object1.SetActive(true);
        if (object2 != null) object2.SetActive(true);

        // Start coroutine to hide objects after 3 seconds
        StartCoroutine(HideObjectsAfterTime());
    }

    IEnumerator HideObjectsAfterTime()
    {
        yield return new WaitForSeconds(visibilityDuration);
        if (object1 != null) object1.SetActive(false);
        if (object2 != null) object2.SetActive(false);
    }

    IEnumerator BlackoutScreen()
    {
        isBlackingOut = true;

        // Set blackout screen to fully black
        blackoutScreen.GetComponent<Image>().color = new Color(0, 0, 0, 1);

        yield return new WaitForSeconds(2f);

        // Fade blackout screen back to transparent
        blackoutScreen.GetComponent<Image>().color = new Color(0, 0, 0, 0);

        isBlackingOut = false;
    }

    void CreateBlackoutScreen()
    {
        // Create a UI Canvas
        GameObject canvasObj = new GameObject("BlackoutCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        // Create the blackout image
        blackoutScreen = new GameObject("BlackoutScreen");
        blackoutScreen.transform.SetParent(canvasObj.transform);
        Image blackoutImage = blackoutScreen.AddComponent<Image>();
        blackoutImage.color = new Color(0, 0, 0, 0); // Start fully transparent

        // Set up full-screen UI properties
        RectTransform rectTransform = blackoutScreen.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }
}
