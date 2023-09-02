using Assets.Game.Gameplay.Attack.Common;
using Core;
using GamePlay.Defend;
using GamePlay.Modes;
using UnityEngine;
using Utils.Extensions;

namespace GamePlay.Attack
{
    public class Enemy : GameBehavior
    {
        [SerializeField] private Transform _model;
        [SerializeField] private EnemyView _view;

        public EnemyFactory OriginFactory { get; set; }

        private GameTile _tileFrom, _tileTo;
        private Vector3 _positionFrom, _positionTo;
        private float _progress, _progressFactor;

        private Direction _direction;
        private DirectionChange _directionChange;
        private float _directionAngleFrom, _directionAngleTo;
        private float _pathOffset;
        private float _speed;
        private float _originalSpeed;
        private float _tempSpeedFactor;
        private int _damage;

        public float Scale { get; private set; }
        private float Health { get; set; }

        public DebuffEnemyMediator DebuffMediator { get; private set; }
        protected IGameEntityInteructionProxy InterructionProxy => SceneContext.I.EnemyInteructionProxy;

        private const float CHANGE_DIR_SPEED_MULTIPLIER = 0.8f;

        public void Initialize(EnemyContext context)
        {
            _originalSpeed = context.Speed;
            _model.localScale = new Vector3(context.Scale, context.Scale, context.Scale);
            _pathOffset = context.PathOffset;
            _speed = context.Speed;
            Scale = context.Scale;
            Health = context.Health;
            _damage = context.Damage;
            DebuffMediator = new DebuffEnemyMediator(this);
            _view.Init(this);
        }

        public void SpawnOn(GameTile tile)
        {
            transform.localPosition = tile.transform.localPosition;
            _tileFrom = tile;
            _tileTo = tile.NextTileOnPath;
            _progress = 0f;
            PrepareIntro();
        }

        private void PrepareIntro()
        {
            _positionFrom = _tileFrom.transform.localPosition;
            _positionTo = _tileFrom.ExitPoint;
            _direction = _tileFrom.PathDirection;
            _directionChange = DirectionChange.None;
            _directionAngleFrom = _directionAngleTo = _direction.GetAngle();
            _model.localPosition = new Vector3(_pathOffset, 0f);
            transform.localRotation = _direction.GetRotation();
            _progressFactor = 2f * _speed;
        }

        private void PrepareOutro()
        {
            _positionTo = _tileFrom.transform.localPosition;
            _directionChange = DirectionChange.None;
            _directionAngleTo = _direction.GetAngle();
            _model.localPosition = new Vector3(_pathOffset, 0f);
            transform.localRotation = _direction.GetRotation();
            _progressFactor = 2f * _speed;
        }

        public override bool GameUpdate()
        {
            if (_view.IsInited == false)
            {
                return true;
            }
            if (Health <= 0f)
            {
                DisableView();
                _view.Die();
                return false;
            }

            _progress += Time.deltaTime * _progressFactor;
            while (_progress >= 1)
            {
                if (_tileTo == null)
                {
                    InterructionProxy.EnemyReachedDestination(_damage);
                    Recycle();
                    return false;
                }

                _progress = (_progress - 1f) / _progressFactor;
                PrepareNextState();
                _progress *= _progressFactor;
            }

            if (_directionChange == DirectionChange.None)
            {
                transform.localPosition = Vector3.LerpUnclamped(_positionFrom, _positionTo, _progress);
            }
            else
            {
                float angle = Mathf.LerpUnclamped(_directionAngleFrom, _directionAngleTo, _progress);
                transform.localRotation = Quaternion.Euler(0f, angle, 0f);
            }
            return true;
        }

        public void TakeDamage(float damage)
        {
            Health -= damage;
        }

        public void SetSpeed(float factor)
        {
            _speed = _originalSpeed * factor;
            HandleDirection();
            _view.SetSpeedFactor(factor);
        }

        private void PrepareNextState()
        {
            _tileFrom = _tileTo;
            _tileTo = _tileTo.NextTileOnPath;
            _positionFrom = _positionTo;
            if (_tileTo == null)
            {
                PrepareOutro();
            }
            _positionTo = _tileFrom.ExitPoint;
            _directionChange = _direction.GetDirectionChangeTo(_tileFrom.PathDirection);
            _direction = _tileFrom.PathDirection;
            _directionAngleFrom = _directionAngleTo;

            HandleDirection();
        }

        private void HandleDirection()
        {
            switch (_directionChange)
            {
                case DirectionChange.None: PrepareForward(); break;
                case DirectionChange.TurnRight: PrepareTurnRight(); break;
                case DirectionChange.TurnLeft: PrepareTurnLeft(); break;
                default: PrepareTurnAround(); break;
            }
        }

        private void PrepareForward()
        {
            transform.localRotation = _direction.GetRotation();
            _directionAngleTo = _direction.GetAngle();
            _model.localPosition = new Vector3(_pathOffset, 0f);
            _progressFactor = _speed;
        }

        private void PrepareTurnRight()
        {
            _directionAngleTo = _directionAngleFrom + 90f;
            _model.localPosition = new Vector3(_pathOffset - 0.5f, 0f);
            transform.localPosition = _positionFrom + _direction.GetHalfVector();
            _progressFactor = _speed * CHANGE_DIR_SPEED_MULTIPLIER;
        }

        private void PrepareTurnLeft()
        {
            _directionAngleTo = _directionAngleFrom - 90f;
            _model.localPosition = new Vector3(_pathOffset + 0.5f, 0f);
            transform.localPosition = _positionFrom + _direction.GetHalfVector();
            _progressFactor = _speed * CHANGE_DIR_SPEED_MULTIPLIER;
        }

        private void PrepareTurnAround()
        {
            _directionAngleTo = _directionAngleFrom + (_pathOffset < 0f ? 180f : -180f);
            _model.localPosition = new Vector3(_pathOffset, 0f);
            transform.localPosition = _positionFrom;
            _progressFactor = _speed * CHANGE_DIR_SPEED_MULTIPLIER;
        }

        public override void Recycle()
        {
            OriginFactory.Reclaim(this);
        }

        private void DisableView()
        {
            _view.GetComponent<TargetPoint>().IsEnabled = false;
        }

        public override void SetPaused(bool isPaused)
        {
            if (isPaused)
                _tempSpeedFactor = _view.SpeedFactor;
            SetSpeed(isPaused ? 0 : _tempSpeedFactor);
        }

        private void OnDestroy()
        {
            DebuffMediator?.Dispose();
        }
    }
}