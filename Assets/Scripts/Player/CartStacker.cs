using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cart))]
public class CartStacker : MonoBehaviour
{
	private List<GameObject> carts = new List<GameObject>();
	private int CartsCount { get { return carts.Count; } }
	private BoxCollider cartCollider;
	private float colliderZSize;

	private void Awake()
	{
		cartCollider = this.gameObject.GetComponent<BoxCollider>();
		colliderZSize = cartCollider.size.z - 1;
	}

	public void StackCart(GameObject cart, float duration)
	{
		carts.TrimExcess();
		carts.Add(cart);
		cart.transform.parent = this.gameObject.transform;
		DOTween.Sequence().
			Insert(0, cart.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutElastic)).
			Insert(0, cart.transform.DOLocalMove(Vector3.forward * colliderZSize * CartsCount, 0.5f)).
			Play();
		cartCollider.size += Vector3.forward * colliderZSize;
		cartCollider.center += Vector3.forward * colliderZSize / 2;
		StartCoroutine(RemoveCart(cart, duration));
	}
	private IEnumerator RemoveCart(GameObject cart, float duration)
	{
		yield return new WaitForSeconds(duration);
		int index = carts.IndexOf(cart);
		cart.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce).
			OnComplete(() => Destroy(cart));
		if (index < carts.Count - 1)
		{
			for (int i = index + 1; i < carts.Count; i++)
			{
				carts[i].transform.DOLocalMoveZ(carts[i].transform.localPosition.z - colliderZSize, 0.5f);
			}
		}
		cartCollider.size -= Vector3.forward * colliderZSize;
		cartCollider.center -= Vector3.forward * colliderZSize / 2;
		carts.Remove(cart);
	}
	public void RemoveAllCarts()
	{
		StopAllCoroutines();
		if (carts.Count > 0)
		{
			cartCollider.size -= Vector3.forward * colliderZSize * CartsCount;
			cartCollider.center -= Vector3.forward * colliderZSize / 2 * CartsCount;
			foreach (GameObject cart in carts)
			{
				cart.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce).
					OnComplete(() => Destroy(cart));
			}
		}
		carts.Clear();
	}
}
