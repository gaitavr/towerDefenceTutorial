using System.Collections.Generic;
using Core.UI;
using Cysharp.Threading.Tasks;
using Game.Core;
using Game.Defend.Tiles;
using UnityEngine;
using Utils.Serialization;
using Core.Loading;
using Core;

//TODO undo + pattern command
public class BoardEditorMode : MonoBehaviour, ICleanUp
{
    [SerializeField] private EditorHud _hud;

    private Vector2Int _boardSize;

    private TilesBuilderViewController TilesBuilder => SceneContext.I.TilesBuilder;
    private GameBoard GameBoard => SceneContext.I.GameBoard;
    private UserAccountState UserState => ProjectContext.I.UserContainer.State;

    public IEnumerable<GameObjectFactory> Factories => new GameObjectFactory[]
    {
        SceneContext.I.ContentFactory
    };
    public string SceneName => Utils.Constants.Scenes.EDITOR_MODE;

    public void Init(Vector2Int boardSize, string boardName = null)
    {
        _boardSize = boardSize;
        SceneContext.I.Initialize();

        BoardData initialData = null;
        if (boardName != null)
            initialData = UserState.TryGetBoard(boardName);

        initialData ??= BoardData.GetInitial(boardSize);
        GameBoard.Initialize(initialData);
    }
    
    public void BeginNewGame()
    {
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
            Version = 1,
            Name = "test",
            X = (byte)_boardSize.x,
            Y = (byte)_boardSize.y,
            Content = GameBoard.GetAllContentTypes,
            Levels = GameBoard.GetAllContentLevels
        };
        //_serializer.Save(data, _fileName);
    }
}