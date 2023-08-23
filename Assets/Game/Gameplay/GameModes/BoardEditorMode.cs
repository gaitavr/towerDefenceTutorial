using System.Collections.Generic;
using Core.UI;
using Cysharp.Threading.Tasks;
using Game.Defend.Tiles;
using UnityEngine;
using Utils.Serialization;
using Core.Loading;
using Core;
using Utils;

namespace GamePlay
{
    public class BoardEditorMode : MonoBehaviour, ICleanUp
    {
        [SerializeField] private EditorHud _hud;

        private BoardData _boardData;

        private TilesBuilderViewController TilesBuilder => SceneContext.I.TilesBuilder;
        private GameBoard GameBoard => SceneContext.I.GameBoard;
        private UserAccountState UserState => ProjectContext.I.UserContainer.State;

        public IEnumerable<GameObjectFactory> Factories => new GameObjectFactory[]
        {
        SceneContext.I.ContentFactory
        };
        public string SceneName => Constants.Scenes.EDITOR_MODE;

        public void Init(BoardContext boardContext)
        {
            SceneContext.I.Initialize();

            _boardData = UserState.TryGetBoard(boardContext.Name);
            _boardData ??= BoardData.GetInitial(boardContext.Size);
            GameBoard.Initialize(_boardData);
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
            _boardData.Content = GameBoard.GetAllContentTypes;
            _boardData.Levels = GameBoard.GetAllContentLevels;

            UserState.AddOrReplaceBoard(_boardData);

            ProjectContext.I.UserStateCommunicator.SaveUserState(UserState);
        }
    }
}