using UnityEngine;

namespace MudOverload.Game.Player
{
    public class PlayerHoldingTilesController : MonoBehaviour
    {
        private static PlayerHoldingTilesController Singleton;

        public static void AddTile(Sprite sprite)
        {
            if (Singleton == null) return;

            var newTile = Instantiate(Singleton.template, Singleton.transform);
            var randomPositionSize = Singleton.randomPositionSize;
            newTile.transform.localPosition = new Vector3(Random.Range(-randomPositionSize, randomPositionSize), Random.Range(-randomPositionSize, randomPositionSize), Random.Range(-randomPositionSize, randomPositionSize));
            newTile.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            newTile.sprite = sprite;
            newTile.gameObject.SetActive(true);
        }

        public static void ClearTiles()
        {
            if (Singleton == null) return;

            foreach(Transform child in Singleton.transform)
            {
                if (!child.gameObject.activeSelf) continue;

                Destroy(child.gameObject);
            }
        }

        public static void RemoveLastTile()
        {
            if (Singleton == null) return;

            foreach (Transform child in Singleton.transform)
            {
                if (!child.gameObject.activeSelf) continue;
                var index = child.GetSiblingIndex();
                if (index != Singleton.transform.childCount - 1) continue;

                Destroy(child.gameObject);
            }
        }

        [SerializeField]
        private float randomPositionSize = 1f;

        [SerializeField]
        private SpriteRenderer template;

        private void Awake()
        {
            Singleton = this;

            template.gameObject.SetActive(false);
        }
    }
}
