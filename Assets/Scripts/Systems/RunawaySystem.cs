using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class RunawaySystem : SystemBase
{
    protected override void OnUpdate()
    {
       // float deltaTime = Time.DeltaTime;
        Entities.WithAll<HerbivoreTag>().ForEach(
            (ref Translation translation, in MoveData moveData)
            =>
            {
                translation.Value.x -= moveData.speed * 0.1f;
            }
            ).Schedule();
    }
}
