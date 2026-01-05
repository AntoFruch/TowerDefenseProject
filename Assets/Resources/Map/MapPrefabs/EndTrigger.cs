using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<MonsterController>().animator.SetTrigger("Attack");
        }
    }
}
