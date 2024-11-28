using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CrankRotation : MonoBehaviour
{
    public Transform radar;
    public Transform submarineSprite;
    private Quaternion previousRotation;
    private float totalRotationX;
    public float rotationSpeed = 20f;

    void Start()
    {
        // Sauvegarde la rotation initiale du crank au début
        previousRotation = transform.localRotation;
        totalRotationX = 0f;
    }

    void Update()
    {
        // Calculer la différence de rotation entre la frame précédente et la frame actuelle
        Quaternion currentRotation = transform.localRotation;
        Quaternion deltaRotation = currentRotation * Quaternion.Inverse(previousRotation);

        // Extraire l'angle de rotation sur l'axe X à partir du quaternion
        Vector3 eulerDeltaRotation = deltaRotation.eulerAngles;
        float deltaX = eulerDeltaRotation.x;

        // Gérer la transition de l'angle pour éviter les sauts brusques à 360°
        if (deltaX > 180f) deltaX -= 360f;

        // Ajouter la différence d'angle à la rotation totale
        totalRotationX += deltaX;

        // Vérifier la direction de rotation sur l'axe X
        if (deltaX > 0) RotateRadar(-1);
        else if (deltaX < 0) RotateRadar(1);

        // Mettre à jour la rotation précédente
        previousRotation = currentRotation;
    }

    void RotateRadar(float direction)
    {
        // Sauvegarder la rotation locale actuelle du crank
        Quaternion crankLocalRotation = transform.localRotation;

        // Tourner le sous-marin
        radar.Rotate(Vector3.up, direction * rotationSpeed * Time.deltaTime);

        // Tourner le sprite du sousmarin
        Vector3 currentEulerAngles = submarineSprite.localEulerAngles;
        currentEulerAngles.y += -direction * rotationSpeed * Time.deltaTime;
        submarineSprite.localEulerAngles = currentEulerAngles;

        // Réappliquer la rotation locale du crank pour qu'elle ne soit pas affectée par la rotation du sous-marin
        transform.localRotation = crankLocalRotation;
    }
}
