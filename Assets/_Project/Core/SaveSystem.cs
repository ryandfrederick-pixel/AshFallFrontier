using System.IO;
using UnityEngine;

namespace AshfallFrontier.Core
{
    /// <summary>
    /// Minimal JSON save/load for M1.
    /// Stores a single-slot save in Application.persistentDataPath.
    /// </summary>
    public class SaveSystem : MonoBehaviour
    {
        public string filename = "ashfall_save.json";

        string PathOnDisk => System.IO.Path.Combine(Application.persistentDataPath, filename);

        public bool HasSave() => File.Exists(PathOnDisk);

        public void SaveFrom(SaveBinder binder)
        {
            if (!binder) return;
            var data = binder.Extract();

            var json = JsonUtility.ToJson(data, prettyPrint: true);
            File.WriteAllText(PathOnDisk, json);
            Debug.Log($"[SaveSystem] Saved to {PathOnDisk}");
        }

        public void LoadInto(SaveBinder binder)
        {
            if (!binder) return;
            if (!HasSave()) return;

            var json = File.ReadAllText(PathOnDisk);
            var data = JsonUtility.FromJson<SaveData>(json);
            binder.Apply(data);
            Debug.Log($"[SaveSystem] Loaded from {PathOnDisk}");
        }

        [ContextMenu("Delete Save")]
        public void DeleteSave()
        {
            if (!HasSave()) return;
            File.Delete(PathOnDisk);
            Debug.Log($"[SaveSystem] Deleted {PathOnDisk}");
        }
    }
}
