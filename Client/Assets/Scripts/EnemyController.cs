using System.Collections.Generic;
using Colyseus.Schema;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyCharacter _character;
    [SerializeField] private EnemyGun _enemyGun;
    private List<float> _recievTimeInterval = new List<float> { 0, 0, 0, 0, 0 };
    private float _lastRecieveTime = 0f;
    private Player _player;
    private float AverageInterval
    {
        get
        {
            int recieveTimeInternalCount = _recievTimeInterval.Count;
            float summ = 0;
            for (int i = 0; i < recieveTimeInternalCount; ++i)
            {
                summ += _recievTimeInterval[i];
            }
            return summ / recieveTimeInternalCount;
        }
    }

    private void SaveRecieveTime()
    {
        float interval = Time.time - _lastRecieveTime;
        _lastRecieveTime = Time.time;
        _recievTimeInterval.Add(interval);
        _recievTimeInterval.RemoveAt(0);
    }
    internal void OnChange(List<DataChange> changes)
    {
        Vector3 position = _character.targetPosition;
        Vector3 velocity = _character.velocity;

        foreach (var dataChange in changes)
        {
            switch (dataChange.Field)
            {
                case "pX":
                    position.x = (float)dataChange.Value;
                    break;
                case "pY":
                    position.y = (float)dataChange.Value;
                    break;
                case "pZ":
                    position.z = (float)dataChange.Value;
                    break;
                case "vX":
                    velocity.x = (float)dataChange.Value;
                    break;
                case "vY":
                    velocity.y = (float)dataChange.Value;
                    break;
                case "vZ":
                    velocity.z = (float)dataChange.Value;
                    break;
                case "rX":
                    _character.SetRotateX((float)dataChange.Value);
                    break;
                case "rY":
                    _character.SetRotateY((float)dataChange.Value);
                    break;
                default:
                    Debug.LogWarning("Dont know this field: " + dataChange.Field);
                    break;
            }
        }
        _character.SetMovement(position, velocity, AverageInterval);
        transform.position = position;
    }

    public void Init(string key, Player player)
    {
        _character.Init(key);
        _player = player;
        _character.SetSpeed(player.speed);
        _character.SetMaxHP(player.maxHP);
        player.OnChange += OnChange;
    }

    public void Shoot(in ShootInfo info)
    {
        Vector3 position = new Vector3(info.pX, info.pY, info.pZ);
        Vector3 velocity = new Vector3(info.dX, info.dY, info.dZ);

        _enemyGun.Shoot(position, velocity);
    }

    public void Destroy()
    {
        _player.OnChange -= OnChange;
        Destroy(gameObject);
    }
}
