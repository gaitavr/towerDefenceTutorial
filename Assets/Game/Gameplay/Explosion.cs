using UnityEngine;

public class Explosion : WarEntity
{
    [SerializeField, Range(0f, 1f)] private float _duration = 0.5f;
    [Space]
    [SerializeField] private AnimationCurve _scaleCurve;
    [SerializeField] private AnimationCurve _colorCurve;

    private float _age;
    private float _scale;
    private MeshRenderer _meshRenderer;
    
    private static int _colorPropId = Shader.PropertyToID("_Color");
    private static MaterialPropertyBlock _propertyBlock;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Initialize(Vector3 position, float blastRadius, float damage = 0f)
    {
        if (damage > 0f)
        {
            TargetPoint.FillBufferInCapsule(position, blastRadius);
            for (var i = 0; i < TargetPoint.BufferedCount; i++)
            {
                TargetPoint.GetBuffered(i).Enemy.TakeDamage(damage);
            }
        }
        transform.localPosition = position;
        _scale = 2f * blastRadius;
    }

    public override bool GameUpdate()
    {
        _age += Time.deltaTime;
        if (_age >= _duration)
        {
            OriginFactory.Reclaim(this);
            return false;
        }

        _propertyBlock ??= new MaterialPropertyBlock();

        var t = _age / _duration;
        var c = Color.yellow;
        c.a = _colorCurve.Evaluate(t);
        _propertyBlock.SetColor(_colorPropId, c);
        _meshRenderer.SetPropertyBlock(_propertyBlock);
        transform.localScale = Vector3.one * (_scale * _scaleCurve.Evaluate(t));
        return true;
    }
}
