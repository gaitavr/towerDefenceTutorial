
using GamePlay.Attack;

namespace GamePlay.Defend
{
    public interface IDebuff
    {
        void Assign(Enemy enemy);
        void Remove();
    }
}