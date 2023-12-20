using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject platformPrefab;
    public Transform player;
    public float minY = 0.5f; // Minimum vertical space between platforms
    public float maxY = 1.5f; // Maximum vertical space between platforms
    private float spawnY = 0.0f;
    private Vector3 screenBottomLeft;
    private Vector3 screenTopRight;
    private Vector3 lastPlatformPosition;
    private float platformWidth;

    void Start()
    {
        platformWidth = platformPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        screenBottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        screenTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
        
        // Set the initial spawnY to the player's Y position minus half the player's height to start at the player's feet
        spawnY = player.position.y - 3;

        // Spawn the first platform directly under the player
        SpawnPlatform(player.position.x, spawnY);

        // Update spawnY for the next platform
        spawnY += Random.Range(minY, maxY);

        // Continue placing platforms upward from the initial spawnY
        while (spawnY < screenTopRight.y)
        {
            SpawnPlatform(spawnY);
            spawnY += Random.Range(minY, maxY);
        }
    }

    void Update()
    {
        if (player.position.y > spawnY - (screenTopRight.y - screenBottomLeft.y))
        {
            SpawnPlatform(spawnY);
        }
    }

    void SpawnPlatform(float yPosition)
    {
        float xPosition = Random.Range(screenBottomLeft.x + platformWidth / 2, screenTopRight.x - platformWidth / 2);
        SpawnPlatform(xPosition, yPosition);
    }

    void SpawnPlatform(float xPosition, float yPosition)
    {
        Vector3 spawnPosition = new Vector3(xPosition, yPosition, 0);

        // Instantiate the platform at the spawn position
        Instantiate(platformPrefab, spawnPosition, Quaternion.identity);

        // Remember the last platform position
        lastPlatformPosition = spawnPosition;

        // Increase the spawnY for the next platform
        spawnY += Random.Range(minY, maxY);
    }
}
