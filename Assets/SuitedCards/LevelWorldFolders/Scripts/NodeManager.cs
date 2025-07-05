using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public static NodeManager Instance;
    private Node[] nodes;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        nodes = GameObject.FindObjectsByType<Node>(FindObjectsSortMode.None);
    }
    public void UpdateNodes()
    {
        foreach (Node node in nodes)
        {
            node.UpdateNode();
        }
    }
}
