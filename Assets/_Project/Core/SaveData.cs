using System;
using System.Collections.Generic;
using UnityEngine;

namespace AshfallFrontier.Core
{
    [Serializable]
    public class SaveData
    {
        public string version = "m1";

        public float px;
        public float py;
        public float pz;

        public List<ItemRow> items = new();

        [Serializable]
        public class ItemRow
        {
            public string id;
            public int qty;
        }

        public Vector3 PlayerPos() => new(px, py, pz);
        public void SetPlayerPos(Vector3 p) { px = p.x; py = p.y; pz = p.z; }
    }
}
