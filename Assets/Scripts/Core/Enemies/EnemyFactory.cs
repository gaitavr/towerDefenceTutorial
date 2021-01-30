﻿using System;
using UnityEngine;

[CreateAssetMenu]
public class EnemyFactory : GameObjectFactory
{
    [Serializable]
    class EnemyConfig
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
    }

    [SerializeField] private EnemyConfig _chomper, _elien, _golem, _grenadier;

    public Enemy Get(EnemyType type)
    {
        var config = GetConfig(type);
        Enemy instance = CreateGameObjectInstance(config.Prefab);
        instance.OriginFactory = this;
        instance.Initialize(config.Scale.RandomValueInRange, config.PathOffset.RandomValueInRange,
            config.Speed.RandomValueInRange, config.Health.RandomValueInRange);
        return instance;
    }

    private EnemyConfig GetConfig(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Golem:
                return _golem;
            case EnemyType.Elien:
                return _elien;
            case EnemyType.Chomper:
                return _chomper;
            case EnemyType.Grenadier:
                return _grenadier;
        }
        Debug.LogError($"No config for: {type}");
        return _elien;
    }

    public void Reclaim(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }
}
