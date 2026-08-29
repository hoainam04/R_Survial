using System.Collections.Generic;
using UnityEngine;

namespace PROJ.Attributes
{
    [CreateAssetMenu(menuName = "PROJ/Attributes/Character Attribute Set")]
    public class CharacterAttributeSetSO : ScriptableObject
    {
        [SerializeField] private List<CharacterAttribute> attributes = new();

        public IReadOnlyList<CharacterAttribute> Attributes => attributes;
    }
}