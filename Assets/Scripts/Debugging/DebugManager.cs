using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MiniJameGam9.Debugging
{
    public class DebugManager : MonoBehaviour
    {
        public static DebugManager Instance { private set; get; }

        private void Awake()
        {
            Instance = this;
        }

        private bool IsDebugEnabled => Application.isEditor;

        private Dictionary<string, RaycastInfo> _raycasts = new();

        public bool Raycast(string id, Vector3 origin, Vector3 direction, Color color, out RaycastHit hit, int layer = 1)
        {
            var isHit = Physics.Raycast(origin, direction, out hit, float.PositiveInfinity, layer);
            if (IsDebugEnabled)
            {
                var raycast = new RaycastInfo(origin, isHit ? hit.point : origin + (direction.normalized * 100f), color);
                if (_raycasts.ContainsKey(id))
                    _raycasts[id] = raycast;
                else
                    _raycasts.Add(id, raycast);
            }
            return isHit;
        }

        private void OnDrawGizmos()
        {
            // Printing all raycast
            foreach (var r in _raycasts)
            {
                Gizmos.color = r.Value.Color;
                Gizmos.DrawLine(r.Value.Origin, r.Value.Destination);
            }
            for (int i = _raycasts.Keys.Count - 1; i >= 0; i--) // TODO: Refactor this
            {
                var currKey = _raycasts.Keys.ToArray()[i];
                if (DateTime.Now > _raycasts[currKey].ExpireTime)
                    _raycasts.Remove(currKey);
            }
        }
    }
}
