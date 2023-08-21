using Core.Pause;
using UnityEngine;
using Utils.Assets;
using Core.Loading;
using Core;

public class ProjectContext : MonoBehaviour
{
    public UserContainer UserContainer { get; private set; }
    public LoadingScreenProvider LoadingScreenProvider { get; private set; }
    public AssetProvider AssetProvider { get; private set; }
    public PauseManager PauseManager { get; private set; }

    public static ProjectContext I { get; private set; }

    private void Awake()
    {
        I = this;
        DontDestroyOnLoad(this);
    }

    public void Initialize()
    {
        UserContainer = new UserContainer();
        LoadingScreenProvider = new LoadingScreenProvider();
        AssetProvider = new AssetProvider();
        PauseManager = new PauseManager();
    }
}