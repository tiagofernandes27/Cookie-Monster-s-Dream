using UnityEngine;

public class RollingPinAnimationEvents : MonoBehaviour
{


    [SerializeField] private RollingPinWeapon weapon;


    public void EnableHitbox()
    {
        weapon?.EnableHitbox();
    }

    public void DisableHitbox()
    {
        weapon?.DisableHitbox();
    }


}
