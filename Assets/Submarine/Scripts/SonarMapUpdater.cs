using UnityEngine;

public class SonarMapUpdater : MonoBehaviour
{
    public Transform radar;
    public Material sonarMaterial;
    public Vector2 mapSize = new Vector2(10f, 10f);
    public SubmarineLever submarineLever;
    public Texture2D mapTexture;

    private Vector2 nextPosition;
    private float timeSinceLastScan = 0f;
    private float scanFrequency;

    void Start()
    {
        Vector2 initialPosition = NormalizePosition(radar.position);
        sonarMaterial.SetVector("_Position", new Vector4(initialPosition.x, initialPosition.y, 0, 0));
        scanFrequency = sonarMaterial.GetFloat("_ScanFreq") * 2f;
    }

    void Update()
    {
        timeSinceLastScan += Time.deltaTime;

        if (timeSinceLastScan >= scanFrequency)
        {
            Vector2 leverNextPosition = submarineLever.previousNextPosition;
            nextPosition = NormalizePosition(new Vector3(leverNextPosition.x, 0, leverNextPosition.y));
            sonarMaterial.SetVector("_NextPosition", new Vector4(nextPosition.x, nextPosition.y, 0, 0));

            sonarMaterial.SetVector("_Position", sonarMaterial.GetVector("_NextPosition"));
            timeSinceLastScan = 0f;
        }
    }

    Vector2 NormalizePosition(Vector3 position)
    {
        float normalizedX = Mathf.InverseLerp(-mapSize.x / 2, mapSize.x / 2, position.x);
        float normalizedY = Mathf.InverseLerp(-mapSize.y / 2, mapSize.y / 2, position.z);
        return new Vector2(normalizedX, normalizedY);
    }
}
