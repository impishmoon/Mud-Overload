using UnityEngine;
using UnityEngine.Tilemaps;

namespace MudOverload.Game
{
	public class TerrainController : MonoBehaviour
	{
		private static TerrainController Singleton;

        public static TileBase GetTile(Vector2 position)
        {
            if (Singleton == null) return null;

            return Singleton.tilemap.GetTile(Singleton.tilemap.WorldToCell(position));
        }

        public static void MineTile(Vector2 position)
        {
            if (Singleton == null) return;

            Singleton.tilemap.SetTile(Singleton.tilemap.WorldToCell(position), null);
        }

        [HideInInspector]
        public Tilemap tilemap;

        private void Awake()
        {
            Singleton = this;

            tilemap = GetComponent<Tilemap>();
        }
    }
}
