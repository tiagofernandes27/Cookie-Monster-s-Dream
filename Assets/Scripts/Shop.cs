using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private ShopUI shopUI;

    private void Start()
    {
        shopUI = ShopUI.Instance;
        shopUI.CloseShop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            
            shopUI.OpenShop();

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player) && shopUI != null)
        {
            shopUI.CloseShop();
        }
    }
}
