using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Prefab;
    public int Characters;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnCharacters());
    }

    IEnumerator SpawnCharacters()
    {
        for (int i = 0; i < Characters; i++)
        {
            Vector3 position = new Vector3(Random.Range(20, 30), 0, Random.Range(0, 10));
            Instantiate(Prefab, position, Prefab.transform.rotation);
            yield return new WaitForSeconds(1.5f); // Vänta 1 sekund mellan varje spawn
        }
    }
}

