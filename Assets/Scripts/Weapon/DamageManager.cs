using MiniJameGam9.Character;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MiniJameGam9.Weapon
{
    public class DamageManager : MonoBehaviour
    {
        public static DamageManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public Dictionary<Profile, Dictionary<Profile, int>> _damages = new();

        public Profile GetAssist(Profile target, Profile except)
        {
            return _damages[target].Where(x => x.Key != except).OrderByDescending(x => x.Value).Select(x => x.Key).FirstOrDefault();
        }

        public void AddDeath(Profile p)
        {
            _damages.Remove(p);
        }

        public void AddDamage(Profile target, Profile attacker, int damage)
        {
            if (!_damages.ContainsKey(target))
            {
                _damages.Add(target, new());
            }
            if (!_damages[target].ContainsKey(attacker))
            {
                _damages[target].Add(attacker, damage);
            }
            else
            {
                _damages[target][attacker] += damage;
            }
        }
    }
}
