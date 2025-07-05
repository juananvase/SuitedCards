using UnityEngine;
using UnityEngine.Events;

public class Node : MonoBehaviour
{
    [SerializeField] private Node[] connectedNodes;
    [SerializeField] private bool isActive = false;
    private bool isVisited = false;
    private bool isSkipped = false;

    public UnityEvent onNodeActivated; // Event to trigger when the node is activated
    public UnityEvent onNodeDeactivated; // Event to trigger when the node is deactivated

    private void Start()
    {
        if(!isActive)
        DeactivateNode(); // Ensure the node is deactivated at the start
    }
    public void ActivateNode()
    {
        isActive = true;
        onNodeActivated.Invoke(); // Invoke the event when the node is activated
    }
    public void DeactivateNode()
    {
        isActive = false;
        onNodeDeactivated.Invoke();
    }
    public void TriggerNode()
    {
        isVisited = true;
        NodeManager.Instance.UpdateNodes(); // Notify the NodeManager to update all nodes
        ActivateConnectedNodes();
        DeactivateNode();
    }
    private void ActivateConnectedNodes()
    {
        foreach (Node node in connectedNodes)
        {
            node.ActivateNode();
        }
    }
    public void UpdateNode()
    {
        if(!isSkipped) //Checks only if the node has not been skipped
        {
            if(isActive && !isVisited)
            {
                DeactivateNode();
                isSkipped = true;
            }
        }
    }
}
