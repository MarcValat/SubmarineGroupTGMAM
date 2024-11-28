using UnityEngine;

public class SubmarineCollision : MonoBehaviour
{
    public Texture2D mapTexture;
    public Vector2 mapSize = new Vector2(10f, 10f);
    public ShakeEffect shakeEffect;

    private bool hasShaken = false;

    public void CheckCollision(Vector2 currentPosition)
    {
        // Normaliser la position par rapport à la taille de la carte
        Vector2 normalizedPosition = NormalizePosition(new Vector3(currentPosition.x, 0, currentPosition.y));

        // Vérifier si la position est proche d'un bord ou d'une zone de collision
        if (IsNearEdge(normalizedPosition))
        {
            if (!hasShaken)
            {
                Debug.Log("Collision détectée ou proche d'un bord !");
                
                if (shakeEffect != null)
                {
                    StartCoroutine(shakeEffect.Shake(0.5f, 0.2f)); // Tremblement pendant 0.5s avec une intensité de 0.2f
                    hasShaken = true;
                }
                else
                {
                    Debug.LogWarning("ShakeEffect n'est pas assigné dans SubmarineCollision !");
                }
            }
        }
        else
        {
            hasShaken = false; // Réinitialiser le tremblement si le sous-marin s'éloigne des bords
        }
    }

    private Vector2 NormalizePosition(Vector3 position)
    {
        float normalizedX = Mathf.InverseLerp(-mapSize.x / 2, mapSize.x / 2, position.x);
        float normalizedY = Mathf.InverseLerp(-mapSize.y / 2, mapSize.y / 2, position.z);
        return new Vector2(normalizedX, normalizedY);
    }

    private bool IsNearEdge(Vector2 normalizedPosition)
    {
        int pixelX = Mathf.RoundToInt(normalizedPosition.x * (mapTexture.width - 1));
        int pixelY = Mathf.RoundToInt(normalizedPosition.y * (mapTexture.height - 1));

        int searchRadius = 3;

        for (int x = -searchRadius; x <= searchRadius; x++)
        {
            for (int y = -searchRadius; y <= searchRadius; y++)
            {
                int sampleX = Mathf.Clamp(pixelX + x, 0, mapTexture.width - 1);
                int sampleY = Mathf.Clamp(pixelY + y, 0, mapTexture.height - 1);

                Color pixelColor = mapTexture.GetPixel(sampleX, sampleY);

                if (pixelColor == Color.white)
                {
                    return true;
                }
            }
        }

        return false;
    }
}