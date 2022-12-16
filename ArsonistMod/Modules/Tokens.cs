using R2API;
using System;

namespace ArsonistMod.Modules
{
    internal static class Tokens
    {
        internal static void AddTokens()
        {
            #region Arsonist
            string prefix = ArsonistPlugin.DEVELOPER_PREFIX + "_ARSONIST_BODY_";

            string desc = "Arsonist is a skilled fighter who makes use of a wide arsenal of weaponry to take down his foes.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Sword is a good all-rounder while Boxing Gloves are better for laying a beatdown on more powerful foes." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Pistol is a powerful anti air, with its low cooldown and high damage." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Roll has a lingering armor buff that helps to use it aggressively." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Bomb can be used to wipe crowds with ease." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, with new bloodlust blazing in his soul.";
            string outroFailure = "..and so he vanished, mortality burning away from flesh.";

            LanguageAPI.Add(prefix + "NAME", "Arsonist");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "The Chosen One");
            LanguageAPI.Add(prefix + "LORE", "sample lore");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Pyromania");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Being ignited increases movement speed and damage.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_FIRESPRAY_NAME", "Firespray");
            LanguageAPI.Add(prefix + "PRIMARY_FIRESPRAY_DESCRIPTION", $"Fire a fireball, <style=cIsDamage>igniting enemies</style> exploding for <style=cIsDamage>{100f * StaticValues.firesprayStrongDamageCoefficient}% damage</style>.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_GUN_NAME", "Flare Gun");
            LanguageAPI.Add(prefix + "SECONDARY_GUN_DESCRIPTION", $"Fire a long range signal flare that deals <style=cIsDamage>{100f * StaticValues.flareGunStrongDamageCoefficient}% damage over 5 seconds</style> before exploding dealing <style=cIsDamage>{100f * StaticValues.flareGunStrongDamageCoefficient}% damage</style>.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_CLEANSE_NAME", "Cleanse");
            LanguageAPI.Add(prefix + "UTILITY_CLEANSE_DESCRIPTION", $"Ignite yourself in a blast of fire, dealing <style=cIsDamage>{100f * StaticValues.cleanseDamageCoefficient}%</style> damage to enemies around you and burning away other status effects.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_NAME", "Masochism");
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", "Damage from being ignited turns into healing over time.");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Arsonist: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Arsonist, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Arsonist: Mastery");
            #endregion
            #endregion
        }
    }
}