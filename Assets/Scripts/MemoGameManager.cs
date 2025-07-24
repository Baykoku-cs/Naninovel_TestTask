using UnityEngine;

public class MemoGameCamera : MonoBehaviour
{
    public static MemoGameCamera Instance { get; private set; }

    [SerializeField]
    private Camera _camera;

    private void Awake()
    {
        if (Instance is not null)
        {
            Debug.LogError("MemoGameCamera instance already exists.");
        }

        Instance = this;
    }
    
    public void ToggleCamera(bool isEnabled)
    {
        _camera.enabled = isEnabled;
    }
}
