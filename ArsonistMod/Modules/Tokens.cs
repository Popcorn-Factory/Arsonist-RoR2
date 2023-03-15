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

            string desc = "The Arsonist is a close-ranked tank who uses fire as a means to an end. Managing his Overheat meter allows him to deal high amounts of damage to groups of enemies. Balance is crucial to victory." 
                + Environment.NewLine + Environment.NewLine
                + "< ! > Your attacks will be weaker when you're overheating. If you're in a tight spot, Cleanse can immediately end the overheating period in exchange for extending its cooldown duration."
                + Environment.NewLine + Environment.NewLine
                + "< ! > Use Cleanse and Masochism together."
                + Environment.NewLine + Environment.NewLine
                + "< ! > Zero-Point Blast's distance scales according to movement speed"
                + Environment.NewLine + Environment.NewLine
                + "< ! > Items such as Razorwire do not work with Cleanse when Masochism is active. Try finding other means of healing while Cleanse is active if you want to use it in a self-damage build.";

            string outro = "..and so he left, with new bloodlust set ablaze.";
            string outroFailure = "..and so he vanished, his only regret: a job left unfinished.";
            string lore = "A u d i o  r e c o r d i n g  t a k e n  f r o m  p e r s o n a l  r e c o r d i n g  d e v i c e .\r\n\r\nP r i n t i n g  t r a n s c r i p t . . .\r\n\r\nJoining the UES proved to be easy. Especially with their job listings. \r\n\r\nA demolitions expert is not much different from the type of work that I already do. And yes, arson is work. The military had been kind enough to leave the old equipment of the Incendiary Unit lying around for keen eyes like mine to find. It wasn’t difficult learning exactly what they were trying to do with that unit, and it was a damned shame that they shut it down.\r\n\r\nThey were brilliant. They were brilliant, and the UES told them they were psychopaths. I was only able to barely salvage the remnants of what little hadn’t been completely destroyed. Even then, I haven’t gotten much of a chance to test my restorations with the high security of my division.\r\n\r\nI heard from a colleague that there was a job happening. Some sort of rescue mission aboard the UES Safe Travels to an uncharted planet. I’ve heard stories of that place, not so nice ones about the creatures that live there.\r\n\r\nHm.\r\n\r\nMaybe I’ll go. Sounds like I’ll find some very good lab rats.\r\n\r\nE n d  o f  r e c o r d i n g .\r\n\r\n";

            LanguageAPI.Add(prefix + "NAME", "Arsonist");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "Manic Incendiary");
            LanguageAPI.Add(prefix + "LORE", lore);
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
            LanguageAPI.Add(prefix + "ALT_PRIMARY_FIRESPRAY_DESCRIPTION", Helpers.heatPrefix + Helpers.criticalPrefix + $"Fire a ball of fire that deals <style=cIsDamage>{100f * StaticValues.firesprayStrongDamageCoefficient}% damage and ignites</style> enemies on hit. ");
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
            LanguageAPI.Add(prefix + "SPECIAL_MASOCHIST_DESCRIPTION", Helpers.startPrefix + $"Damage from being <style=cIsDamage>Ignited</style> turns into <style=cIsHealing>healing</style> for {StaticValues.masochismBuffDuration} seconds. Increases attack speed <style=cIsDamage>{100*StaticValues.igniteAttackSpeedMultiplier}%</style>.");
            #endregion

            #region Keywords
            LanguageAPI.Add(prefix + "KEYWORD_PASSIVE", $"<style=cKeywordName>Heat</style> " + 
                $"Skills increase/decrease heat. At max heat, arsonist overheats, weakening their skills and causing longer cooldowns. Attack speed increases cooling rate. The selected primary chosen alters the heat gauge. ");
            LanguageAPI.Add(prefix + "KEYWORD_BASEGAUGE", $"<style=cKeywordName>Base Gauge</style> " + 
                $"Heat gauge increase per level as well as with stock-based items. " +
                $"Cooling rate also increases at higher %s of heat.");
            LanguageAPI.Add(prefix + "KEYWORD_CRITICALGAUGE", $"<style=cKeywordName>Critical Gauge</style> " + 
                $"Heat gauge does not increase, instead critical gauge increases per level as well as with stock-based items. " +
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