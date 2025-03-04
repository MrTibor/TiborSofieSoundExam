using UnityEngine;

public class SailMovement : MonoBehaviour
{
    public float swaySpeed = 1f;  // Speed of movement
    public float swayAmount = 10f; // Angle range in degrees

    private float angle = 0f;

    void Update()
    {
        angle = Mathf.Sin(Time.time * swaySpeed) * swayAmount; // Create smooth movement
        transform.localRotation = Quaternion.Euler(angle, 0, 0); // Rotate around X-axis
    }
}
