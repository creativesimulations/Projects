using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Furry
{


    public class AbilityContainer : MonoBehaviour
{
        [SerializeField] private List<ScriptableObject> _optionalAbilities;
        [SerializeField] private List<ScriptableObject> _immuneToAbilities;
        [SerializeField] private ScriptableObject _startingAbility;

        private Dictionary<string, ScriptableObject> _possibleAbilitiesDictionary = new Dictionary<string, ScriptableObject>();
        private Dictionary<string, ScriptableObject> _immuneToAbilitiesDictionary = new Dictionary<string, ScriptableObject>();
        public ScriptableObject CurrentAbility { get; private set; }

        private void Start()
        {
            InitializeDictionaries();
            if (_startingAbility != null)
            {
                CurrentAbility = _startingAbility;
            }
        }
        /// <summary>
        /// Get all Abilities and put them in a new dictionary so they can be easily referenced later.
        /// </summary>
        public void InitializeDictionaries()
        {
            foreach (ScriptableObject ability in _optionalAbilities)
            {
                _possibleAbilitiesDictionary.Add(ability.name, ability);
            }
            foreach (ScriptableObject ability in _immuneToAbilities)
            {
                _immuneToAbilitiesDictionary.Add(ability.name, ability);
            }
        }

        public void SetNewAbility(string newAbility)
        {
            if (CurrentAbility == null || CurrentAbility.name != newAbility)
            {
                CurrentAbility = _possibleAbilitiesDictionary[newAbility];
            }
        }

        public bool GetImmuneEntry(string abilityName)
        {
            if (_immuneToAbilitiesDictionary.ContainsKey(abilityName))
            {
                return true;
            }
            return false;
        }

    }
}