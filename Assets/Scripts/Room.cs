using UnityEngine;

public class Room : MonoBehaviour
{

    private GameObject currentGrid;
    [SerializeField] private Transform roomGridAnchor;
    [SerializeField] private Transform playerSpawnPoint;
    public enum RoomDifficulty
    {
        Easy,
        Hard
    }

    public void SetRoomGrid(GameObject newGridPrefab)
    {
        if (currentGrid != null)
            Destroy(currentGrid);

        // Spawn grid at center of room (indicated by roomGridAnchor)
        currentGrid = Instantiate(newGridPrefab, roomGridAnchor.position, roomGridAnchor.rotation, transform);
    }

    public Transform GetPlayerSpawnPosition() { 
        return playerSpawnPoint; 
    }
}
