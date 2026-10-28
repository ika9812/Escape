using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Collider _playerCollider;
    [SerializeField] Enemy[] _enemies;
    private void OnEnable()
    {
        foreach(var enemy in _enemies)
        {
            enemy._playerCollider = _playerCollider;
        }
    }
}
