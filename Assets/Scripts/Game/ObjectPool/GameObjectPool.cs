using System.Collections.Generic;
using UnityEngine;

public abstract class GameObjectPool<T> : MonoBehaviour where T : Component
{
	[SerializeField] private List<GameObject> objectPrefabs = new List<GameObject>();
	[SerializeField] private string poolName;
	private GameObject poolHolder;
	private Queue<T> objects = new Queue<T>();
	public static GameObjectPool<T> instance;
	public static GameObjectPool<T> Instance
	{
		get { return instance; }
		private set
		{
			if (instance == null)
			{
				instance = value;
			}
			else
			{
				throw new System.Exception($"Pool {instance.poolName} already exists");
			}
		}
	}

	private void Awake()
	{
		if (instance != null)
		{
			Destroy(this);
			return;
		}
		instance = this;
		DontDestroyOnLoad(this.gameObject);
		poolHolder = new GameObject(poolName);
		poolHolder.transform.parent = this.transform;
		objectPrefabs.TrimExcess();
		objects = new Queue<T>();
	}
	public T Get()
	{
		if (objects.Count <= 0)
		{
			Add(1);
		}
		var gettingObject = objects.Dequeue();
		gettingObject.gameObject.SetActive(true);
		return gettingObject;
	}
	public void RetunInPool(T returningObject)
	{
		if (poolHolder != null)
		{
			returningObject.transform.SetParent(poolHolder.transform, false);
		}
		returningObject.gameObject.SetActive(false);
		objects.Enqueue(returningObject);
	}
	private void Add(int num)
	{
		for (int i = 0; i < num; i++)
		{
			var objectToPool = Instantiate(objectPrefabs[Random.Range(0, objectPrefabs.Count)], poolHolder.transform);
			objectToPool.SetActive(false);
			objects.Enqueue(objectToPool.GetComponent<T>());
		}
	}
}