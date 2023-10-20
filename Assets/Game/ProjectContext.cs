using Core.Pause;
using UnityEngine;
using Utils.Assets;
using Core.Loading;
using Core;
using Core.Communication;

public sealed class ProjectContext : MonoBehaviour
{
    [SerializeField] private Camera _uiCamera;

    public UserContainer UserContainer { get; private set; }
    public IUserStateCommunicator UserStateCommunicator { get; private set; }
    public IPvpCommunicator PvpCommunicator { get; private set; }
    public LoadingScreenProvider LoadingScreenProvider { get; private set; }
    public AssetProvider AssetProvider { get; private set; }
    public PauseManager PauseManager { get; private set; }
    public Camera UICamera => _uiCamera;

    public static ProjectContext I { get; private set; }

    private void Awake()
    {
        I = this;
        DontDestroyOnLoad(this);
    }

    public void Initialize()
    {
        UserContainer = new UserContainer();
        UserStateCommunicator = new LocalUserStateCommunicator();
        PvpCommunicator = new LocalPvpCommunicator();
        LoadingScreenProvider = new LoadingScreenProvider();
        AssetProvider = new AssetProvider();
        PauseManager = new PauseManager();
    }
}