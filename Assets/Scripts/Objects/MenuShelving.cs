using System.Collections.Generic;
using UnityEngine;

public class MenuShelving : MonoBehaviour
{
    private bool isFull = false;
    [SerializeField] private List<GameObject> productsPlaces = new List<GameObject>(9);
    private List<Product> products = new List<Product>();
    private void Start()
    {
        isFull = Random.Range(0, 5) != 0;
        if (isFull)
        {
            int productsQuantity = Random.Range(3, productsPlaces.Count);
            products = new List<Product>(productsQuantity);
            RandomizeList(ref productsPlaces);
            for (int i = 0; i < productsQuantity; i++)
            {
                products.Insert(i, ProductsPool.Instance.Get());
                products[i].transform.parent = this.gameObject.transform;
                products[i].transform.localScale = Vector3.one;
                products[i].transform.position = productsPlaces[i].transform.position;
            }
        }
    }
    private void RandomizeList(ref List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var place = list[i];
            list.RemoveAt(i);
            list.Insert(Random.Range(0, list.Count), place);
        }
    }
}
