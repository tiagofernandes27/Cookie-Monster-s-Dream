using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{

    public enum RoomDifficulty
    {
        Easy,
        Hard
    }

    public static RoomManager Instance;

    [SerializeField] private Animator roomTransitionAnimator;

    // actual room instance
    private GameObject currentRoom;
    //private PlayerTest player;
    [SerializeField] private List<GameObject> easyRoomsLayouts;
    [SerializeField] private List<GameObject> hardRoomsLayouts;
    private int riskMeter;

    [SerializeField] private Image riskFillImage; 

    private void Awake()
    {
        Instance = this;
        riskMeter = 0;
    }

    private void Start()
    {
        // first room is easy
        SpawnRoom(RoomDifficulty.Easy, true);
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

        SpawnRoom(difficulty, false);
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

    private void SpawnRoom(RoomDifficulty difficulty, bool firstRoom) {

        if (currentRoom != null) Destroy(currentRoom);

        // calculate probability of easy room going to hard according to risk meter (first room is easy but does not increase risk)

        if (difficulty == RoomDifficulty.Easy && !firstRoom)
        {
            if (riskMeter == 100) {
                difficulty = RoomDifficulty.Hard;
                riskMeter = 0;
            } 
            else {
                int roll = Random.Range(0, 101); // 0 to 100
                if (roll < riskMeter)
                {
                    Debug.Log("Risk triggered! Easy room turned into HARD!");
                    difficulty = RoomDifficulty.Hard;
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
        Transform[] spawnPoints = new Transform[3];
        spawnPoints[0] = currentRoom.transform.Find("EnemySpawnPoint");
        spawnPoints[1] = currentRoom.transform.Find("EnemySpawnPoint2");
        spawnPoints[2] = currentRoom.transform.Find("EnemySpawnPoint3");

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

        PlayerTest.Instance.transform.position = playerPos.position;
        if (difficulty == RoomDifficulty.Easy) 
            EnemyManager.Instance.StartLevel(1, 10, 5, spawnPoints);    // change values according to difficulty
        else 
            EnemyManager.Instance.StartLevel(1, 10, 5, spawnPoints);

        Debug.Log($"Risk Meter: {riskMeter}%");
    }

    private void UpdateRiskMeterUI()
    {
        // Clamp risk between 0–100
        int riskValue = Mathf.Clamp(riskMeter, 0, 100);

        // Convert percentage into fill amount (0–1)
        riskFillImage.fillAmount = riskValue / 100f;
    }
}
