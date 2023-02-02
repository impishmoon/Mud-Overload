using UnityEngine;
using UnityEngine.Tilemaps;

namespace MudOverload.Game
{
	public class TerrainController : MonoBehaviour
	{
		private static TerrainController Singleton;

        public static TileBase getTile(Vector2 position)
        {
            if (Singleton == null) return null;

            return Singleton.tilemap.GetTile(Singleton.tilemap.WorldToCell(position));
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
