using Assets;
using Cysharp.Threading.Tasks;

namespace Common
{
    public class AlertPopupProvider : LocalAssetLoader
    {
        public UniTask<AlertPopup> Load()
        {
            return LoadInternal<AlertPopup>("AlertPopup");
        }

        public void Unload()
        {
            UnloadInternal();
        }
    }
}