using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHolder : MonoBehaviour
{


    [System.Serializable]
    private class WeaponEntry
    {
        public string weaponName;
        public Weapon weapon; // Reference to prefab in WeaponHolder
        public bool storeUnlocked;   // permanent until run ends
        public bool sessionUnlocked; // temporary until death

        public bool IsUnlocked => storeUnlocked || sessionUnlocked;
    }

    [Header("Weapons")]
    [SerializeField] private List<WeaponEntry> meleeWeapons = new List<WeaponEntry>();
    [SerializeField] private List<WeaponEntry> rangedWeapons = new List<WeaponEntry>();

    private int currentMeleeIndex = 0;
    private int currentRangedIndex = 0;

    private Weapon activeMelee;
    private Weapon activeRanged;
    private Weapon activeVisual; // Only 1 is visible at a time

    private void Start()
    {
        // Initialize default unlocked weapons
        if (meleeWeapons.Count > 0)
        {
            activeMelee = meleeWeapons[currentMeleeIndex].weapon;
            UnlockWeapon(activeMelee, true); // Fists of Hunger
        }

        if (rangedWeapons.Count > 0)
        {
            activeRanged = rangedWeapons[currentRangedIndex].weapon;
            UnlockWeapon(activeRanged, true); // Spitting
        }

        UpdateVisualWeapon(activeMelee); // Default to melee visual
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        // Attack input
        if (Mouse.current.leftButton.isPressed && activeMelee != null)
        {
            activeMelee.Attack();
            UpdateVisualWeapon(activeMelee);
        }

        if (Mouse.current.rightButton.isPressed && activeRanged != null)
        {
            activeRanged.Attack();
            UpdateVisualWeapon(activeRanged);
        }

        // Switch melee weapon
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            CycleWeapon(meleeWeapons, ref currentMeleeIndex, ref activeMelee);
            UpdateVisualWeapon(activeMelee);
        }

        // Switch ranged weapon
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            CycleWeapon(rangedWeapons, ref currentRangedIndex, ref activeRanged);
            UpdateVisualWeapon(activeRanged);
        }
    }

    private void CycleWeapon(List<WeaponEntry> weaponList, ref int currentIndex, ref Weapon activeWeapon)
    {
        if (weaponList.Count <= 1) return;

        int startingIndex = currentIndex;

        do
        {
            currentIndex = (currentIndex + 1) % weaponList.Count;
        }
        while (!weaponList[currentIndex].IsUnlocked && currentIndex != startingIndex);

        activeWeapon = weaponList[currentIndex].weapon;
    }

    private void UpdateVisualWeapon(Weapon newVisualWeapon)
    {
        if (activeVisual == newVisualWeapon) return;

        // Disable all weapons visually
        foreach (var entry in meleeWeapons)
            if (entry.weapon != null) entry.weapon.SetVisualActive(false);

        foreach (var entry in rangedWeapons)
            if (entry.weapon != null) entry.weapon.SetVisualActive(false);

        // Enable only the new visual weapon
        if (newVisualWeapon != null)
        {
            newVisualWeapon.SetVisualActive(true);
            activeVisual = newVisualWeapon;
        }
    }

    // Call this from Store or Loot system
    public void UnlockWeapon(Weapon weapon, bool permanent)
    {
        if (weapon == null)
        {
            Debug.LogWarning("UnlockWeapon called with null weapon reference.");
            return;
        }

        WeaponEntry found = meleeWeapons.Find(w => w.weapon == weapon);
        if (found == null)
            found = rangedWeapons.Find(w => w.weapon == weapon);

        if (found != null)
        {
            if (permanent)
                found.storeUnlocked = true;

            found.sessionUnlocked = true; // always true when picked
        }
        else
        {
            Debug.LogWarning($"Weapon reference '{weapon.name}' not found in WeaponHolder lists!");
        }
    }

    // Call this on player death
    public void ResetSessionUnlocks()
    {
        foreach (var w in meleeWeapons)
            w.sessionUnlocked = false;
        foreach (var w in rangedWeapons)
            w.sessionUnlocked = false;
    }


}
