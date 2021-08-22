using DG.Tweening;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] private GameObject customerCart;
    [SerializeField] private GameObject bonusLabel;

    public GameObject Cart { get { return customerCart; } }

    [Min(0)] [SerializeField] private float bonusDuration;
    [Min(0)] [SerializeField] private int bonusScore;
    [Min(0)] [SerializeField] private int bonusIncome;
    [Range(0, 1)] [SerializeField] private float bonusSpeed;

    public float BonusDuration { get { return bonusDuration; } }
    public int BonusScore { get { return bonusScore; } }
    public int BonusIncome { get { return bonusIncome; } }
    public float BonusSpeed { get { return bonusSpeed; } }
    public bool HasCart { get { return Cart != null; } }

    private void FixedUpdate()
    {
        transform.position += Vector3.forward * speed * Time.fixedDeltaTime;
    }
    public void Die()
    {
        if (Cart != null)
        {
            UIManager.Instance.ActivateBonusLabel(bonusLabel.GetComponent<BonusLabel>(), bonusDuration);
        }
        this.transform.GetComponentInChildren<Animator>().SetBool("isDead", true);
        speed = 0;
        SoundManager.Instance.PlaySound("CustomerDie", true);
        SpawnManager.Instance.RemoveObject(this.gameObject);
        this.transform.parent = Camera.main.transform;

        Vector3 onCameraPosition = new Vector3(Random.Range(0.2f, 0.8f), Random.Range(0.4f, 0.8f), 1f);
        Vector3 startPosition = Camera.main.transform.InverseTransformPoint(Camera.main.ViewportToWorldPoint(onCameraPosition));
        Vector3 endPosition = startPosition - Vector3.up;

        this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        this.transform.localRotation = Quaternion.identity;
        DOTween.Sequence().
            Insert(0, this.transform.DOScale(0.1f, 1f)).
            Insert(0, this.transform.DOLocalRotate(new Vector3(0, -180, 360 * Random.Range(-2.5f, 2.5f)), 1f)).
            Insert(0, this.transform.DOLocalMove(Vector3.Lerp(this.transform.localPosition, startPosition, 0.5f) + Vector3.up * 10, 0.5f)).
            Append(this.transform.DOLocalMove(startPosition, 0.5f)).
            AppendInterval(1f).
            Append(this.transform.DOLocalMove(endPosition, 3f).SetEase(Ease.InCirc)).
            AppendCallback(() => Destroy(this.gameObject)).
            Play();
    }
}
