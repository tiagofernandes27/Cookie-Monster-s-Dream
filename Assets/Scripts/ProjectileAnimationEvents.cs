using UnityEngine;

public class ProjectileAnimationEvents : MonoBehaviour
{


    private PotatoeProjectile parentProjectile;


    private void Awake()
    {
        parentProjectile = GetComponentInParent<PotatoeProjectile>();
    }

    // Called from Animation Event
    public void DestroyAfterExplosion()
    {
        if (parentProjectile != null)
            parentProjectile.DestroyAfterExplosion();
    }


}
