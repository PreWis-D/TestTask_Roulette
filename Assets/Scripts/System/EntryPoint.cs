using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private BonusGameHolder _bonusGameHolder;

    private void Start()
    {
        _bonusGameHolder.Init(_camera);
    }
}