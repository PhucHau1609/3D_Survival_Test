using System.Collections.Generic;
using UnityEngine;

public class MapGenerator2 : MonoBehaviour
{
    public Transform player; // Nhân vật
    public GameObject[] chunkPrefabs; // Các prefab chunks
    public int renderDistance = 3; // Số chunk giữ xung quanh nhân vật
    public float chunkSize = 50f; // Kích thước mỗi chunk

    private List<GameObject> activeChunks = new List<GameObject>();
    private Transform lastAnchorPoint;
    private Transform firstAnchorPoint;
    private bool isGenerating = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isGenerating)
        {
            isGenerating = true;
            SpawnInitialChunks();
        }
    }

    void Update()
    {
        if (isGenerating)
        {
            UpdateChunks();
        }
    }

    void SpawnInitialChunks()
    {
        GameObject firstChunk = Instantiate(chunkPrefabs[0], transform.position, Quaternion.Euler(0, 45, 0));
        activeChunks.Add(firstChunk);

        lastAnchorPoint = firstChunk.transform.Find("End Point");
        firstAnchorPoint = firstChunk.transform.Find("Start Point");

        for (int i = 1; i < renderDistance; i++)
        {
            SpawnNextChunk(true);
        }
    }

    void SpawnNextChunk(bool forward)
    {
        GameObject chunkPrefab = chunkPrefabs[Random.Range(0, chunkPrefabs.Length)];
        GameObject newChunk;

        if (forward)
        {
            newChunk = Instantiate(chunkPrefab, lastAnchorPoint.position, Quaternion.Euler(0, 45, 0));
            lastAnchorPoint = newChunk.transform.Find("End Point");
            activeChunks.Add(newChunk);
        }
        else
        {
            newChunk = Instantiate(chunkPrefab, firstAnchorPoint.position, Quaternion.Euler(0, 45, 0));
            firstAnchorPoint = newChunk.transform.Find("Start Point");
            activeChunks.Insert(0, newChunk);
        }
    }

    void UpdateChunks()
    {
        // Xóa chunk quá xa
        if (activeChunks.Count > renderDistance)
        {
            GameObject oldChunk = activeChunks[0];
            activeChunks.RemoveAt(0);

            if (activeChunks.Count > 0)
            {
                firstAnchorPoint = activeChunks[0].transform.Find("Start Point");
            }

            Destroy(oldChunk);
        }

        // Kiểm tra NULL trước khi sử dụng Transform
        if (lastAnchorPoint == null || firstAnchorPoint == null)
        {
            return;
        }

        // Spawn khi đi tiến
        while (Vector3.Distance(player.position, lastAnchorPoint.position) < chunkSize)
        {
            SpawnNextChunk(true);
        }

        // Spawn khi đi lùi (Lặp nhiều lần để tạo cầu liên tục)
        while (Vector3.Distance(player.position, firstAnchorPoint.position) < chunkSize)
        {
            SpawnNextChunk(false);
        }
    }
}
