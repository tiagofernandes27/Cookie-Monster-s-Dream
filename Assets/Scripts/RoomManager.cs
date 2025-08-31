using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{

    public enum RoomType
    {
        Easy,
        Hard,
        Start
    }

    public static RoomManager Instance;

    [SerializeField] private Animator roomTransitionAnimator;

    // actual room instance
    private GameObject currentRoom;
    //private PlayerTest player;
    [SerializeField] private List<GameObject> easyRoomPrefabs;
    [SerializeField] private List<GameObject> hardRoomPrefabs;
    [SerializeField] private GameObject startRoomPrefab;
    [SerializeField] private GameObject bossRoomPrefab;
    private int riskMeter;

    [SerializeField] private Image riskFillImage;

    private int roomsCounter;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        TransitionToRoom(RoomType.Start);
    }

    public void TransitionToRoom(RoomType difficulty)
    {
        //Debug.Log("GENERATING NEW ROOM");
        StartCoroutine(DoTransitionRoom(difficulty));
    }

    private IEnumerator DoTransitionRoom(RoomType roomType)
    {
        roomTransitionAnimator.SetTrigger("End");
        yield return new WaitForSeconds(1);

        if (roomType == RoomType.Start)
            SpawnStartRoom();
        else if (roomsCounter >= 5)
            SpawnBossRoom();    
        else
            SpawnRoom(roomType);

        roomTransitionAnimator.SetTrigger("Start");
    }


    private GameObject GetRandomRoomGrid(RoomType difficulty)
    {
        switch (difficulty)
        {
            case RoomType.Easy:
                return easyRoomPrefabs[Random.Range(0, easyRoomPrefabs.Count)];
            case RoomType.Hard:
                return hardRoomPrefabs[Random.Range(0, hardRoomPrefabs.Count)];
            default:
                return null;
        }
    }

    private void SpawnBossRoom()
    {
        if (currentRoom != null) Destroy(currentRoom);

        currentRoom = Instantiate(bossRoomPrefab, Vector3.zero, Quaternion.identity);
        Transform playerPos = currentRoom.transform.Find("PlayerSpawnPoint");
        Player.Instance.transform.position = playerPos.position;
        Transform bossSpawn = currentRoom.transform.Find("BossSpawnPoint");
        EnemyManager.Instance.SpawnBoss(bossSpawn);

        if (playerPos == null)
        {
            Debug.LogError("Player position NULL!");
        }
    }

    public void SpawnStartRoom()
    {
        // reset risk meter and rooms completed counter (game resets)
        riskMeter = 0;
        roomsCounter = 0;
        UpdateRiskMeterUI();

        if (currentRoom != null) Destroy(currentRoom);

        currentRoom = Instantiate(startRoomPrefab, Vector3.zero, Quaternion.identity);
        Transform playerPos = currentRoom.transform.Find("PlayerSpawnPoint");
        Player.Instance.transform.position = playerPos.position;
        OpenRoomDoors();

        if (playerPos == null)
        {
            Debug.LogError("Player position NULL!");
        }
    }

    private void SpawnRoom(RoomType difficulty) {

        if (currentRoom != null) Destroy(currentRoom);

        // calculate probability of easy room going to hard according to risk meter (first room is easy but does not increase risk)

        if (difficulty == RoomType.Easy)
        {
            if (riskMeter == 100) {
                difficulty = RoomType.Hard;
                riskMeter = 0;
            } 
            else {
                int roll = Random.Range(0, 101); // 0 to 100
                if (roll < riskMeter)
                {
                    Debug.Log("Risk triggered! Easy room turned into HARD!");
                    difficulty = RoomType.Hard;
                    riskMeter = 0; // reset risk meter
                    UpdateRiskMeterUI();
                }
                else
                {
                    riskMeter += 25; // increase risk by 25%
                    UpdateRiskMeterUI();
                }
            }        
        }

        currentRoom = Instantiate(GetRandomRoomGrid(difficulty), Vector3.zero, Quaternion.identity);
        Transform playerPos = currentRoom.transform.Find("PlayerSpawnPoint");
        Transform[] spawnPoints = new Transform[2];
        spawnPoints[0] = currentRoom.transform.Find("EnemySpawnPoint1");
        spawnPoints[1] = currentRoom.transform.Find("EnemySpawnPoint2");

        if (playerPos == null)
        {
            Debug.LogError("Player position NULL!");
        }
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Debug.Log("Spawn point "+i+" "+spawnPoints[i]);
            if (spawnPoints[i] == null)
            {
                Debug.LogError($"Enemy Spawn point {i} is null!");
            }
        }

        Player.Instance.transform.position = playerPos.position;
        if (difficulty == RoomType.Easy) 
            EnemyManager.Instance.StartWaves(1, 10, 5, spawnPoints);    // change values according to difficulty
        else 
            EnemyManager.Instance.StartWaves(1, 10, 5, spawnPoints);

        roomsCounter++;
        Debug.Log("Rooms Counter: "+roomsCounter);
        Debug.Log($"Risk Meter: {riskMeter}%");

        if (roomsCounter > 1) { // player passed a room
            Player.Instance.PlayerMoney += 5;
            Debug.Log("Player Money " + Player.Instance.PlayerMoney);
        }
    }

    private void UpdateRiskMeterUI()
    {
        // Clamp risk between 0–100
        int riskValue = Mathf.Clamp(riskMeter, 0, 100);

        // Convert percentage into fill amount (0–1)
        riskFillImage.fillAmount = riskValue / 100f;
    }

    public void OpenRoomDoors()
    {
        if (currentRoom == null)
        {
            //Debug.LogWarning("No current room to open doors in!");
            return;
        }
        else { 
        
        }

        // Try to find EasyDoor and HardDoor under the currentRoom hierarchy
        Transform easyDoor = currentRoom.transform.Find("EasyDoor");
        Transform hardDoor = currentRoom.transform.Find("HardDoor");

        if (easyDoor != null)
        {
            Door easyDoorScript = easyDoor.GetComponent<Door>();
            if (easyDoorScript != null)
                easyDoorScript.OpenDoor();
            else
                Debug.LogWarning("EasyDoor exists but has no Door script!");
        }
        else
        {
            Debug.LogWarning("EasyDoor not found in currentRoom!");
        }

        if (hardDoor != null)
        {
            Door hardDoorScript = hardDoor.GetComponent<Door>();
            if (hardDoorScript != null)
                hardDoorScript.OpenDoor();
            else
                Debug.LogWarning("HardDoor exists but has no Door script!");
        }
        else
        {
            Debug.LogWarning("HardDoor not found in currentRoom!");
        }
    }

}
