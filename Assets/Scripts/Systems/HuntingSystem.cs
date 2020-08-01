using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class HuntingSystem : SystemBase
{
    protected override void OnUpdate()
    {
       // float deltaTime = Time.DeltaTime;
        Entities.WithAll<PredatorTag>().ForEach(
            (ref Translation translation, in MoveData moveData)
            =>
            {
                translation.Value.x += moveData.speed * 0.1f;
            }
            ).Schedule();
    }
}
