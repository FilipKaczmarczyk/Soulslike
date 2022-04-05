using System;
using UnityEngine;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {
        public GameObject target;
        public Transform camera;
        public Transform cameraPivot;

        private Vector3 _cameraTransformPosition;
        public LayerMask ignoreLayers;

        public static CameraController cc;

        [SerializeField] private float lookSpeed = 0.1f;
        [SerializeField] private float followSpeed = 0.1f;
        [SerializeField] private float pivotSpeed;
        [SerializeField] private float minimumPivot = -35;
        [SerializeField] private float maximumPivot = 35;

        private float defaultPositon;
        private float lookAngle;
        private float pivotAngle;

        private void Awake()
        {
            cc = this;
            defaultPositon = camera.transform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        }

        public void FollowTarget(float delta)
        {
            var targetPosition = Vector3.Lerp(transform.position, target.transform.position, delta / followSpeed);
            transform.position = targetPosition;
        }

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            lookAngle += (mouseXInput * lookSpeed) / delta;
            
            pivotAngle -= (mouseYInput * pivotSpeed) / delta;
            pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);
            
            var rotation = Vector3.zero;
            rotation.y = lookAngle;
            var targetRotation = Quaternion.Euler(rotation);
            transform.rotation = targetRotation;
            
            rotation = Vector3.zero;
            rotation.x = pivotAngle;
            
            targetRotation = Quaternion.Euler(rotation);
            cameraPivot.localRotation = targetRotation;
        }
    }
}
