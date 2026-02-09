using Unity.Netcode;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class PlayerGameplayManager : NetworkBehaviour
{
    [SerializeField] private GameObject bodyObject;
    [SerializeField] private GameObject[] unwantedObjects;
    [SerializeField] private Camera othersCameraObject;
    //Clean up Components or Gameobjects for other players
    public override void OnNetworkSpawn()
    {
        
        if (!IsOwner)
        {
            foreach (var obj in unwantedObjects)
            {
                Destroy(obj);
            }
            othersCameraObject.enabled = false;
            //gameObject.GetComponent<XRInputModalityManager>().enabled = false;
            //gameObject.GetComponent<InputActionManager>().enabled = false;
            //gameObject.GetComponent<XROrigin>().enabled = false;
            return;
        }
        
        bodyObject.layer = LayerMask.NameToLayer("Player");
    }
}
