using UnityEngine;

public sealed class TestComponent : MonoBehaviour
{
    public float param1 = 0;
    [SerializeField] float _param2 = 0;
    [field:SerializeField] public float param3 { get; set; }
    [field:SerializeField] public float Param4 { get; set; }

    void Start()
      => Debug.Log($"{param1 + _param2 + param3 + Param4}");
}
