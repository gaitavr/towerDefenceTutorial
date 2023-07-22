using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace GameResult
{
    [RequireComponent(typeof(PlayableDirector))]
    public class GameResultIntroAnimation : MonoBehaviour
    {
        [SerializeField]
        private List<GameResultSetting> _settings;
        
        private PlayableDirector _director;
        private TaskCompletionSource<bool> _playAwater;

        private void Awake()
        {
            _director = GetComponent<PlayableDirector>();
        }

        public async Task Play(GameResultType result)
        {
            foreach (var s in _settings)
            {
                s.Object.SetActive(s.Type == result);
            }
           
            _playAwater = new TaskCompletionSource<bool>();
            _director.stopped -= OnTimelineFinished;
            _director.stopped += OnTimelineFinished;
            
            _director.Play();
            await _playAwater.Task;
        }

        private void OnTimelineFinished(PlayableDirector _)
        {
            _playAwater.TrySetResult(true);
        }
        
        [Serializable]
        private class GameResultSetting
        {
            public GameResultType Type;
            public GameObject Object;
        }
    }
}
