using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Stats")]
    public float damage = 30;
    public float health = 300;
    public float maxHealth = 500;
    public int attackTemp = 3;
    public bool IsDead { get; set; }

    public BossBehaviour currentBehaviour;

    public void Hit(float damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);

        if (health <= 0)
            IsDead = true;
    }
}


