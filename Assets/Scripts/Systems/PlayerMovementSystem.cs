using System;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;

public class PlayerMovementSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;
        return Entities.ForEach(
            (ref Translation translation, in MoveData moveData) 
            => 
            {
                float3 normalizeDir = math.normalizesafe(moveData.direction);
                translation.Value += normalizeDir * moveData.speed * deltaTime;
            })
        .Schedule(inputDeps);
    }
}
