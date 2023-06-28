using Fusion;
using UnityEngine;

// NOTE
// This needs to be added to a prefab gameobject that the server spawns. It will not work if added to a gameobject that is already in the scene.

public class FusionTestMessages : NetworkBehaviour
{
    [SerializeField] bool sendMessage;
    [SerializeField] string message = "Hello World";

    public override void FixedUpdateNetwork()
    {
        if (sendMessage)
        {
            sendMessage = false;
            RPC_Message(message);
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_Message(string message)
    {
        Debug.Log("RPC_Message: " + message);
        RPC_MessageAll(message);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_MessageAll(string message)
    {
        Debug.Log("RPC_MessageAll: " + message);
    }
}
