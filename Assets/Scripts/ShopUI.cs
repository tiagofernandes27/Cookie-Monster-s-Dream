using System.Collections.Generic;
using TMPro;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AbilityEntry
{
    public Button button;
    public int cost;
    public string abilityName;
}

public class ShopUI : MonoBehaviour
{
    public static ShopUI Instance;

    [SerializeField] private GameObject shopPanel;

    [SerializeField] private List<AbilityEntry> abilities = new List<AbilityEntry>();

    private void Awake()
    {
        Instance = this;

        // Loop through all buttons and assign a listener
        for (int i = 0; i < abilities.Count; i++)
        {
            var ability = abilities[i]; // local copy
            abilities[i].button.onClick.AddListener(() => {
                Debug.Log("Click");
                BuyAbility(ability);
            });
        }
    }

    public void OpenShop() => shopPanel.SetActive(true);
    public void CloseShop() => shopPanel.SetActive(false);


    private void Start()
    {
        CloseShop();
    }

    private void BuyAbility(AbilityEntry ability)
    {
        Debug.Log("Selected Ability" + ability.abilityName);
        if (PlayerTest.Instance.playerMoney > ability.cost)
            Debug.Log($"Bought Ability "+ability.abilityName);
        else
            Debug.Log("Player does not have enough money");
    }

    
}


