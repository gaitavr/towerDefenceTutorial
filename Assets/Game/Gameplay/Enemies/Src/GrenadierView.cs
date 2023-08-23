

namespace GamePlay
{
    public class GrenadierView : EnemyView
    {
        public void OnDieAnimationFinished()
        {
            _enemy.Recycle();
        }

        public void OnStepAnimation()
        {
            //TODO play sound
        }
    }
}
