using R2API;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArsonistMod.Modules
{
    public static class Damage
    {
        internal static DamageAPI.ModdedDamageType arsonistStickyDamageType;
        internal static DamageAPI.ModdedDamageType arsonistWeakStickyDamageType;
        internal static DamageAPI.ModdedDamageType arsonistChildExplosionDamageType;

        internal static void SetupModdedDamage()
        {
            arsonistStickyDamageType = DamageAPI.ReserveDamageType();
            arsonistWeakStickyDamageType = DamageAPI.ReserveDamageType();
            arsonistChildExplosionDamageType = DamageAPI.ReserveDamageType();
        }
    }
}
