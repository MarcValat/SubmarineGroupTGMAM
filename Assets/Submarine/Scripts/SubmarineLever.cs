using UnityEngine;

public class SubmarineLever : MonoBehaviour
{
    public Transform lever;
    public Transform radar;
    public SubmarineCollision submarineCollision;
    public float maxLeverDistance = 0.2f; // Maximum lever movement
    public float maxSpeed = 5f; // Max speed of the submarine
    public Vector2 currentPosition;
    public Vector2 previousNextPosition;

    private Vector3 leverInitialPosition;

    void Start()
    {
        // Store the initial lever position
        leverInitialPosition = lever.localPosition;
    }

    void Update()
    {
        // Vérifiez la collision en passant la position actuelle au script SubmarineCollision
        if (submarineCollision != null)
        {
            submarineCollision.CheckCollision(currentPosition);
        }

        // Limit lever movement to a maximum distance (forward and backward)
        Vector3 leverLocalPosition = lever.localPosition;
        leverLocalPosition.z = Mathf.Clamp(leverLocalPosition.z, -maxLeverDistance, maxLeverDistance);
        lever.localPosition = leverLocalPosition;

        // Calculate speed based on lever position (between -1 and 1, with 0 being neutral)
        float speedFactor = lever.localPosition.z / maxLeverDistance;

        // Move the submarine only if the lever is not in the neutral position (0)
        if (Mathf.Abs(speedFactor) > 0.01f) // Small threshold to avoid unnecessary recalculations
        {
            float speed = speedFactor * maxSpeed/100f * Time.deltaTime;

            // Get the current Y-axis rotation of the radar
            Quaternion submarineRotation = radar.rotation;

            // Calculate movement direction based on submarine's forward direction and current rotation
            Vector3 forwardMovement = submarineRotation * Vector3.forward * speed;

            // Update the current position by applying the forward movement
            currentPosition += new Vector2(forwardMovement.x, forwardMovement.z);

            // Calculate the new NextPosition
            previousNextPosition = currentPosition + new Vector2(forwardMovement.x, forwardMovement.z);
        }
    }
}
