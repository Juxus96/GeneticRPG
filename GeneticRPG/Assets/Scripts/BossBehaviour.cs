using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    public enum actionType
    {
        ATTACK,
        DEFEND,
        NONE
    }

    public actionType[] bossBehaviour;
}
