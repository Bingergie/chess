using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    private static T _instance;

    public static T Instance {
        get => _instance;
        private set {
            if (_instance == null) {
                _instance = value;
            }
            else if (_instance != value) {
                Debug.Log($"{nameof(T)} instance already exists, destroying {nameof(value)}");
                Destroy(value.gameObject);
            }
        }
    }

    protected void Awake() {
        Instance = this as T;
    }
}