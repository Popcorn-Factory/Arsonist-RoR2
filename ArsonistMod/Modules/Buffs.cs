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
        internal static BuffDef newMasochismBuff;
        internal static BuffDef flareStrongBuff;
        internal static BuffDef FlareWeakBuff;
        internal static BuffDef blueBuff;
        internal static BuffDef lowerBuff;
        internal static BuffDef fallDamageReductionBuff;
        internal static BuffDef masochismActiveBuff;
        internal static BuffDef masochismDeactivatedDebuff;
        internal static BuffDef masochismDeactivatedNonDebuff;
        internal static BuffDef overheatDebuff;
        internal static BuffDef cleanseSpeedBoost;

        internal static void RegisterBuffs()
        {
            Sprite masochismSprite = Modules.AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("masochismBuff");
            Sprite blueGaugeSprite = Modules.AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("blueGaugeBuff");
            Sprite flareDebuffSprite = Modules.AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("flareDebuff");
            Sprite ZPBBuffSprite = Modules.AssetsArsonist.mainAssetBundle.LoadAsset<Sprite>("ZPBBuff");

            cleanseSpeedBoost = AddNewBuff("Arsonist Cleanse Speed Boost", flareDebuffSprite, Color.blue, false, false);
            masochismDeactivatedNonDebuff = AddNewBuff("Arsonist Overheat Debuff Protection", masochismSprite, Color.gray, false, false);
            masochismDeactivatedDebuff = AddNewBuff("Arsonist Masochism Debuff", masochismSprite, Color.grey, false, true);
            masochismActiveBuff = AddNewBuff("Arsonist Masochism Active", masochismSprite, Color.magenta, false, false);
            armorBuff = AddNewBuff("ArsonistArmorBuff", RoR2.LegacyResourcesAPI.Load<Sprite>("Textures/BuffIcons/texBuffGenericShield"), Color.white, false, false);
            masochismBuff = AddNewBuff("Arsonist Masochism Overheat Buff", masochismSprite, Color.red, false, false);
            newMasochismBuff = AddNewBuff("Arsonist Masochism Buff", masochismSprite, Color.red, true, false);
            flareStrongBuff = AddNewBuff("Flare Strong Burn", flareDebuffSprite, Color.blue, true, true);
            FlareWeakBuff = AddNewBuff("Flare Weak Burn", flareDebuffSprite, Color.yellow, true, true);
            blueBuff = AddNewBuff("Blue Heat Gauge Buff", blueGaugeSprite, Color.cyan, false, false);
            lowerBuff = AddNewBuff("Non-Blue Heat Gauge Debuff", blueGaugeSprite, Color.white, false, false);
            overheatDebuff = AddNewBuff("Overheat Debuff", blueGaugeSprite, Color.red, false, false);
            fallDamageReductionBuff = AddNewBuff("Fall Damage Reduction", ZPBBuffSprite, Color.yellow, false, false);
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