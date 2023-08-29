using System;
using UnityEngine;

namespace GamePlay.Defend
{
    public class TargetPointTrigger : MonoBehaviour
    {
        public event Action<TargetPoint> Entered;

        public void UpdateSelf()
        {
            if (TargetPoint.FillBufferInBox(transform.position, Vector3.one * 0.5f))
            {
                foreach (var targetPoint in TargetPoint.TargetPoints())
                {
                    Entered?.Invoke(targetPoint);
                }
            }
        }
    }
}