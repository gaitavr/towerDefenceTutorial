

namespace GamePlay.Attack
{
    public class ElienView : EnemyView
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
