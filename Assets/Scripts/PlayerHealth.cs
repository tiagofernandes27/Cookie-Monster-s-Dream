using UnityEngine;

public class PlayerHealth : MonoBehaviour
{


    public static PlayerHealth Instance { get; private set; }


    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;


    private void Awake()
    {
        Instance = this;
    }

    public void IncrementHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth <= 0)
        {
            EnemyManager.Instance.StopWaves();
            RoomManager.Instance.TransitionToRoom(RoomManager.RoomType.Start);
            WeaponHolder.Instance.ResetSessionUnlocks();
        }
    }


}
