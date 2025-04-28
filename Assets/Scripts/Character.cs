using UnityEngine;

public class Character : MonoBehaviour
{
    // Encapsulated variables
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private int health = 100;

    // Public method for movement
    public virtual void Move(Vector3 direction)
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    // Public method for taking damage
    public virtual void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Health now: {health}");
    }

    public bool IsDead()
    {
        return health <= 0;
    }
}
