using MudOverload.Game.Player;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MudOverload.Game
{
    [Serializable]
    public struct NamedSprite
    {
        public string name;
        public Sprite sprite;
        public TileBase tile;
    }

    public class TerrainController : MonoBehaviour
    {
        private static TerrainController Singleton;

        [SerializeField]
        private NamedSprite[] spriteList;

        public static TileBase GetTile(Vector2 position)
        {
            if (Singleton == null) return null;

            return Singleton.tilemap.GetTile(Singleton.tilemap.WorldToCell(position));
        }

        public static void SetTile(Vector2 position, string tileName)
        {
            if (Singleton == null) return;

            var tilePos = Singleton.tilemap.WorldToCell(position);

            TileBase selectedTile = null;
            foreach (var namedSprite in Singleton.spriteList)
            {
                if (namedSprite.name == tileName)
                {
                    selectedTile = namedSprite.tile;
                }
            }

            if (selectedTile != null)
            {
                Singleton.tilemap.SetTile(tilePos, selectedTile);
            }
        }

        public static string MineTile(Vector2 position)
        {
            if (Singleton == null) return "";

            var cellPosition = Singleton.tilemap.WorldToCell(position);

            var currentTile = Singleton.tilemap.GetTile(cellPosition);
            foreach (var namedSprite in Singleton.spriteList)
            {
                if (namedSprite.name == currentTile.name)
                {
                    MiningEffect.MiningEffectManager.CreateEffect(namedSprite.sprite, new Vector2(cellPosition.x, cellPosition.y), PlayerController.GetMiningEffectTargetPosition());
                }
            }

            Singleton.tilemap.SetTile(cellPosition, null);

            return currentTile.name;
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
