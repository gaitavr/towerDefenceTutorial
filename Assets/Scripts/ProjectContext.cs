using Core.Pause;
using Login;
using UnityEngine;

public class ProjectContext : MonoBehaviour
{
    public LoadingScreenProvider LoadingScreenProvider { get; private set; }
    public LoginWindowProvider LoginWindowProvider { get; private set; }
    public AssetProvider AssetProvider { get; private set; }
    public PauseManager PauseManager { get; private set; }

    public static ProjectContext Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void Initialize()
    {
        LoadingScreenProvider = new LoadingScreenProvider();
        LoginWindowProvider = new LoginWindowProvider();
        AssetProvider = new AssetProvider();
        PauseManager = new PauseManager();
    }
}