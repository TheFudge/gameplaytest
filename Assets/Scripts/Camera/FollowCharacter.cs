using UnityEngine;

namespace Game
{
    public class FollowCharacter : MonoBehaviour
    {
        [SerializeField] private Transform targetToFollow;
        
        [SerializeField] private Vector3 cameraOffset = Vector3.zero;
        
        [SerializeField] private bool lerpToObject = true;
        [Range(0f, 20f)] [SerializeField] private float lerpSpeed = 4f;

        
        private float cameraZPosition = -10f;
        private Vector3 cameraPosition;
        private bool followingTarget = false;
        
        void Start()
        {
            followingTarget = targetToFollow != null;
        }

        public void FolloTarget(Transform target)
        {
            targetToFollow = target;
            followingTarget = true;
        }

        public void StopFollowingTarget(Transform target)
        {
            if (targetToFollow == target)
                return;
            targetToFollow = null;
            followingTarget = false;
        }

        public void SetOffset(Vector3 newOffset)
        {
            cameraOffset = newOffset;
        }

        private void LateUpdate()
        {
            if (!followingTarget)
                return;

            if (lerpToObject)
            {
                DoLerpedFollow();
            }
            else
            {
                DoHardFollow();
            }
        }

        void DoLerpedFollow()
        {
            cameraPosition = Vector3.Lerp(transform.position, targetToFollow.position + cameraOffset,
                Time.deltaTime * lerpSpeed);
            cameraPosition.z = cameraZPosition;
            transform.position = cameraPosition;

        }

        void DoHardFollow()
        {
            cameraPosition = targetToFollow.position + cameraOffset;
            cameraPosition.z = cameraZPosition;
            transform.position = cameraPosition;
        }
    }
}