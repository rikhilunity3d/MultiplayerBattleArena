using UnityEngine;

public class Enemy : Character
{
    private void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * 2f);
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        Debug.Log("Enemy got hit! 👾");
    }
}
