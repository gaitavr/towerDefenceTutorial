using Cysharp.Threading.Tasks;
using Game.Core;
using UnityEngine;

namespace Game.Defend.TilesBuilder
{
    public class TilesBuilderTrigger : MonoBehaviour
    {
        private void OnMouseDown()
        {
            SceneContext.I.TilesBuilder.Show().Forget();
        }
    }
}