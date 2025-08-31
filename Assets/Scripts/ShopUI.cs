using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum AbilityType
{
    Weapon,
    Health,
    AttackSpeed,
    Damage,
}

[System.Serializable]
public class AbilityEntry
{
    public Button button;
    public int cost;
    public string abilityName;
    public Animator lockAnimator;
    public GameObject shadow;
    public Weapon weapon;
    public AbilityType type;
    public bool unlocked;
    // For upgrades
    public int hpIncrease;
    public float attackSpeedIncrease;
    public float damageIncrease;
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
        if (ability.unlocked)
        {
            Debug.Log($"Ability {ability.abilityName} already unlocked.");
            return;
        }

        if (Player.Instance.PlayerMoney < ability.cost)
        {
            Debug.Log("Not enough money.");
            return;
        }
        
        Debug.Log($"Bought Ability " + ability.abilityName);

        Player.Instance.PlayerMoney -= ability.cost;
        ability.unlocked = true;

        switch (ability.type)
        {
            case AbilityType.Weapon:
                if (ability.weapon != null)
                {
                    if (ability.lockAnimator != null)
                        ability.lockAnimator.SetTrigger("StartLockAnimation");

                    WeaponHolder.Instance.UnlockWeapon(ability.weapon, true);
                    Debug.Log($"Unlocked weapon: {ability.abilityName}");
                }
                break;

            case AbilityType.Health:
                if (ability.hpIncrease > 0)
                    PlayerHealth.Instance.IncrementHealth(ability.hpIncrease);
                break;

            case AbilityType.AttackSpeed:
                if (ability.attackSpeedIncrease > 0)
                    ability.weapon.UpgradeAttackSpeed(ability.attackSpeedIncrease);
                break;
            case AbilityType.Damage:
                if (ability.damageIncrease > 0)
                    ability.weapon.UpgradeDamage(ability.damageIncrease);
                break;
        }
    }

    public void HideLock(GameObject lockpref)
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            if (lockpref != null && abilities[i].lockAnimator != null)
            {
                if (abilities[i].lockAnimator.gameObject == lockpref)
                {
                    abilities[i].lockAnimator.gameObject.SetActive(false);
                    abilities[i].shadow.SetActive(false);
                }
            }
        }
    }

    
}


