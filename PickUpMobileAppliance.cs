using Kitchen;
using KitchenMods;

namespace KitchenGetOverHere
{
    public class KitchenGetOverHere : ItemInteractionSystem, IModSystem
    {
        protected override InteractionType RequiredType => InteractionType.Grab;

        private EntityContext ctx;

        protected override bool IsPossible(ref InteractionData data)
        {
            ctx = data.Context;
            if (!ctx.Has<CMobileAppliance>(data.Target) || !ctx.Has<CPosition>(data.Target))
                return false;
            if (!ctx.Require(data.Interactor, out CItemHolder holder) || holder.HeldItem != default)
                return false;
            return true;
        }

        protected override void Perform(ref InteractionData data)
        {
            //ctx.Set(data.Target, default(CPosition));
            ctx.Set(data.Target, default(CHeldAppliance));
            ctx.Add<CRemoveView>(data.Target);
            ctx.Set(data.Target, new CRequiresView
            {
                Type = ViewType.HeldAppliance
            });
            ctx.UpdateHolder(data.Target, data.Interactor);
        }
    }
}
