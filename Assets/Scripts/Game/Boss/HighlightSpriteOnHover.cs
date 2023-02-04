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
        private SpriteRedFlasher redFlasher;

        private SpriteRenderer highlightRenderer;

        private bool isMouseDown = false;
        private float mouseDownTime = 0;
        private float timeToDestroy = 1.5f;

        private void Awake()
        {
            renderer = GetComponent<SpriteRenderer>();
            redFlasher = GetComponent<SpriteRedFlasher>();
        }

        private void OnEnable()
        {
            groups.TryGetValue(groupId, out var group);
            if (group == null)
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
                    component.SetRedFlashRedOverride(0);
                }

                group.Remove(this);
            }
        }

        private void Update()
        {
            if (isMouseDown)
            {
                var progress = Mathf.InverseLerp(mouseDownTime, mouseDownTime + timeToDestroy, Time.time);
                SetRedFlashRedOverride(progress);

                if (progress >= 1)
                {
                    print("dead");
                }
            }
            else
            {
                SetRedFlashRedOverride(0);
            }
        }

        public void SetEnabled(bool enabled)
        {
            this.enabled = enabled;
        }

        private void Highlight()
        {
            if (highlightRenderer == null)
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

        private void SetRedFlashRedOverride(float multiplier)
        {
            groups.TryGetValue(groupId, out var group);
            if (group != null)
            {
                foreach (var component in group)
                {
                    component.redFlasher.redOverrideA = multiplier;
                }
            }
        }

        private void OnMouseEnter()
        {
            groups.TryGetValue(groupId, out var group);
            if (group != null)
            {
                foreach (var component in group)
                {
                    component.Highlight();
                }
            }
        }

        private void OnMouseExit()
        {
            isMouseDown = false;

            groups.TryGetValue(groupId, out var group);
            if (group != null)
            {
                foreach (var component in group)
                {
                    component.Unhighlight();
                }
            }
        }

        private BossController.Limbs GroupIdToLimb()
        {
            if (groupId == "leftHand")
            {
                return BossController.Limbs.LEFT_HAND;
            }
            else if (groupId == "leftLeg")
            {
                return BossController.Limbs.LEFT_LEG;
            }
            else
            {
                return BossController.Limbs.RIGHT_LEG;
            }
        }

        private void OnMouseDown()
        {
            if (!this.enabled) return;

            BossController.KillLimb(GroupIdToLimb());
        }
    }
}
