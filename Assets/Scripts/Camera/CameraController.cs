using UnityEngine;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform defaultCamera;
        [SerializeField] private Transform cameraPivot;
        [SerializeField] private GameObject cameraTarget;
        [SerializeField] private LayerMask ignoreLayers;
        
        [SerializeField] private float lookSpeed = 0.1f;
        [SerializeField] private float followSpeed = 0.1f;
        [SerializeField] private float pivotSpeed;
        [SerializeField] private float minimumPivot = -35;
        [SerializeField] private float maximumPivot = 35;
        
        [SerializeField] private float cameraSphereRadius = 0.2f;
        [SerializeField] private float collisionOffset = 0.2f;
        [SerializeField] private float minimumCollisionOffset = 0.2f;
        
        private Vector3 _cameraTransformPosition;
        private Vector3 _cameraFollowVelocity;

        private float _targetPosition;
        private float _defaultPosition;
        private float _lookAngle;
        private float _pivotAngle;

        private void Awake()
        {
            _defaultPosition = defaultCamera.transform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        }

        public void FollowTarget(float delta)
        {
            var targetPosition = Vector3.SmoothDamp(transform.position, cameraTarget.transform.position,
                ref _cameraFollowVelocity, delta / followSpeed);
            
            transform.position = targetPosition;
            
            HandleCameraCollision(delta);
        }

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            _lookAngle += (mouseXInput * lookSpeed) / delta;
            
            _pivotAngle -= (mouseYInput * pivotSpeed) / delta;
            _pivotAngle = Mathf.Clamp(_pivotAngle, minimumPivot, maximumPivot);
            
            var rotation = Vector3.zero;
            rotation.y = _lookAngle;
            var targetRotation = Quaternion.Euler(rotation);
            transform.rotation = targetRotation;
            
            rotation = Vector3.zero;
            rotation.x = _pivotAngle;
            
            targetRotation = Quaternion.Euler(rotation);
            cameraPivot.localRotation = targetRotation;
        }
        private void HandleCameraCollision(float delta)
        {
            _targetPosition = _defaultPosition;
            
            var direction = _cameraTransformPosition - cameraPivot.position;
            direction.Normalize();
            
            if (Physics.SphereCast(cameraPivot.transform.position, cameraSphereRadius, direction, out var hit, 
                Mathf.Abs(_targetPosition), ignoreLayers))
            {
                var distance = Vector3.Distance(cameraPivot.transform.position, hit.point);
                _targetPosition = -(distance - collisionOffset);
            }

            if (Mathf.Abs(_targetPosition) < minimumCollisionOffset)
            {
                _targetPosition = -minimumCollisionOffset;
            }

            _cameraTransformPosition.z = Mathf.Lerp(_cameraTransformPosition.z, _targetPosition, delta / 0.2f);
            defaultCamera.localPosition = _cameraTransformPosition;
        }
        
    }
    
}
