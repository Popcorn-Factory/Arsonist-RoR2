using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ArsonistMod.Modules
{
    public static class Buffs
    {
        // armor buff gained during roll
        internal static BuffDef armorBuff;
        internal static BuffDef masochismBuff;
        internal static BuffDef flareStrongBuff;
        internal static BuffDef FlareWeakBuff;
        internal static BuffDef blueBuff;
        internal static BuffDef fallDamageReductionBuff;

        internal static void RegisterBuffs()
        {
            Sprite tempIcon = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdSlow80.asset").WaitForCompletion().iconSprite;
            Sprite shieldSprite = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdHiddenInvincibility.asset").WaitForCompletion().iconSprite;
            armorBuff = AddNewBuff("ArsonistArmorBuff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texBuffGenericShield"), Color.white, false, false);
            masochismBuff = AddNewBuff("Arsonist Masochism Overheat Buff", tempIcon, Color.red, false, false);
            flareStrongBuff = AddNewBuff("Flare Strong Burn", tempIcon, Color.blue, true, true);
            FlareWeakBuff = AddNewBuff("Flare Weak Burn", tempIcon, Color.yellow, true, true);
            blueBuff = AddNewBuff("Blue Heat Gauge Buff", Assets.blazingBuffIcon, Color.cyan, false, true);
            fallDamageReductionBuff = AddNewBuff("Fall Damage Reduction", shieldSprite, Color.yellow, false, true);
        }

        // simple helper method
        internal static BuffDef AddNewBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff)
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
            buffDef.name = buffName;
            buffDef.buffColor = buffColor;
            buffDef.canStack = canStack;
            buffDef.isDebuff = isDebuff;
            buffDef.eliteDef = null;
            buffDef.iconSprite = buffIcon;

            Modules.Content.AddBuffDef(buffDef);

            return buffDef;
        }
    }
}