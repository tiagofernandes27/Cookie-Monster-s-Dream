using UnityEngine;

public class LockAnimationEvent : MonoBehaviour
{


    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void HideLock()
    {
        ShopUI.Instance.HideLock(gameObject);
    }


}
