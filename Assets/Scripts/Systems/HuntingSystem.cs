using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using Unity.Jobs;

public class HuntingSystem : SystemBase
{
    private struct SearchPreyJob : IJob
    {
        public NativeArray<float3> result;
        public void Execute()
        {
            result[0] = new float3(0f, 0f, 0f);
        }
    }
    protected override void OnUpdate()
    {
        EntityQuery entityQuery = GetEntityQuery(typeof(HerbivoreTag), ComponentType.ReadOnly<Translation>());
        NativeArray<Translation> preys = entityQuery.ToComponentDataArray<Translation>(Unity.Collections.Allocator.TempJob);

        Entities.WithAll<PredatorTag>().ForEach(
            (ref Translation translation, ref MoveData moveData)
            =>
            {
                // get closest prey
                float3 nearestPreyPosition = new float3(float.MaxValue, float.MaxValue, float.MaxValue);
                for (int i = 0; i < preys.Length; i++)
                {
                    if (math.distancesq(translation.Value, preys[i].Value) < math.distancesq(nearestPreyPosition, preys[i].Value))
                    {
                        nearestPreyPosition = preys[i].Value;
                    }
                }

                // set MoveData to direction away from neares predator
                moveData.direction = math.normalizesafe(nearestPreyPosition - translation.Value);
            }
            ).WithDisposeOnCompletion(preys).ScheduleParallel();
    }
}
