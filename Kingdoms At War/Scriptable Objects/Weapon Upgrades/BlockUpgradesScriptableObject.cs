using UnityEngine;
using Core;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "Block Upgrades", menuName = "ScriptableObjects/Upgrade block for Weapons")]
    public class BlockUpgradesScriptableObject : ScriptableObject
    {

        public new string name;
        public string description;
        public float increaseAmountBy;

        [SerializeField] public UpgradeType upgradeType = 0;
        public enum UpgradeType // your custom enumeration
        {
            fireSpeed = 0,
            explosiveRadius = 1,
            explosiveAndUpwardForces = 2,
            newWeapon = 3
        };

    }
}