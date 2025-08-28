using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static Room;

public class Door : MonoBehaviour
{

    [SerializeField] private RoomDifficulty difficulty;
    private PlayerTest player;
    private BoxCollider2D doorCollider;
    private Vector2 originalColliderSize;

    private void Awake()
    {
        doorCollider = GetComponent<BoxCollider2D>();
        originalColliderSize = doorCollider.size;
    }

    private void Update()
    {
        /* Testing openDoor function (press t key to test) */
        if (Keyboard.current.tKey.IsActuated())
        {
            OpenDoor();
            Debug.Log("DOOR OPEN");
        }
    }

    public void OpenDoor() {
        doorCollider.isTrigger = true;
        doorCollider.size = new Vector2(originalColliderSize.x, originalColliderSize.y * 0.5f);
        doorCollider.offset = new Vector2(doorCollider.offset.x, originalColliderSize.y * 0.25f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.TryGetComponent(out PlayerTest player))
        {
            this.player = player;
            Debug.Log("PLAYER GOES TO "+difficulty+" ROOM");
            RoomManager.Instance.TransitionToRoom(difficulty);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.TryGetComponent(out PlayerTest player))
        {
            Debug.Log("PLAYER LEFT DOOR");
        }
    }
}
