using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Furry
{


    public class AbilityController : MonoBehaviour
{
        [SerializeField] private List<ScriptableObject> _optionalAbilities;
        [SerializeField] private List<ScriptableObject> _immuneToAbilities;
        [SerializeField] private ScriptableObject _startingAbility;

        public Dictionary<string, ScriptableObject> PossibleAbilitiesDictionary { get; private set; }
        public Dictionary<string, ScriptableObject> ImmuneToAbilitiesDictionary {get; private set;}
        public ScriptableObject CurrentAbility { get; private set; }

        private void Awake()
        {
            PossibleAbilitiesDictionary = new Dictionary<string, ScriptableObject>();
            ImmuneToAbilitiesDictionary = new Dictionary<string, ScriptableObject>();
        }

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
                PossibleAbilitiesDictionary.Add(ability.name, ability);
            }
            foreach (ScriptableObject ability in _immuneToAbilities)
            {
                ImmuneToAbilitiesDictionary.Add(ability.name, ability);
            }
        }

        public void SetNewAbility(string newAbility)
        {
            if (CurrentAbility == null || CurrentAbility.name != newAbility)
            {
                CurrentAbility = PossibleAbilitiesDictionary[newAbility];
            }
        }

        public void CheckImmunity(string abilityName)
        {
        }

    }
}