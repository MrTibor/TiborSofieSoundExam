using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 2f;
    public float gravity = 9.81f;
    public float jumpHeight = 1.5f;

    public AudioClip jumpSound;
    [Range(0f, 1f)] public float jumpVolume = 1f;

    public AudioClip landingSound;
    [Range(0f, 1f)] public float landingVolume = 1f;

    public AudioClip coughingSound;
    [Range(0f, 1f)] public float coughingVolume = 1f;

    public AudioClip rainSound;
    [Range(0f, 1f)] public float rainVolume = 0.5f;

    public AudioClip timedSound;
    [Range(0f, 1f)] public float timedSoundVolume = 1f;
    public float timedSoundInterval = 50f; // Play every 50 seconds

    private CharacterController controller;
    private float xRotation = 0f;
    private Transform playerCamera;
    private Vector3 velocity;
    private bool isGrounded;
    private bool wasInAir;
    private AudioSource audioSource;
    private AudioSource rainAudioSource;
    private AudioSource timedSoundSource;
    private float coughingTimer = 30f;
    private float timedSoundTimer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        if (rainSound != null)
        {
            rainAudioSource = gameObject.AddComponent<AudioSource>();
            rainAudioSource.clip = rainSound;
            rainAudioSource.loop = true;
            rainAudioSource.playOnAwake = true;
            rainAudioSource.volume = rainVolume;
            rainAudioSource.Play();
        }

        timedSoundSource = gameObject.AddComponent<AudioSource>();
        timedSoundSource.playOnAwake = false;
        timedSoundSource.loop = false;

        timedSoundTimer = timedSoundInterval; // Start countdown at 50s
    }

    void Update()
    {
        bool previouslyGrounded = isGrounded;
        isGrounded = controller.isGrounded;

        MovePlayer();
        Jump();
        ApplyGravity();
        LookAround();
        HandleCoughing();
        HandleLandingSound(previouslyGrounded);
        HandleTimedSound();
        AdjustRainVolume();
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime);
    }

    void Jump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
            wasInAir = true;

            if (jumpSound != null)
            {
                audioSource.PlayOneShot(jumpSound, jumpVolume);
            }
        }
    }

    void ApplyGravity()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleLandingSound(bool wasGroundedBefore)
    {
        if (!wasGroundedBefore && isGrounded && wasInAir)
        {
            if (landingSound != null)
            {
                audioSource.PlayOneShot(landingSound, landingVolume);
            }
            wasInAir = false;
        }
    }

    void HandleCoughing()
    {
        coughingTimer -= Time.deltaTime;

        if (coughingTimer <= 0f)
        {
            if (coughingSound != null)
            {
                audioSource.PlayOneShot(coughingSound, coughingVolume);
            }
            coughingTimer = 30f;
        }
    }

    void HandleTimedSound()
    {
        timedSoundTimer -= Time.deltaTime;

        if (timedSoundTimer <= 0f)
        {
            if (timedSound != null)
            {
                timedSoundSource.PlayOneShot(timedSound, timedSoundVolume);
            }
            timedSoundTimer = timedSoundInterval; // Reset timer for next play
        }
    }

    void AdjustRainVolume()
    {
        if (rainAudioSource != null)
        {
            rainAudioSource.volume = rainVolume;
        }
    }
}
