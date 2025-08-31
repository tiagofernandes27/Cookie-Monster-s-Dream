using UnityEngine;

public class EnemyPressureCookerAnimator : MonoBehaviour
{


    private const string IS_EXPLODING = "IsExploding";


    [SerializeField] private Enemy parentEnemy; // Reference to parent Enemy script


    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool(IS_EXPLODING, parentEnemy.IsExploding());
    }

    // This method is called by an Animation Event
    public void OnExplosionFrame()
    {
        if (parentEnemy != null)
        {
            parentEnemy.DealExplosionDamage();
        }
    }

    public void Explode()
    {
        if (parentEnemy != null)
        {
            parentEnemy.Explode();
        }
    }


}
