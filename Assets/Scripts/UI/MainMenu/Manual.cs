using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Manual : MonoBehaviour
{
    [SerializeField] private string defaultNodeName;
    [TextArea] [SerializeField] private string defaultNodeDescription;

    [SerializeField] private HorizontalLayoutGroup horizontalLayout;

    private ManualNode selectedNode = null;

    [SerializeField] private Text nodeNameLabel;
    [SerializeField] private Text nodeDescriptionLabel;

    [SerializeField] private Ease easing;

    private void Awake()
    {
        ManualNode.OnAnyNodeSelected += SelectNode;
    }

    private void SelectNode(ManualNode node)
    {
        if (selectedNode != null)
        {
            selectedNode.transform.DOScale(Vector3.one, 0.25f).SetEase(easing);
            DOTween.To(x => horizontalLayout.spacing = x, horizontalLayout.spacing, horizontalLayout.spacing + 0.01f, 0.25f).SetEase(easing);
        }
        if (selectedNode != node)
        {
            selectedNode = node;
            nodeNameLabel.text = node.NodeName;
            nodeDescriptionLabel.text = node.NodeDescription;
            node.transform.DOScale(Vector3.one * 1.5f, 0.25f).SetEase(easing);
            DOTween.To(x => horizontalLayout.spacing = x, horizontalLayout.spacing, horizontalLayout.spacing - 0.01f, 0.25f).SetEase(easing);
        }
        else
        {
            nodeNameLabel.text = defaultNodeName;
            nodeDescriptionLabel.text = defaultNodeDescription;
            selectedNode = null;
        }
    }

    private void OnDisable()
    {
        SelectNode(selectedNode);
        DOTween.Kill(selectedNode, true);
    }
    private void OnValidate()
    {
        nodeNameLabel.text = defaultNodeName;
        nodeDescriptionLabel.text = defaultNodeDescription;
    }
}
