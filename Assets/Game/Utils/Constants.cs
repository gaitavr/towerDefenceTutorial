namespace Utils
{
    public sealed class Constants
    {
        public sealed class Scenes
        {
            public const string MAIN_MENU = "MainMenu";
            public const string LOGIN = "Login";
            public const string ATTACK_MODE = "AttackMode";
            public const string DEFEND_MODE = "DefendMode";
            public const string BOARD_EDITOR_MODE = "BoardEditorMode";
        }

        public sealed class Game
        {
            public const int MIN_BOARD_SIZE = 5;
            public const int MAX_BOARD_SIZE = 100;
            public const int MIN_BOARD_NAME_LENGHT = 4;
        }
    }
}