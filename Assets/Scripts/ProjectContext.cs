using UnityEngine;

public class ProjectContext : MonoBehaviour
{
    public LoadingScreenProvider LoadingScreenProvider { get; private set; }

    public static ProjectContext Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void Initialize()
    {
        LoadingScreenProvider = new LoadingScreenProvider();
    }
}