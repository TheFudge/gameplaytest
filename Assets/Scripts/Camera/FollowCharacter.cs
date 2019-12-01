using UnityEngine;

public class FollowCharacter : MonoBehaviour
{
    [SerializeField]
    private Transform targetToFollow;
    private bool followingTarget = false;

    [SerializeField]
    private bool lerpToObject = true;
    [Range(0f, 20f)]
    [SerializeField]
    private float lerpSpeed = 4f;
    
    [SerializeField]
    private Vector3 cameraOffset = Vector3.zero;

    private float cameraZPosition = -10f;
    
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
        Vector3 pos = Vector3.Lerp(transform.position, targetToFollow.position + cameraOffset, Time.deltaTime * lerpSpeed);
        pos.z = cameraZPosition;
        transform.position = pos;

    }

    void DoHardFollow()
    {
        transform.position = targetToFollow.position + cameraOffset;
    }
}
