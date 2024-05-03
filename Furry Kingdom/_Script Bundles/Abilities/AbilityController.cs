using System.Collections.Generic;
using UnityEngine;

namespace Furry
{
    public class AbilityController : MonoBehaviour
    {
        [Header("Ability references.")]
        [Tooltip("Possible abilities this object can have.")]
        [SerializeField] private List<Ability> _optionalAbilities;
        [Tooltip("Possible abilities this object can be immune to.")]
        [SerializeField] private List<Ability> _immuneToAbilities;
        [Tooltip("The ability this object can start with.")]
        [SerializeField] private Ability _startingAbility;

        private Dictionary<string, Ability> _possibleAbilitiesDictionary;
        private Dictionary<string, Ability> _immuneToAbilitiesDictionary;
        private Ability _currentAbility;

        private void Awake()
        {
            _possibleAbilitiesDictionary = new Dictionary<string, Ability>();
            _immuneToAbilitiesDictionary = new Dictionary<string, Ability>();
        }

        private void Start()
        {
            InitializePossibleAbilitiesDict();
            InitializeImmuneAbilitiesDict();
            if (_startingAbility != null)
            {
                if (!_possibleAbilitiesDictionary.ContainsKey(_startingAbility.name))
                {
                    _possibleAbilitiesDictionary.Add(_startingAbility.name, _startingAbility);
                }
                SetNewAbility(_startingAbility.name);
            }
        }

        /// <summary>
        /// Get all Abilities in the _optionalAbilities list and the starting ability and puts them in a new dictionary so they can be easily referenced later.
        /// </summary>
        public void InitializePossibleAbilitiesDict()
        {
            foreach (Ability ability in _optionalAbilities)
            {
                _possibleAbilitiesDictionary.Add(ability.name, ability);
            }
        }
        /// <summary>
        /// Get all Abilities in the _immuneToAbilities list and puts them in a new dictionary so they can be easily referenced later.
        /// </summary>
        public void InitializeImmuneAbilitiesDict()
        {
            foreach (Ability ability in _immuneToAbilities)
            {
                _immuneToAbilitiesDictionary.Add(ability.name, ability);
            }
        }

        /// <summary>
        /// Sets the current ability to an ability referenced by name in the object's ability dictionary if it is present.
        /// </summary>
        /// <param name="newAbility"></param>
        public void SetNewAbility(string newAbility)
        {
            if (_currentAbility == null || _currentAbility.name != newAbility)
            {
                _currentAbility = _possibleAbilitiesDictionary[newAbility];
            }
        }

        /// <summary>
        /// Returns the current ability name.
        /// </summary>
        /// <returns></returns>
        public string GetAbilityName()
        {
            if (_currentAbility == null)
            {
                return _currentAbility.name;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Checks whether the object is immune to an ability by checking the name reference in its immune dictionary.
        /// </summary>
        /// <param name="abilityName"></param> Ability name to check.
        public bool CheckImmunity(string abilityName)
        {
            return _immuneToAbilitiesDictionary.ContainsKey(abilityName);
        }

        /// <summary>
        /// Use the current ability.
        /// </summary>
        /// <param name="owner"></param> Game object using the ability.
        /// <param name="target"></param> Target.
        public void UseCurrentAbility(GameObject owner, GameObject target)
        {
            if (_currentAbility == null)
            {
                _currentAbility.Use(owner, target);
            }
        }

    }
}