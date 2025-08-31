using UnityEngine;

public class EnemyRatAnimator : MonoBehaviour
{


    private const string IS_WALKING = "IsWalking";


    [SerializeField] private EnemyRatMovement enemyRatMovement;


    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool(IS_WALKING, enemyRatMovement.IsWalking());
    }


}
