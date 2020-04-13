using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Transform _model;

    public EnemyFactory OriginFactory { get; set; }

    private GameTile _tileFrom, _tileTo;
    private Vector3 _positionFrom, _positionTo;
    private float _progress, _progressFactor;

    private Direction _direction;
    private DirectionChange _directionChange;
    private float _directionAngleFrom, _directionAngleTo;
    private float _pathOffset;
    private float _speed;

    public void Initialize(float scale, float pathOffset, float speed)
    {
        _model.localScale = new Vector3(scale, scale, scale);
        _pathOffset = pathOffset;
        _speed = speed;
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

    public bool GameUpdate()
    {
        _progress += Time.deltaTime * _progressFactor;
        while (_progress >= 1)
        {
            if (_tileTo == null)
            {
                OriginFactory.Reclaim(this);
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

        switch (_directionChange)
        {
            case DirectionChange.None: PrepareForward();break;
            case DirectionChange.TurnRight: PrepareTurnRight();break;
            case DirectionChange.TurnLeft: PrepareTurnLeft();break;
            default: PrepareTurnAround();break;
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
        _progressFactor = _speed / (Mathf.PI * 0.5f * (0.5f - _pathOffset));
    }

    private void PrepareTurnLeft()
    {
        _directionAngleTo = _directionAngleFrom - 90f;
        _model.localPosition = new Vector3(_pathOffset + 0.5f, 0f);
        transform.localPosition = _positionFrom + _direction.GetHalfVector();
        _progressFactor = _speed / (Mathf.PI * 0.5f * (0.5f - _pathOffset));
    }

    private void PrepareTurnAround()
    {
        _directionAngleTo = _directionAngleFrom + (_pathOffset < 0f ? 180f : -180f);
        _model.localPosition = new Vector3(_pathOffset, 0f);
        transform.localPosition = _positionFrom;
        _progressFactor = _speed / (Mathf.PI * Mathf.Max(Mathf.Abs(_pathOffset), 0.2f));
    }
}