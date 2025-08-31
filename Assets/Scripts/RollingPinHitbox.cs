using UnityEngine;

public class RollingPinHitbox : MonoBehaviour
{


    [SerializeField] private RollingPinWeapon weapon;


    private void OnTriggerEnter2D(Collider2D other)
    {
        weapon?.OnHitCollider(other);
    }


}
