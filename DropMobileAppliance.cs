using Kitchen;
using KitchenMods;
using Unity.Entities;
using UnityEngine;

namespace KitchenGetOverHere
{
    public class DropMobileAppliance : ItemInteractionSystem, IModSystem
    {
        protected override InteractionType RequiredType => InteractionType.Grab;

        private Entity Mobile;

        EntityContext ctx;

        protected override bool IsPossible(ref InteractionData data)
        {
            ctx = data.Context;
            if (!ctx.Require(data.Interactor, out CItemHolder holder) || holder.HeldItem == default || !ctx.Has<CMobileAppliance>(holder.HeldItem))
                return false;
            Mobile = holder.HeldItem;
            if (!Bounds.Contains(data.Attempt.Location.Rounded()))
                return false;
            return true;
        }

        protected override void Perform(ref InteractionData data)
        {
            Entity occupant = GetOccupant(data.Attempt.Location);
            if (occupant != default && !ctx.Has<CAllowMobilePathing>(occupant))
                return;

            ctx.Set(Mobile, new CPosition(data.Attempt.Location.Rounded()));
            ctx.Remove<CHeldAppliance>(Mobile);
            ctx.Remove<CHeldBy>(Mobile);
            ctx.Add<CRemoveView>(Mobile);
            ctx.Set(Mobile, new CRequiresView
            {
                Type = ViewType.Appliance
            });
            ctx.Set(data.Interactor, default(CItemHolder));
            
        }
    }
}
