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
            hit.collider.GetComponent<HealthSystem>()?.TakeDamage(10);
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