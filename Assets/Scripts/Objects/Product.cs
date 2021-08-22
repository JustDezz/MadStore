using DG.Tweening;
using UnityEngine;

public class Product : MonoBehaviour
{
    [SerializeField] private string productName;
    [Min(0)] [SerializeField] private float productPrice;
    public static event System.Action OnAnyProductCollected;
    public string ProductName { get { return productName; } }
    public float ProductPrice { get { return productPrice; } }
    public void MoveInCart(GameObject cart)
    {
        OnAnyProductCollected?.Invoke();

        this.transform.parent = cart.transform;
        DOTween.Sequence().
            Insert(0, this.transform.DORotate(Vector3.up * 179 * 10, 1f).SetEase(Ease.Linear)).
            Insert(0, this.transform.DOLocalMove(
                new Vector3(0 + Random.Range(-0.5f, 0.5f), 2 + Random.Range(-0.5f, 0.5f), 2 + Random.Range(-0.5f, 0.5f)), 0.5f).
                    OnComplete(() => this.transform.DOLocalMove(Vector3.zero, 0.5f))).
            Insert(0, this.transform.DOScale(transform.localScale * Random.Range(1f, 1.5f), 0.5f).
                OnComplete(() => this.transform.DOScale(Vector3.zero, 0.5f))).
            AppendCallback(() => ProductsPool.Instance.RetunInPool(this)).
            Play();
    }
    private void OnDestroy()
    {
        DOTween.Kill(this.gameObject);
    }
}
