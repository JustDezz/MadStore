using DG.Tweening;
using UnityEngine;

public class Security : MonoBehaviour
{
    public void Die()
    {
        this.transform.GetComponentInChildren<Animator>().SetBool("isDead", true);
        SoundManager.Instance.PlayOneShot("SecurityDie", true);
        SpawnManager.Instance.RemoveObject(this.gameObject);
        this.transform.parent = Camera.main.transform;

        Vector3 onCameraPosition = new Vector3(Random.Range(0.2f, 0.8f), Random.Range(0.4f, 0.8f), 1f);
        Vector3 startPosition = Camera.main.transform.InverseTransformPoint(Camera.main.ViewportToWorldPoint(onCameraPosition));
        Vector3 endPosition = startPosition - Vector3.up;

        this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        this.transform.localRotation = Quaternion.identity;
        DOTween.Sequence().
            Insert(0, this.transform.DOScale(0.1f, 1f)).
            Insert(0, this.transform.DOLocalRotate(new Vector3(360 * Random.Range(-2.5f, 2.5f), 90, 0), 1f)).
            Insert(0, this.transform.DOLocalMove(Vector3.Lerp(this.transform.localPosition, startPosition, 0.5f) + Vector3.up * 10, 0.5f)).
            Append(this.transform.DOLocalMove(startPosition, 0.5f)).
            AppendInterval(1f).
            Append(this.transform.DOLocalMove(endPosition, 3f).SetEase(Ease.InCirc)).
            AppendCallback(() => Destroy(this.gameObject)).
            Play();
    }
}
