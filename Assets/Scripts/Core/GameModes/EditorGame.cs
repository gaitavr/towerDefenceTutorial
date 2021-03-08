using System.Collections.Generic;
using UnityEngine;

public class EditorGame : MonoBehaviour
{
    [SerializeField]
    private Vector2Int _boardSize;
    [SerializeField]
    private GameBoard _board;
    [SerializeField]
    private TilesBuilder _tilesBuilder;
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private GameTileContentFactory _contentFactory;
    
    private readonly BoardSerializer _serializer = new BoardSerializer();
    
    public IEnumerable<GameObjectFactory> Factories => new GameObjectFactory[]{_contentFactory};
    
    public void Init()
    {
        var initialData = GenerateInitialData();
        _board.Initialize(initialData, _contentFactory);
        _tilesBuilder.Initialize(_contentFactory, _camera, _board);
    }
    
    private BoardData GenerateInitialData()
    {
        var result = _serializer.Load();
        if (result == null)
        {
            result = new BoardData
            {
                X = (byte) _boardSize.x,
                Y = (byte) _boardSize.y,
                Content = new GameTileContentType[_boardSize.x * _boardSize.y]
            };
            result.Content[0] = GameTileContentType.SpawnPoint;
            result.Content[result.Content.Length - 1] = GameTileContentType.Destination;
        }
        return result;
    }
    
    public void BeginNewGame()
    {
        Cleanup();
        _tilesBuilder.Enable();
    }
    
    public void Cleanup()
    {
        _tilesBuilder.Disable();
        _board.Clear();
    }
    
    private void GoToMainMenu()
    {
        // var operations = new Queue<ILoadingOperation>();
        // operations.Enqueue(new ClearGameOperation(this));
        // LoadingScreen.Instance.Load(operations);
    }
    
    // private void Update()
    // {
    //     if (Input.GetKeyUp(KeyCode.Space))
    //     {
    //         var data = new BoardData()
    //         {
    //             Version = Serialization.Serialization.VERSION,
    //             AccountId = 1145,
    //             X = (byte)_size.x,
    //             Y = (byte)_size.y,
    //             Content = _tiles.Select(t => t.Content.Type).ToArray()
    //         };
    //         _serializer.Save(data);
    //     }
    // }
}