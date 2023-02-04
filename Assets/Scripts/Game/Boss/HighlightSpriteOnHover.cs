using System.Collections.Generic;
using UnityEngine;

namespace MudOverload.Game.Boss
{
	public class HighlightSpriteOnHover : MonoBehaviour
	{
		private static Dictionary<string, List<HighlightSpriteOnHover>> groups = new Dictionary<string, List<HighlightSpriteOnHover>>();

        [SerializeField]
        private string groupId;
        [SerializeField]
        private Material material;

        private new SpriteRenderer renderer;

        private SpriteRenderer highlightRenderer;

        private void Awake()
        {
            renderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            groups.TryGetValue(groupId, out var group);
            if(group == null)
            {
                group = new List<HighlightSpriteOnHover>();
                groups.Add(groupId, group);
            }
            group.Add(this);
        }

        private void OnDisable()
        {
            groups.TryGetValue(groupId, out var group);
            if (group != null)
            {
                foreach (var component in group)
                {
                    component.Unhighlight();
                }

                group.Remove(this);
            }
        }

        public void SetEnabled(bool enabled)
        {
            this.enabled = enabled;
        }

        private void Highlight()
        {
            if(highlightRenderer == null)
            {
                var highlightRendererObj = new GameObject();
                highlightRendererObj.transform.parent = transform;
                highlightRendererObj.transform.localScale = Vector3.one * 1.1f;
                highlightRendererObj.transform.localPosition = Vector3.zero;
                highlightRendererObj.transform.localRotation = Quaternion.identity;
                highlightRenderer = highlightRendererObj.AddComponent<SpriteRenderer>();
                highlightRenderer.flipX = renderer.flipX;
                highlightRenderer.sprite = renderer.sprite;
                highlightRenderer.sortingOrder = renderer.sortingOrder + 1;
                highlightRenderer.material = material;
            }
            else
            {
                highlightRenderer.enabled = true;
            }
        }

        private void Unhighlight()
        {
            if (highlightRenderer)
            {
                highlightRenderer.enabled = false;
            }
        }

        private void OnMouseEnter()
        {
            groups.TryGetValue(groupId, out var group);
            if (group != null)
            {
                foreach(var component in group)
                {
                    component.Highlight();
                }
            }
        }

        private void OnMouseExit()
        {

            groups.TryGetValue(groupId, out var group);
            if (group != null)
            {
                foreach (var component in group)
                {
                    component.Unhighlight();
                }
            }
        }
    }
}
