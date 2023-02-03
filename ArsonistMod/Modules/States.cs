﻿using ArsonistMod.SkillStates;
using ArsonistMod.SkillStates.BaseStates;
using System.Collections.Generic;
using System;
using ArsonistMod.SkillStates.Arsonist.Secondary;

namespace ArsonistMod.Modules
{
    public static class States
    {
        internal static void RegisterStates()
        {
            Modules.Content.AddEntityState(typeof(BaseMeleeAttack));
            
            Modules.Content.AddEntityState(typeof(FireSpray));
            Modules.Content.AddEntityState(typeof(Flare));
            Modules.Content.AddEntityState(typeof(ZeroPointPunch));
            Modules.Content.AddEntityState(typeof(Cleanse));
            Modules.Content.AddEntityState(typeof(Masochism));
        }
    }
}