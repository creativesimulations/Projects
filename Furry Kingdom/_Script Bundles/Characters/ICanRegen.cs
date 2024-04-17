
using UnityEngine;

namespace Furry
{

    public interface ICanRegen
    {

        [SerializeField] int RegenAmount { get; set; }
        [SerializeField] float RegenSpeed { get; set; }
    }

}