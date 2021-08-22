using UnityEngine;

public class ManualNode : MonoBehaviour
{
    public static System.Action<ManualNode> OnAnyNodeSelected;

    [SerializeField] private string nodeName;
    [TextArea] [SerializeField] private string nodeDescription;

    public string NodeName { get { return nodeName; } }
    public string NodeDescription { get { return nodeDescription; } }

    public void InteracWithNode()
    {
        OnAnyNodeSelected?.Invoke(this);
    }

    private void OnValidate()
    {
        if (nodeName != null)
        {
            this.gameObject.name = nodeName;
        }
        else
        {
            this.gameObject.name = "Node";
        }
    }
}
