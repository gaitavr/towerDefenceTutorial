using UnityEngine;

public class IceSlower : IDebuff
{
    public void Assign(Enemy enemy)
    {
        enemy.SetSpeed(0.5f);
    }

    public void Delete(Enemy enemy)
    {
        enemy.SetSpeed(1f);
    }
}
