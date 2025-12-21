using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;       // vitesse de déplacement
    [SerializeField] private float rotationSpeed = 90f;  // vitesse de rotation en degrés par seconde
    [SerializeField] private float pauseDuration = 2f;   // temps de pause à la fin du tour

    private CharacterController controller;
    private float speed;        // vitesse actuelle pour l'Animator
    private float angle;        // angle pour le cercle
    private bool isPaused = false;
    private float pauseTimer = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!isPaused)
        {
            // --- Déplacement circulaire ---
            angle += rotationSpeed * Time.deltaTime;

            // On boucle l'angle après 360°
            if (angle >= 360f)
            {
                angle = 0f;
                isPaused = true;     // activer la pause
                pauseTimer = 0f;
            }

            // Direction tangentielle du cercle
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));
            Vector3 velocity = direction.normalized * moveSpeed;

            controller.Move(velocity * Time.deltaTime);

            // Orientation du personnage
            if (velocity != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), 0.1f);

            // Mise à jour de la vitesse pour Animator
            speed = velocity.magnitude;
        }
        else
        {
            // --- Pause ---
            pauseTimer += Time.deltaTime;
            speed = 0f; // personnage immobile

            if (pauseTimer >= pauseDuration)
            {
                isPaused = false; // reprendre le mouvement
            }
        }
    }

    // Getter pour l'Animator
    public float GetMoveSpeed()
    {
        return speed;
    }
}
