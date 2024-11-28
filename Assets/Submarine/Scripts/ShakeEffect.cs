using UnityEngine;
using System.Collections;

public class ShakeEffect : MonoBehaviour
{
    public Transform cameraToShake;

    public IEnumerator Shake(float duration, float magnitude)
    {
        if (cameraToShake == null)
        {
            Debug.LogWarning("Aucune caméra assignée à cameraToShake. Assignez une caméra dans l'inspecteur.");
            yield break;
        }

        Vector3 originalPosition = cameraToShake.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            cameraToShake.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        cameraToShake.localPosition = originalPosition;
    }
}
