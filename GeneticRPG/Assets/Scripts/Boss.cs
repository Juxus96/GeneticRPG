using UnityEngine;

public class Boss : MonoBehaviour
{
    public float damage = 30;
    public float health = 500;
    public float maxHealth = 500;
    public int attackTemp = 3;
    //public bool defending = false;

    public void RecieveDamage(float damage)
    {
        /*if (!defending)*/ health -= damage;
    }
}
