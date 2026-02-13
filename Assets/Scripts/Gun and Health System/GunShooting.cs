using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunShooting : NetworkBehaviour
{
    public Camera playerCamera;
    public float range = 100f;
    public LayerMask hitMask;
    private bool hasShot = false;

    public TrailRenderer trail;
    public float trailSpeed = 200f;
    public Transform trailStartPosition;
    Coroutine trailRoutine;
    
    public InputActionProperty fireTriggerButton;
   
    void Update()
    {
        if(!IsOwner) return;
        if (fireTriggerButton.action.triggered)
        {
            Shoot();
        }

        if (!fireTriggerButton.action.triggered)
        {
            ResetShot();
        }
    }
    
    private void Shoot()
    { if (hasShot) return;
        hasShot = true;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        Vector3 endPoint;

        if (Physics.Raycast(ray, out hit, range, hitMask))
        {
            //hit.collider.GetComponent<HealthSystem>()?.TakeDamage(10);
            ulong targetClientId = hit.collider.transform.parent.transform.GetComponent<NetworkObject>().OwnerClientId;
            HandleGiveDamage(targetClientId);
            endPoint = hit.point;
        }
        else
        {
            endPoint = ray.origin + ray.direction * range;
        }
        
        if (trailRoutine != null)
            StopCoroutine(trailRoutine);

        trailRoutine = StartCoroutine(MoveTrail(endPoint));
    }

    private void HandleGiveDamage(ulong  targetClientId)
    {
        if (IsServer)
        {
            OnPlayerGiveDamageClientRpc(targetClientId,OwnerClientId,new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { targetClientId }
                }
            });
        }
        else
        {
            OnPlayerGiveDamageServerRpc(targetClientId);
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void OnPlayerGiveDamageServerRpc(ulong targetClientId, ServerRpcParams serverRpcParams = default)
    {
        var sender = serverRpcParams.Receive.SenderClientId;
        OnPlayerGiveDamageClientRpc(targetClientId, sender, new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { targetClientId }
            }
        });
    }

    [ClientRpc]
    private void OnPlayerGiveDamageClientRpc(ulong targetClientId, ulong sourceClientId,
        ClientRpcParams clientRpcParams = default)
    {
        Debug.Log($"{sourceClientId} shoot -10dmg {targetClientId}");

        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(targetClientId, out var client))
        {
            NetworkObject playerObject = client.PlayerObject;

            if (playerObject != null)
            {
                GameObject player = playerObject.gameObject;
                Debug.Log("Found Player: " + player.name);
                //It hurts me on every shoot 
                // Very Bad Approach :-( sorry no time 
                // Good approach to store the players data in a Networks sync Dictonary to access cliet Id with there object? I have done this in my projects earlier
                player.GetComponent<PlayerGameplayManager>().bodyObject.GetComponent<HealthSystem>().TakeDamage(10);
            }
        }
    }

    IEnumerator MoveTrail(Vector3 end)
    {
        trail.Clear();
        trail.transform.position = trailStartPosition.position;
        trail.emitting = true;

        float distance = Vector3.Distance(trailStartPosition.position, end);
        float time = distance / trailSpeed;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / time;
            trail.transform.position = Vector3.Lerp(trailStartPosition.position, end, t);
            yield return null;
        }

        trail.transform.position = end;
        
        yield return new WaitForSeconds(trail.time);

        trail.emitting = false;  
    }

    private void ResetShot()
    {
        hasShot = false;
    }
}