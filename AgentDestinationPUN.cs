using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

// For silky smooth Photon PUN movement using NavMeshAgent

public class AgentDestinationPUN : MonoBehaviourPun, IPunObservable
{    
    Vector3 agentDestination = new Vector3();
    NavMeshAgent agent;

    private void Awake()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            agentDestination = agent.destination;
            stream.SendNext(agentDestination);
        }
        else
        {
            agentDestination = (Vector3)stream.ReceiveNext();
        }
    }

    private void Update()
    {
        if (!this.photonView.IsMine)
        {
            agent.SetDestination(agentDestination);
        }
    }
}
