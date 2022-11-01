using ArsonistMod.SkillStates;
using ArsonistMod.SkillStates.BaseStates;
using System.Collections.Generic;
using System;

namespace ArsonistMod.Modules
{
    public static class States
    {
        internal static void RegisterStates()
        {
            Modules.Content.AddEntityState(typeof(BaseMeleeAttack));
            Modules.Content.AddEntityState(typeof(SlashCombo));

            Modules.Content.AddEntityState(typeof(Shoot));

            Modules.Content.AddEntityState(typeof(Roll));

            Modules.Content.AddEntityState(typeof(ThrowBomb));

            
            Modules.Content.AddEntityState(typeof(FireSpray));
            Modules.Content.AddEntityState(typeof(Cleanse));
        }
    }
}