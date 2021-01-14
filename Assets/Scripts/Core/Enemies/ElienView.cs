using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
