using ArsonistMod.SkillStates;
using ArsonistMod.SkillStates.BaseStates;
using System.Collections.Generic;
using System;
using ArsonistMod.SkillStates.Arsonist.Secondary;
using ArsonistMod.SkillStates.EmoteStates;
using ArsonistMod.SkillStates.ZeroPointBlast;

namespace ArsonistMod.Modules
{
    public static class States
    {
        internal static void RegisterStates()
        {
            Modules.Content.AddEntityState(typeof(BaseMeleeAttack));
            
            Modules.Content.AddEntityState(typeof(FireSpray));
            Modules.Content.AddEntityState(typeof(Flamethrower));
            Modules.Content.AddEntityState(typeof(Flare));

            //Zero point states
            Modules.Content.AddEntityState(typeof(ZeroPointBlastStart));
            Modules.Content.AddEntityState(typeof(ZeroPointBlastEnd));
            Modules.Content.AddEntityState(typeof(ZeroPointBlastWhiff));

            Modules.Content.AddEntityState(typeof(Cleanse));
            Modules.Content.AddEntityState(typeof(NeoMasochism));
            Modules.Content.AddEntityState(typeof(Masochism));
            Modules.Content.AddEntityState(typeof(EmoteSit));
            Modules.Content.AddEntityState(typeof(EmoteStrut));
            Modules.Content.AddEntityState(typeof(EmoteLobby));
        }
    }
}