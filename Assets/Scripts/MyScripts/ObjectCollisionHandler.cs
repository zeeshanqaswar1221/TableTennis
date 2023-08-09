using UnityEngine;

public class ObjectCollisionHandler : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    private bool isCollided = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle") && !isCollided)
        {
            int randomIndex = Random.Range(0, objectsToSpawn.Length);
            GameObject randomObjectPrefab = objectsToSpawn[randomIndex];

            Vector3 spawnPosition = collision.contacts[0].point; // Get the collision point
            GameObject spawnedObject = Instantiate(randomObjectPrefab, spawnPosition, Quaternion.identity);

            Destroy(spawnedObject, 0.3f);

            isCollided = true; // Set the flag to true to prevent immediate additional spawns
            StartCoroutine(ResetCollisionFlagAfterDelay());
        }
    }

    private System.Collections.IEnumerator ResetCollisionFlagAfterDelay()
    {
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds
        isCollided = false; // Reset the flag after the delay
    }
}
