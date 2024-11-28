using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRInteractionController : MonoBehaviour
{
    public enum InteractionType { Rotate, Translate }
    public InteractionType interactionType;

    public Transform referenceTransform; // Référence pour le pivot ou la direction
    public float maxLeverDistance = 0.2f; // Maximum de déplacement (pour les leviers)
    public float rotationSpeed = 100f; // Vitesse de rotation (pour les cranks)

    private XRBaseInteractor interactor;
    private Vector3 initialInteractorPosition;
    private Quaternion initialObjectRotation;
    private Vector3 initialObjectPosition;

    private Vector3 lastInteractorPosition; // Stocke la dernière position de l'interactor

    void Start()
    {
        if (interactionType == InteractionType.Translate)
            initialObjectPosition = transform.localPosition;
        else if (interactionType == InteractionType.Rotate)
            initialObjectRotation = transform.localRotation;
    }

    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Stocker les données initiales lorsque l'interaction commence
        this.interactor = args.interactor;
        initialInteractorPosition = args.interactor.transform.position;
        lastInteractorPosition = initialInteractorPosition;

        if (interactionType == InteractionType.Translate)
            initialObjectPosition = transform.localPosition;
        else if (interactionType == InteractionType.Rotate)
            initialObjectRotation = transform.localRotation;
    }

    public void OnSelectExited(SelectExitEventArgs args)
    {
        // Réinitialiser l'interactor lorsque l'utilisateur relâche l'objet
        if (this.interactor == args.interactor)
            this.interactor = null;
    }

    void Update()
    {
        if (interactor == null) return;

        // Calculer la différence de position entre la position initiale et la position actuelle du contrôleur
        Vector3 interactorDelta = interactor.transform.position - initialInteractorPosition;

        if (interactionType == InteractionType.Translate)
        {
            // Translation : déplacement sur l'axe Z local
            float zMovement = Vector3.Dot(interactorDelta, referenceTransform.forward);
            Vector3 newPosition = initialObjectPosition + new Vector3(0, 0, zMovement);

            // Limiter le mouvement
            newPosition.z = Mathf.Clamp(newPosition.z, -maxLeverDistance, maxLeverDistance);
            transform.localPosition = newPosition;
        }
        else if (interactionType == InteractionType.Rotate)
        {
            // Obtenir la position actuelle et la position précédente du contrôleur
            Vector3 currentInteractorPosition = interactor.transform.position;
            Vector3 currentDirection = (currentInteractorPosition - referenceTransform.position).normalized;
            Vector3 lastDirection = (lastInteractorPosition - referenceTransform.position).normalized;

            // Calculer l'angle uniquement autour de l'axe X
            float angle = Vector3.SignedAngle(lastDirection, currentDirection, referenceTransform.up); // Utiliser l'axe UP pour la direction circulaire

            // Appliquer la rotation uniquement sur l'axe X
            transform.localRotation *= Quaternion.Euler(0, angle * rotationSpeed * Time.deltaTime, 0);

            // Mettre à jour la dernière position du contrôleur
            lastInteractorPosition = currentInteractorPosition;
        }

    }
}
