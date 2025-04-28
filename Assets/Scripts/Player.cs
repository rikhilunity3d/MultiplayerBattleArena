using UnityEngine;

public class Player : Character
{
    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(h, 0, v);
        Move(moveDirection);
    }

    // Example: Override TakeDamage if needed later
    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        Debug.Log("Player took extra stylish damage effect!");
    }
}
