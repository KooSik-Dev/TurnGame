using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;

    public Enemy Enemy1;
    public Enemy Enemy2;

    public bool PlayerTurn = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Enemy2.EnemyAttack();
        PlayerTurn = true;
    }

    public void EndTurn()
    {
        PlayerTurn = true;
        Enemy1.EnemyAttack();
        Enemy2.EnemyAttack();
        PlayerTurn = true;
    }
}
