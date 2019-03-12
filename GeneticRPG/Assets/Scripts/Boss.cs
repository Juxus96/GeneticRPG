using UnityEngine;

public class Boss : MonoBehaviour
{
    public float damage = 30;
    public float health = 300;
    public float maxHealth = 500;
    public int attackTemp = 3;
    public bool IsDead { get; set; }

    public void Hit(float damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        IsDead = health <= 0;
       
    }
}
