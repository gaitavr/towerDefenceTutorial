using Assets.Game.Gameplay.Attack.Common;
using System;
using Utils;

namespace GamePlay.Attack
{
    [Serializable]
    public class EnemyConfig
    {
        public Enemy Prefab;
        [FloatRangeSlider(0.5f, 2f)]
        public FloatRange Scale = new FloatRange(1f);
        [FloatRangeSlider(-0.4f, 0.4f)]
        public FloatRange PathOffset = new FloatRange(0f);
        [FloatRangeSlider(0.2f, 5f)]
        public FloatRange Speed = new FloatRange(1f);
        [FloatRangeSlider(10f, 1000f)]
        public FloatRange Health = new FloatRange(100f);
        [FloatRangeSlider(1f, 100f)]
        public FloatRange Damage = new FloatRange(5f);

        public EnemyContext ToContext()
        {
            var damage = Damage.RandomValueInRange;
            return new EnemyContext(Scale.RandomValueInRange, PathOffset.RandomValueInRange,
                Speed.RandomValueInRange, Health.RandomValueInRange, (int)damage);
        }
    }
}