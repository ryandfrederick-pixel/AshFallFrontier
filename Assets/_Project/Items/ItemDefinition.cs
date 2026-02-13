using UnityEngine;

namespace AshfallFrontier.Items
{
    /// <summary>
    /// Lightweight item definition (data-only). Create via: Assets -> Create -> AshfallFrontier -> Item Definition.
    /// Place definitions under a Resources folder (e.g. Assets/Resources/Items) for auto-loading by id.
    /// </summary>
    [CreateAssetMenu(menuName = "AshfallFrontier/Item Definition", fileName = "Item_")]
    public class ItemDefinition : ScriptableObject
    {
        [Header("Identity")]
        [Tooltip("Unique id used for save/load. Example: 'potion_small'.")]
        public string id;

        public string displayName;

        [Header("Stacking")]
        public bool stackable = true;
        public int maxStack = 99;

        [Header("UI")]
        public Sprite icon;

        void OnValidate()
        {
            if (!string.IsNullOrWhiteSpace(id)) id = id.Trim();
            if (maxStack < 1) maxStack = 1;
        }
    }
}
