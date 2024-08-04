using UnityEngine;

public class DotDestroySelf : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}