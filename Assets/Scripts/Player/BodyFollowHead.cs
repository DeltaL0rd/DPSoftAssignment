using Unity.Netcode;
using UnityEngine;

public class BodyFollowHead : NetworkBehaviour
{
    public Transform head;
    public float yOffset = 0f;

    void Update()
    {
        if(!IsOwner)  return;
        Vector3 pos = head.position;
        pos.y = transform.position.y + yOffset;
        transform.position = pos;
    }
}
