using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField, Range(0, 20)] private float _speed;

    [SerializeField] private Vector3 _direction;

    private bool _isEnable;

    private void Update()
    {
        if (_isEnable == false)
            return;

        transform.Translate(_direction * _speed * Time.deltaTime);
    }

    public void Enable() => _isEnable = true;

    public void Disable() => _isEnable = false;
}
