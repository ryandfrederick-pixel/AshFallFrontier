using UnityEngine;

namespace AshfallFrontier.Combat
{
    /// <summary>
    /// Minimal third-person orbit camera.
    /// - Mouse to look
    /// - Clamped pitch
    /// - Optional cursor lock
    /// </summary>
    public class ThirdPersonCamera : MonoBehaviour
    {
        [Header("Target")]
        public Transform target;

        [Header("Orbit")]
        public float distance = 4.5f;
        public float height = 1.6f;
        public float sensitivityX = 220f;
        public float sensitivityY = 140f;
        public float pitchMin = -35f;
        public float pitchMax = 70f;

        [Header("Smoothing")]
        public float positionLerp = 18f;
        public float rotationLerp = 22f;

        [Header("Cursor")]
        public bool lockCursor = true;

        private float _yaw;
        private float _pitch;

        void Start()
        {
            if (lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            var e = transform.eulerAngles;
            _yaw = e.y;
            _pitch = e.x;
        }

        void LateUpdate()
        {
            if (!target) return;

            float mx = Input.GetAxisRaw("Mouse X");
            float my = Input.GetAxisRaw("Mouse Y");

            _yaw += mx * sensitivityX * Time.deltaTime;
            _pitch -= my * sensitivityY * Time.deltaTime;
            _pitch = Mathf.Clamp(_pitch, pitchMin, pitchMax);

            var rot = Quaternion.Euler(_pitch, _yaw, 0f);
            var desiredPos = target.position + Vector3.up * height - (rot * Vector3.forward) * distance;

            transform.position = Vector3.Lerp(transform.position, desiredPos, 1f - Mathf.Exp(-positionLerp * Time.deltaTime));
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1f - Mathf.Exp(-rotationLerp * Time.deltaTime));
        }

        public Vector3 FlatForward()
        {
            var f = transform.forward;
            f.y = 0;
            return f.normalized;
        }

        public Vector3 FlatRight()
        {
            var r = transform.right;
            r.y = 0;
            return r.normalized;
        }
    }
}
