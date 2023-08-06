using System.Collections.Generic;
using Common;
using Core.UI;
using Cysharp.Threading.Tasks;
using Game.Core;
using Game.Defend.Tiles;
using Loading;
using UnityEngine;

public class EditorGame : MonoBehaviour, ICleanUp
{
    [SerializeField] private Vector2Int _boardSize;
    [SerializeField] private EditorHud _hud;
    
    private readonly BoardSerializer _serializer = new();
    private string _fileName;
    
    public IEnumerable<GameObjectFactory> Factories => new GameObjectFactory[]
    {
        SceneContext.I.ContentFactory
    };
    public string SceneName => Constants.Scenes.EDITOR_GAME;
    
    private TilesBuilderViewController TilesBuilder => SceneContext.I.TilesBuilder;
    private GameBoard GameBoard => SceneContext.I.GameBoard;

    public void Init(string fileName)
    {
        SceneContext.I.Initialize();
        _fileName = fileName;
        var initialData = GenerateInitialData();
        GameBoard.Initialize(initialData);
    }

    private BoardData GenerateInitialData()
    {
        var result = _serializer.Load(_fileName);
        if (result == null)
        {
            result = new BoardData
            {
                X = (byte) _boardSize.x,
                Y = (byte) _boardSize.y,
                Content = new GameTileContentType[_boardSize.x * _boardSize.y]
            };
            result.Content[0] = GameTileContentType.SpawnPoint;
            result.Content[^1] = GameTileContentType.Destination;
        }
        return result;
    }
    
    public void BeginNewGame()
    {
        Cleanup();
        _hud.QuitGame += GoToMainMenu;
        _hud.SaveClicked += OnSaveClicked;
        TilesBuilder.SetActive(true);
    }
    
    public void Cleanup()
    {
        _hud.QuitGame -= GoToMainMenu;
        _hud.SaveClicked -= OnSaveClicked;
        TilesBuilder.SetActive(false);
        GameBoard.Clear();
    }
    
    private void Update()
    {
        SceneContext.I.GameTileRaycaster.GameUpdate();
        TilesBuilder.GameUpdate();
    }
    
    private void GoToMainMenu()
    {
        var operations = new Queue<ILoadingOperation>();
        operations.Enqueue(new ClearGameOperation(this));
        ProjectContext.I.LoadingScreenProvider.LoadAndDestroy(operations).Forget();
    }
    
    private void OnSaveClicked()
    {
        var data = new BoardData()
        {
            Version = Serialization.VERSION,
            AccountId = 1145,
            X = (byte)_boardSize.x,
            Y = (byte)_boardSize.y,
            Content = GameBoard.GetAllContentTypes,
            Levels = GameBoard.GetAllContentLevels
        };
        _serializer.Save(data, _fileName);
    }
}