using System.Collections.Generic;
using UnityEngine;

namespace MudOverload.UI
{
	public class IsUIOpen : MonoBehaviour
	{
		private static IsUIOpen Singleton;
        private static Dictionary<string, bool> Users = new Dictionary<string, bool>();

        public static void AddUser(string name)
        {
            Users.Add(name, true);
        }

        public static void RemoveUser(string name)
        {
            Users.Remove(name);
        }

        public static bool AreThereUIUsers()
        {
            return Users.Count > 0;
        }

        private void Awake()
        {
            if (!Singleton)
            {
                DontDestroyOnLoad(gameObject);
                Singleton = this;
            }
        }
    }
}
