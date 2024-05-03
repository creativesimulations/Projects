using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using System.Diagnostics;

namespace Furry
{
    public static class AbilityFactory
    {

        private static Dictionary<string, Type> _abilitiesByName;
        private static bool IsInitialized => _abilitiesByName != null;

        /// <summary>
        /// Get all Abilities and put them in a new dictionary so they can be easily referenced later.
        /// </summary>
        public static void InitializeFactory()
        {
            if (IsInitialized)
                return;

            var abilityTypes = Assembly.GetAssembly(typeof(Ability)).GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Ability)));

            _abilitiesByName = new Dictionary<string, Type>();

            foreach (var type in abilityTypes)
            {
                _abilitiesByName.Add(type.Name, type);
            }
        }

        /// <summary>
        /// Creates and returns an instance of an ability.
        /// </summary>
        /// <param name="abilitytype"></param> The name of the ability to create.
        /// <returns></returns>
        public static Ability GetAbility(string abilitytype)
        {
            InitializeFactory();

            if (_abilitiesByName.ContainsKey(abilitytype))
            {
                Type type = _abilitiesByName[abilitytype];
                var ability = Activator.CreateInstance(type) as Ability;
                return ability;
            }

            return null;
        }

        /// <summary>
        /// Returns a list of all of the ability names that are available.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllDictNames()
        {
            var keyNames = new List<string>();
            foreach (string type in _abilitiesByName.Keys)
            {
                keyNames.Add(type);
            }
            return keyNames;
        }
    }
}