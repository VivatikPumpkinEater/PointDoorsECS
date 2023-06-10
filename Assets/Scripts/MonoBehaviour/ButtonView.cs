using UnityEngine;

public class ButtonView : MonoBehaviour
{
    [SerializeField] private float _radius;

    public float Radius => _radius;
    public Vector3 Position => transform.position;
}
