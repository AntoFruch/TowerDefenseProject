using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;       // vitesse de déplacement
    [SerializeField] private float rotationSpeed = 90f;  // vitesse de rotation en degrés par seconde
    [SerializeField] private float pauseDuration = 2f;   // temps de pause à la fin du tour

    private CharacterController controller;
    private float speed;        // vitesse actuelle pour l'Animator

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
            
    }

    // Getter pour l'Animator
    public float GetMoveSpeed()
    {
        return speed;
    }
}
