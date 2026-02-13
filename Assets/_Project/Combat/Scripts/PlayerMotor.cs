using UnityEngine;

namespace AshfallFrontier.Combat
{
    /// <summary>
    /// Simple CharacterController-based motor for third-person movement.
    /// Uses camera-relative input (WASD) with smooth rotation.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMotor : MonoBehaviour
    {
        [Header("Refs")]
        public ThirdPersonCamera cam;

        [Header("Movement")]
        public float moveSpeed = 5.2f;
        public float sprintSpeed = 7.0f;
        public float rotationSpeed = 14f;
        public float gravity = -22f;

        [Header("Jump (optional)")]
        public bool allowJump = false;
        public float jumpHeight = 1.2f;

        private CharacterController _cc;
        private Vector3 _vel;

        void Awake()
        {
            _cc = GetComponent<CharacterController>();
        }

        void Update()
        {
            if (!cam) return;

            // Input
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            bool sprint = Input.GetKey(KeyCode.LeftShift);

            Vector3 input = new Vector3(h, 0f, v);
            input = Vector3.ClampMagnitude(input, 1f);

            Vector3 moveDir = (cam.FlatForward() * input.z + cam.FlatRight() * input.x);
            float speed = sprint ? sprintSpeed : moveSpeed;

            // Rotate towards move
            if (moveDir.sqrMagnitude > 0.001f)
            {
                var desired = Quaternion.LookRotation(moveDir, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, desired, 1f - Mathf.Exp(-rotationSpeed * Time.deltaTime));
            }

            // Ground check
            if (_cc.isGrounded && _vel.y < 0f)
                _vel.y = -2f; // stick

            if (allowJump && _cc.isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                _vel.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            _vel.y += gravity * Time.deltaTime;

            // Move
            Vector3 motion = moveDir * speed;
            motion += _vel;
            _cc.Move(motion * Time.deltaTime);
        }
    }
}
