using System.Collections.Generic;
using UnityEngine;

public class Shelving : MonoBehaviour
{
    private bool isFull = false;
    [SerializeField] private List<GameObject> productsPlaces = new List<GameObject>(9);
    private List<Product> products = new List<Product>();
    private void OnEnable()
    {
        foreach (Product product in products)
        {
            ProductsPool.Instance.RetunInPool(product);
        }
        products.Clear();
        isFull = Random.Range(0, 2) == 0;
        if (isFull)
        {
            int productsQuantity = Random.Range(1, productsPlaces.Count);
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
    public Product onIcome(GameObject cart)
    {
        if (isFull)
        {
            Product product = products[Random.Range(0, products.Count)];
            product.MoveInCart(cart);
            products.Remove(product);
            isFull = products.Count > 0;
            return product;
        }
        return null;
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