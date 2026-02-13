using UnityEngine;
using AshfallFrontier.Items;

namespace AshfallFrontier.Core
{
    /// <summary>
    /// Saves immediately whenever the attached Inventory changes.
    /// Attach to the Player alongside Inventory + SaveBinder.
    /// </summary>
    [RequireComponent(typeof(Inventory))]
    public class AutoSaveOnInventoryChange : MonoBehaviour
    {
        private Inventory _inv;
        private SaveBinder _binder;
        private SaveSystem _save;

        void Awake()
        {
            _inv = GetComponent<Inventory>();
            _binder = GetComponent<SaveBinder>();
        }

        void Start()
        {
            _save = FindFirstObjectByType<SaveSystem>();
            if (_inv != null)
                _inv.OnChanged += HandleChanged;
        }

        void OnDestroy()
        {
            if (_inv != null)
                _inv.OnChanged -= HandleChanged;
        }

        void HandleChanged()
        {
            if (_save == null) _save = FindFirstObjectByType<SaveSystem>();
            if (_save == null || _binder == null) return;

            _save.SaveFrom(_binder);
        }
    }
}
