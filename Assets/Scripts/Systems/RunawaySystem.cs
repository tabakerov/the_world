using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Collections;

public class RunawaySystem : SystemBase
{
    protected override void OnUpdate()
    {
        // float deltaTime = Time.DeltaTime;
        EntityQuery entityQuery = GetEntityQuery(typeof(PredatorTag), ComponentType.ReadOnly<Translation>());
        NativeArray<Translation> predators = entityQuery.ToComponentDataArray<Translation>(Unity.Collections.Allocator.TempJob);

        Entities.WithAll<HerbivoreTag>().ForEach(
            (ref Translation translation, ref MoveData moveData)
            =>
            {
                // get closest predator
                float3 nearestPredatorPosition = new float3(float.MaxValue, float.MaxValue, float.MaxValue);
                for (int i = 0; i < predators.Length; i++)
                {
                    if (math.distancesq(translation.Value, predators[i].Value) < math.distancesq(nearestPredatorPosition, predators[i].Value))
                    {
                        nearestPredatorPosition = predators[i].Value;
                    }
                    Debug.Log(nearestPredatorPosition);
                }

                // set MoveData to direction away from neares predator
                moveData.direction = math.normalizesafe(translation.Value - nearestPredatorPosition);
            }
            ).ScheduleParallel();
    }
}
