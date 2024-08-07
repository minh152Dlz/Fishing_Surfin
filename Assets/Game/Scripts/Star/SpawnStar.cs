using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStar : MonoBehaviour
{
    private static SpawnStar instance;
    public static SpawnStar Instance {  get { return instance; } }

    [SerializeField] private GameObject starPrefab;

    [SerializeField] private float minX = -10f;
    [SerializeField] private float maxX = 10f;
    [SerializeField] private float minY = -5f;
    [SerializeField] private float maxY = 5f;

    [SerializeField] private float waitDestroy = 5f;
    private Coroutine spawnCoroutine;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        StartCoroutineRoutine();
    }
    public void StartCoroutineRoutine()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        spawnCoroutine = StartCoroutine(SpawnObjectRoutine());
    }
    IEnumerator SpawnObjectRoutine()
    {
        while (true)
        {
            GameObject spawnedObject = SpawnObject();

            yield return new WaitForSeconds(waitDestroy);

            //Destroy(spawnedObject);
            spawnedObject.SetActive(false);

            yield return new WaitForSeconds(1f);
        }
    }

    public GameObject SpawnObject()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        Vector2 randomPosition = new Vector2(randomX, randomY);

        starPrefab.SetActive(true);
        starPrefab.transform.position = randomPosition;
        return starPrefab;
        //return Instantiate(starPrefab, randomPosition, Quaternion.identity);
    }
}
