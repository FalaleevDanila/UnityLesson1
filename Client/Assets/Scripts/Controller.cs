using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private float _restartDelay = 3f;
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private float _mouseSensetivity = 2f;
    [SerializeField] private PlayerGun _gun;

    private MultiplayerManager _multiplayerManager;
    private bool _hold = false;
    private bool _hideCursor;

    private void Start()
    {
        _hideCursor = true;
        _multiplayerManager = MultiplayerManager.Instance;
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _hideCursor = !_hideCursor;
        }
        if (_hold) return;
        
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float mouseX = 0;
        float mouseY = 0;

        bool isShoot = false;
        if (_hideCursor)
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");

            isShoot = Input.GetMouseButton(0);
        }
        

        bool space = Input.GetKeyDown(KeyCode.Space);

        _player.SetInput(h, v, mouseX * _mouseSensetivity);
        _player.RotateX(-mouseY * _mouseSensetivity);

        if (space) _player.Jump();

        if (isShoot && _gun.TryShoot(out ShootInfo shootInfo)) SendShoot(ref shootInfo);

        SendMove();
    }

    private void SendShoot(ref ShootInfo shootInfo)
    {
        shootInfo.key = _multiplayerManager.GetSessionID();
        string data = JsonUtility.ToJson(shootInfo);

        _multiplayerManager.Send("shoot", data);
    }

    private void SendMove()
    {
        _player.GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY);
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"pX", position.x},
            {"pY", position.y},
            {"pZ", position.z},
            {"vX", velocity.x},
            {"vY", velocity.y},
            {"vZ", velocity.z},
            {"rX", rotateX },
            {"rY", rotateY }

        };
        _multiplayerManager.SendMessage("move", data);
    }

    public void Restart(int spawnIndex)
    {
        _multiplayerManager._spawnPoints.GetPoint(spawnIndex, out Vector3 position, out Vector3 rotation);
        StartCoroutine(Hold());
        
        _player.transform.position = position;
        rotation.x = 0f;
        rotation.z = 0f;
        _player.transform.eulerAngles = rotation;
        _player.SetInput(0,0,0);
        
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "pX", position.x },
            { "pY", position.y },
            { "pZ", position.z },
            { "vX", 0f },
            { "vY", 0f },
            { "vZ", 0f },
            { "rX", 0f },
            { "rY", rotation.y }
        };
        _multiplayerManager.SendMessage("move", data);
    }

    private IEnumerator Hold()
    {
        _hold = true;
        yield return new WaitForSecondsRealtime(_restartDelay);
        _hold = false;
    }
}


[System.Serializable]
public struct ShootInfo
{
    public string key;
    public float dX;
    public float dY;
    public float dZ;
    public float pX;
    public float pY;
    public float pZ;
}

