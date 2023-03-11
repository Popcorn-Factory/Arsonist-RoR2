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

            string desc = "Arsonist is heat.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            //desc = desc + "< ! > Sword is a good all-rounder while Boxing Gloves are better for laying a beatdown on more powerful foes." + Environment.NewLine + Environment.NewLine;
            //desc = desc + "< ! > Pistol is a powerful anti air, with its low cooldown and high damage." + Environment.NewLine + Environment.NewLine;
            //desc = desc + "< ! > Roll has a lingering armor buff that helps to use it aggressively." + Environment.NewLine + Environment.NewLine;
            //desc = desc + "< ! > Bomb can be used to wipe crowds with ease." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, with new bloodlust blazing in his soul.";
            string outroFailure = "..and so he vanished, mortality burning away from flesh.";

            LanguageAPI.Add(prefix + "NAME", "Arsonist");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "The Pyromaniac");
            LanguageAPI.Add(prefix + "LORE", "The lost cinders of the world.");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Pyromania");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "<style=cIsUtility>Heat Gauge</style>. Being <style=cIsDamage>ignited</style> increases <style=cIsUtility>movement speed and damage</style>. " + "<style=cStack>Ifrit's Distinction applies these effects permanently</style>." + "<style=cIsUtility>Take half damage from ignition sources</style>. ");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_FIRESPRAY_NAME", "Overheat");
            LanguageAPI.Add(prefix + "PRIMARY_FIRESPRAY_DESCRIPTION", Helpers.heatPrefix + $"Fire a ball of fire that deals <style=cIsDamage>{100f * StaticValues.firesprayStrongDamageCoefficient}% damage and ignites</style> enemies on hit. ");
            LanguageAPI.Add(prefix + "ALT_PRIMARY_FIRESPRAY_NAME", "Overdrive");
            LanguageAPI.Add(prefix + "ALT_PRIMARY_FIRESPRAY_DESCRIPTION", Helpers.heatPrefix + $"Fire a ball of fire that deals <style=cIsDamage>{100f * StaticValues.firesprayStrongDamageCoefficient}% damage and ignites</style> enemies on hit. ");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_FLAREGUN_NAME", "Signal Flare");
            LanguageAPI.Add(prefix + "SECONDARY_FLAREGUN_DESCRIPTION", Helpers.heatPrefix + $"Fire a long range signal flare that deals <style=cIsDamage>{100f * StaticValues.flareStrongDamageCoefficient}% damage</style> over 5 seconds, then exploding for <style=cIsDamage>{100f * StaticValues.flareStrongDamageCoefficient}% damage</style>.");
            LanguageAPI.Add(prefix + "SECONDARY_PUNCH_NAME", "Zero Point Blast");
            LanguageAPI.Add(prefix + "SECONDARY_PUNCH_DESCRIPTION", Helpers.startPrefix + $"Propel yourself forwards, colliding into the first enemy and dealing <style=cIsDamage>{100f * StaticValues.zeropointpounchDamageCoefficient}% damage</style> in a blast around you. " +
                $" On propulsion, blast enemies behind you for <style=cIsDamage>{100f * StaticValues.zeropointpounchDamageCoefficient}% damage</style>. ");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_CLEANSE_NAME", "Cleanse");
            LanguageAPI.Add(prefix + "UTILITY_CLEANSE_DESCRIPTION", Helpers.startPrefix + $"<style=cIsDamage>Ignite</style> yourself in a blast of fire, dealing <style=cIsDamage>{100f * StaticValues.cleanseDamageCoefficient}%</style> to enemies around you and burning away other status effects. ");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_MASOCHIST_NAME", "Masochism");
            LanguageAPI.Add(prefix + "SPECIAL_MASOCHIST_DESCRIPTION", Helpers.startPrefix + $"Damage from being <style=cIsDamage>Ignited</style> turns into <style=cIsHealing>healing</style> for {StaticValues.masochismBuffDuration} seconds.");
            #endregion

            #region Keywords
            LanguageAPI.Add(prefix + "KEYWORD_PASSIVE", $"<style=cKeywordName>Heat</style> " + 
                $"Skills increase/decrease heat. At max heat, arsonist overheats, weakening their skills and causing longer cooldowns. Attack speed increases cooling rate. Which primary chosen alters the heat gauge. ");
            LanguageAPI.Add(prefix + "KEYWORD_BASEGAUGE", $"<style=cKeywordName>Base Gauge</style> " + 
                $"Heat gauge increase per level as well as with stock-based items. " +
                $"Cooling rate also increases at higher %s of heat.");
            LanguageAPI.Add(prefix + "KEYWORD_BLUEGAUGE", $"<style=cKeywordName>Blue Gauge</style> " + 
                $"Heat gauge does not increase, instead blue gauge increases per level as well as with stock-based items. " +
                $"Gain 2x damage while heat is in blue gauge. ");
            LanguageAPI.Add(prefix + "KEYWORD_FIRESPRAYHEAT", $"<style=cKeywordName>Heat</style> " +
                $"Costs <style=cIsDamage>{Modules.StaticValues.firesprayEnergyCost} heat</style>. Half damage and speed in overheat.");
            LanguageAPI.Add(prefix + "KEYWORD_FLAREHEAT", $"<style=cKeywordName>Heat</style> " + 
                $"Costs <style=cIsDamage>{Modules.StaticValues.flareEnergyCost} heat</style>. Half damage in overheat.");
            LanguageAPI.Add(prefix + "KEYWORD_ZEROPOINTHEAT", $"<style=cKeywordName>Heat</style> " +
                $"Cools <style=cIsUtility>half of CURRENT heat</style>. Half damage in overheat.");
            LanguageAPI.Add(prefix + "KEYWORD_CLEANSEHEAT", $"<style=cKeywordName>Heat</style> " +
                $"Cools <style=cIsUtility>half of TOTAL heat</style>. Accelerates cooling in overheat, however, no ignite.");
            LanguageAPI.Add(prefix + "KEYWORD_MASOCHISMHEAT", $"<style=cKeywordName>Heat</style> " +
                $"Costs <style=cIsDamage>{Modules.StaticValues.masochismEnergyCost} heat</style>. Half healing in overheat.");
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