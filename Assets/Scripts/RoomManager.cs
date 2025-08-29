using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Room;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    // prefab used to generate rooms
    [SerializeField] private GameObject roomPrefab;

    [SerializeField] private Animator roomTransitionAnimator;

    // actual room instance
    private GameObject currentRoom;
    //private PlayerTest player;
    public List<GameObject> easyRoomsLayouts;
    public List<GameObject> hardRoomsLayouts;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // first room is easy
        spawnRoom(RoomDifficulty.Easy);
    }

    public void TransitionToRoom(RoomDifficulty difficulty)
    {
        //Debug.Log("GENERATING NEW ROOM");
        StartCoroutine(DoTransition(difficulty));
    }

    private IEnumerator DoTransition(RoomDifficulty difficulty)
    {
        roomTransitionAnimator.SetTrigger("End");
        yield return new WaitForSeconds(1);

        spawnRoom(difficulty);
        roomTransitionAnimator.SetTrigger("Start");
    }


    private GameObject GetRandomRoomGrid(RoomDifficulty difficulty)
    {
        switch (difficulty)
        {
            case RoomDifficulty.Easy:
                return easyRoomsLayouts[Random.Range(0, easyRoomsLayouts.Count)];
            case RoomDifficulty.Hard:
                return hardRoomsLayouts[Random.Range(0, hardRoomsLayouts.Count)];
            default:
                return null;
        }
    }

    private void spawnRoom(RoomDifficulty difficulty) {
        if (currentRoom != null) Destroy(currentRoom);
        currentRoom = Instantiate(roomPrefab);
        // Get Room component and assign grid
        Room room = currentRoom.GetComponent<Room>();
        GameObject gridPrefab = GetRandomRoomGrid(difficulty);
        room.SetRoomGrid(gridPrefab);
        // Move player
        Vector3 playerPosition = room.GetPlayerSpawnPosition().position;
        PlayerTest.Instance.transform.position = playerPosition;   
    }
}
