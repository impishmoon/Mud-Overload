using System.Collections.Generic;
using UnityEngine;

namespace MudOverload.Game.Boss
{
	public class BossCameraController : MonoBehaviour
	{
        public static BossCameraController Singletron;

        public float dampTime = 0.2f;
        public float screenEdgeBuffer = 4f;
        public float minSize = 3f;

        [HideInInspector]
        public Camera selfCamera;

        private Vector3 moveSpeed;
        private float zoomSpeed;

        private float lastShake = 0;
        private float shakeDuration = 0.5f;
        private float shakeMagnitude = 2;
        private Vector3 lastPosition = new Vector3(0, 0, -30);

        private List<Transform> nonPlayerTargets = new List<Transform>();

        private float defaultScreenEdgeBuffer = -1;
        private float defaultMinSize = -1;

        public float GetDefaultScreenEdgeBuffer()
        {
            return defaultScreenEdgeBuffer;
        }

        public void ResetScreenEdgeBuffer()
        {
            screenEdgeBuffer = defaultScreenEdgeBuffer;
        }

        public void SetScreenEdgeBuffer(float newSize)
        {
            screenEdgeBuffer = newSize;
        }

        public float GetDefaultMinSize()
        {
            return defaultMinSize;
        }

        public void SetDefaultMinSize(float newSize)
        {
            minSize = newSize;
        }

        public void ResetDefaultMinSize()
        {
            minSize = defaultMinSize;
        }

        private void Awake()
        {
            Singletron = this;
            selfCamera = GetComponent<Camera>();

            if (defaultMinSize != -1 || defaultScreenEdgeBuffer != -1)
            {
                defaultScreenEdgeBuffer = screenEdgeBuffer;
                defaultMinSize = minSize;
            }
        }

        private void Update()
        {
            /*if(SceneManager.GetActiveScene().name == "Main Menu" && Input.GetKeyDown(KeyCode.Slash)) {
                StartShake(1.5f, 0.5f);
            }*/
        }

        private void FixedUpdate()
        {
            if (GetTargetsCount() > 0)
            {
                lastPosition = transform.position;
                selfCamera.orthographicSize = Mathf.SmoothDamp(selfCamera.orthographicSize, GetRequiredSize(), ref zoomSpeed, dampTime);
                transform.position = Vector3.SmoothDamp(transform.position, GetAveragePosition(), ref moveSpeed, dampTime) + GetShakePosition();
            }
            else
            {
                transform.position = lastPosition + GetShakePosition();
            }
        }

        public static void AddNonPlayerTarget(Transform target)
        {
            if (Singletron == null) return;

            if (Singletron.nonPlayerTargets.Contains(target)) return; //no duplicates
            Singletron.nonPlayerTargets.Add(target);
        }

        public static void RemoveNonPlayerTarget(Transform target)
        {
            Singletron.nonPlayerTargets.Remove(target);
        }

        public static int GetTargetsCount()
        {
            return Singletron.nonPlayerTargets.Count;
        }

        public static void ForcePosition()
        {
            if (Singletron == null) return;

            var averagePosition = Singletron.GetAveragePosition();

            if (float.IsNaN(averagePosition.x)) return;

            Singletron.transform.position = averagePosition;
            Singletron.selfCamera.orthographicSize = Singletron.GetRequiredSize();
        }

        public static void StartShake(float magnitude, float duration)
        {
            Singletron.shakeMagnitude = magnitude;
            Singletron.shakeDuration = duration;
            Singletron.lastShake = Time.time;

            print(string.Format("mag = {0} - dur = {1} - time = {2}", magnitude, duration, Time.time));
        }

        private Vector3 GetShakePosition()
        {
            if (lastShake == 0) return Vector3.zero;

            return Random.insideUnitCircle * Mathf.Lerp(shakeMagnitude, 0, (Time.time - lastShake) / shakeDuration);
        }

        private Vector3 GetAveragePosition()
        {
            var targetsCount = GetTargetsCount();
            if (targetsCount == 0) return new Vector3(0, 0, -30);

            float x = 0;
            float y = 0;

            foreach (Transform target in nonPlayerTargets)
            {
                x += target.position.x;
                y += target.position.y;
            }

            return new Vector3(x / targetsCount, y / targetsCount, -30);
        }

        private Vector3 GetVelocityOffset(Rigidbody2D rigidbody, float modifier)
        {
            return new Vector3(rigidbody.velocity.x * modifier, rigidbody.velocity.y * modifier, 0);
        }

        private float GetRequiredSize()
        {
            Vector3 desiredLocalPos = transform.InverseTransformPoint(GetAveragePosition());

            float size = 0;

            foreach (Transform target in nonPlayerTargets)
            {
                Vector3 targetLocalPos = transform.InverseTransformPoint(target.position);
                Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;
                size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));
                size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / selfCamera.aspect);
            }

            size += screenEdgeBuffer;
            size = Mathf.Max(size, minSize);

            return size;
        }
    }
}
