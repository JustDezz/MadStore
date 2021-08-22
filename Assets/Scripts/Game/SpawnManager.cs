using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager instance;
    public static SpawnManager Instance { get { return instance; } }
    private Transform objectHolder;
    private List<GameObject> activeObjects = new List<GameObject>();
    private List<GameObject> toDelete = new List<GameObject>();

    [Space(10)] [SerializeField] private List<GameObject> initiatingObjects;

    [Header("Security related")]
    [Space(10)] [Min(0)] [SerializeField] private float securityMinTime;
    [Min(0)] [SerializeField] private float securityMaxTime;
    [SerializeField] private GameObject securityPrefab;

    [Header("Customer related")]
    [Space(10)] [Min(0)] [SerializeField] private float customerMinTime;
    [Min(0)] [SerializeField] private float customerMaxTime;
    [SerializeField] private List<GameObject> customerPrefabs;

    [Header("Bonus related")]
    [Space(10)] [Min(0)] [SerializeField] private float bonusMinTime;
    [Min(0)] [SerializeField] private float bonusMaxTime;
    [SerializeField] private List<GameObject> bonusPrefabs;

    [Space(20)] [SerializeField] private PlayerRunning runner;

    private GameObject lastShelving;
    private GameObject lastFloor;
    private GameObject lastSecurity;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        objectHolder = new GameObject("ObjectHolder").transform;
        Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 1, 0)), out RaycastHit toSpawnHit);
        foreach (GameObject gameObject in initiatingObjects)
        {
            gameObject.transform.parent = objectHolder;
            activeObjects.Add(gameObject);
            Vector3 currentPosition = gameObject.transform.position;
            switch (gameObject.tag)
            {
                case "Shelving":
                    {
                        var rightShelving = ShelvingsPool.Instance.Get();
                        rightShelving.parent = objectHolder;
                        rightShelving.position = currentPosition - Vector3.right * currentPosition.x * 2;
                        rightShelving.rotation = Quaternion.Euler(0, 180, 0);
                        activeObjects.Add(rightShelving.gameObject);
                        do
                        {
                            currentPosition += Vector3.forward * gameObject.transform.localScale.z * 4;

                            lastShelving = ShelvingsPool.Instance.Get().gameObject;
                            lastShelving.transform.parent = objectHolder;
                            lastShelving.transform.position = currentPosition;
                            lastShelving.transform.rotation = Quaternion.identity;
                            activeObjects.Add(lastShelving.gameObject);

                            rightShelving = ShelvingsPool.Instance.Get();
                            rightShelving.parent = objectHolder;
                            rightShelving.position = currentPosition - Vector3.right * currentPosition.x * 2;
                            rightShelving.rotation = Quaternion.Euler(0, 180, 0);

                            activeObjects.Add(rightShelving.gameObject);
                        } while (lastShelving.gameObject.transform.position.z < toSpawnHit.point.z);
                        break;
                    }
                case "Floor":
                    {
                        do
                        {
                            currentPosition += Vector3.forward * gameObject.transform.localScale.z;
                            lastFloor = Instantiate(gameObject, currentPosition, gameObject.transform.rotation, objectHolder);
                            activeObjects.Add(lastFloor.gameObject);
                        } while (lastFloor.gameObject.transform.position.z < toSpawnHit.point.z);
                        break;
                    }
            }
        }
        bool isOnLeft = Random.Range(0, 2) == 0;
        lastSecurity = Instantiate(securityPrefab, new Vector3(isOnLeft ? -2.1f : 2.1f, 0, toSpawnHit.point.z),
            Quaternion.Euler(0, isOnLeft ? 0 : 180, 0), objectHolder);
        activeObjects.Add(lastSecurity);
        initiatingObjects.Clear();
        StartCoroutine(SpawnSecurity());
        StartCoroutine(SpawnCustomer());
        StartCoroutine(SpawnBonus());
    }
    private void Update()
    {
        Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 1, 0)), out RaycastHit toSpawnHit);
        Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, -1.5f, 0)), out RaycastHit toDeleteHit);
        foreach (GameObject gameObject in activeObjects)
        {
            if (gameObject.transform.position.z + gameObject.transform.lossyScale.z < toDeleteHit.point.z)
            {
                toDelete.Add(gameObject);
            }
        }
        while (lastShelving.gameObject.transform.position.z <= toSpawnHit.point.z)
        {
            Vector3 maxPosition = lastShelving.gameObject.transform.position +
                Vector3.forward * lastShelving.gameObject.transform.localScale.z * 4;

            lastShelving = ShelvingsPool.Instance.Get().gameObject;
            lastShelving.transform.parent = objectHolder;
            lastShelving.transform.position = maxPosition;
            activeObjects.Add(lastShelving.gameObject);

            var rightShelving = ShelvingsPool.Instance.Get();
            rightShelving.parent = objectHolder;
            rightShelving.position = maxPosition - Vector3.right * maxPosition.x * 2;
            rightShelving.rotation = Quaternion.Euler(0, 180, 0);
            activeObjects.Add(rightShelving.gameObject);
        }
        while (lastFloor.gameObject.transform.position.z <= toSpawnHit.point.z)
        {
            Vector3 maxPosition = lastFloor.gameObject.transform.position + Vector3.forward * lastFloor.gameObject.transform.localScale.z;
            lastFloor = Instantiate(lastFloor.gameObject, maxPosition, lastFloor.gameObject.transform.rotation, objectHolder);
            activeObjects.Add(lastFloor.gameObject);
        }
        foreach (GameObject gameObject in toDelete)
        {
            activeObjects.Remove(gameObject);
            if (gameObject.CompareTag("Shelving"))
            {
                ShelvingsPool.Instance.RetunInPool(gameObject.transform);
                continue;
            }
            Destroy(gameObject.gameObject);
        }
        toDelete.Clear();
    }
    private IEnumerator SpawnSecurity()
    {
        while (true)
        {
            yield return new WaitForSeconds(
                Random.Range(securityMinTime, Mathf.Lerp(securityMinTime, securityMaxTime, runner.PlayerMinSpeed / runner.Speed)));
            Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 1, 0)), out RaycastHit hit);
            if (lastSecurity == null || (hit.point - lastSecurity.transform.position).sqrMagnitude > 100)
            {
                bool isOnLeft = Random.Range(0, 2) == 0;
                lastSecurity = Instantiate(securityPrefab, new Vector3(isOnLeft ? -2.1f : 2.1f, 0, hit.point.z),
                    Quaternion.Euler(0, isOnLeft ? 0 : 180, 0), objectHolder);
                activeObjects.Add(lastSecurity);
            }
        }
    }
    private IEnumerator SpawnCustomer()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(customerMinTime, customerMaxTime));
            Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 1, 0)), out RaycastHit hit);
            activeObjects.Add(Instantiate(customerPrefabs[Random.Range(0, customerPrefabs.Count)],
                new Vector3(Random.Range(0, 2) == 0 ? -1.5f : 1.5f, 0, hit.point.z),
                Quaternion.identity, objectHolder));
        }
    }
    private IEnumerator SpawnBonus()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(bonusMinTime, bonusMaxTime));
            Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 1, 0)), out RaycastHit hit);
            var bonus = Instantiate(bonusPrefabs[Random.Range(0, bonusPrefabs.Count)],
                new Vector3(Random.Range(0, 2) == 0 ? -1.5f : 1.5f, 1, hit.point.z),
                Quaternion.identity, objectHolder);
            activeObjects.Add(bonus);
            if ((bonus.transform.position - lastSecurity.transform.position).sqrMagnitude < 4)
            {
                bonus.transform.position += Vector3.forward * 6;
            }
        }
    }
    public void RemoveObject(GameObject gameObject)
    {
        if (activeObjects.Contains(gameObject))
        {
            activeObjects.Remove(gameObject);
        }
    }
}