using UnityEngine;

public class EnemyRatCombat : MonoBehaviour
{


    [SerializeField] private int damage = 1;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<PlayerHealth>()?.ChangeHealth(-damage);
    }


}
