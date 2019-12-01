using UnityEngine;

namespace Game
{
    public class FollowCharacter : MonoBehaviour
    {
        [SerializeField] private Transform targetToFollow;
        private bool followingTarget = false;

        [SerializeField] private bool lerpToObject = true;
        [Range(0f, 20f)] [SerializeField] private float lerpSpeed = 4f;

        [SerializeField] private Vector3 cameraOffset = Vector3.zero;

        private float cameraZPosition = -10f;
        private Vector3 cameraPosition;
        
        void Start()
        {
            followingTarget = targetToFollow != null;
        }

        public void Follow(Transform target)
        {
            targetToFollow = target;
            followingTarget = true;
        }

        public void StopFollowing(Transform target)
        {
            if (targetToFollow == target)
                return;
            targetToFollow = null;
            followingTarget = false;
        }

        void LateUpdate()
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