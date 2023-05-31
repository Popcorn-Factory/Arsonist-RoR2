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

            string desc = "The Arsonist is a close-ranged tank who uses fire as a means to an end. Managing his Overheat meter allows him to deal high amounts of damage to groups of enemies. Balance is crucial to victory." 
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
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Arsonist will convert 50% of total damage received as fire damage over time; Arsonist also has resistance to fire damage from all sources.");
            //LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "<style=cIsUtility>Heat Gauge</style>. Being <style=cIsDamage>ignited</style> increases <style=cIsUtility>movement speed and damage</style>. " + "<style=cStack>Ifrit's Distinction applies these effects permanently</style>." + "<style=cIsUtility>Take half damage from ignition sources</style>. ");

            LanguageAPI.Add(prefix + "PASSIVE_NORMAL_GAUGE_NAME", "Normal Gauge");
            LanguageAPI.Add(prefix + "PASSIVE_NORMAL_GAUGE_DESCRIPTION", $"Heat gauge increase per level as well as with stock-based items. " +
                $"Cooling rate also increases at higher level of heat, starting at {Modules.Config.baseGaugeLowerBoundRecharge.Value}x at 0%, up to a maximum cooling rate of {Modules.Config.baseGaugeUpperBoundRecharge.Value}x at 100% heat.");

            LanguageAPI.Add(prefix + "PASSIVE_BLUE_GAUGE_NAME", "Supercritical Gauge");
            LanguageAPI.Add(prefix + "PASSIVE_BLUE_GAUGE_DESCRIPTION", $"Heat gauge does not increase, instead supercritical gauge increases per level as well as with stock-based items. " +
                $"Gain {Modules.StaticValues.blueDamageMultiplier}x damage while heat is in the blue portion of the gauge.");

            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_FIRESPRAY_NAME", "Fireball");
            LanguageAPI.Add(prefix + "PRIMARY_FIRESPRAY_DESCRIPTION", Helpers.heatPrefix + $"Fire a ball of fire that deals <style=cIsDamage>{100f * StaticValues.firesprayStrongDamageCoefficient}% damage and ignites</style> enemies on hit. ");
            LanguageAPI.Add(prefix + "ALT_PRIMARY_FIRESPRAY_NAME", "Overdrive");
            LanguageAPI.Add(prefix + "ALT_PRIMARY_FIRESPRAY_DESCRIPTION", Helpers.heatPrefix + Helpers.criticalPrefix + $"Fire a ball of fire that deals <style=cIsDamage>{100f * StaticValues.altFiresprayStrongDamageCoefficient}% damage and ignites</style> enemies on hit. ");
            LanguageAPI.Add(prefix + "PRIMARY_FLAMETHROWER_NAME", "Dragon's Breath");
            LanguageAPI.Add(prefix + "PRIMARY_FLAMETHROWER_DESCRIPTION", $"Fire a constant beam of fire that deals <style=cIsDamage>{100f * StaticValues.flamethrowerStrongDamageCoefficient}%</style> damage and has an increased chance to burn enemies the closer you are to your target.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_FLAREGUN_NAME", "Signal Flare");
            LanguageAPI.Add(prefix + "SECONDARY_FLAREGUN_DESCRIPTION", Helpers.heatPrefix + $"Fire a long range signal flare that deals <style=cIsDamage>{100f * StaticValues.flareStrongDamageCoefficient}% damage</style> over 5 seconds, then exploding for <style=cIsDamage>{100f * StaticValues.flareStrongDamageCoefficient}% damage</style>" +
                $", launching {Modules.Config.flareSalvoAmount.Value} flare(s) that deal <style=cIsDamage>{100f * StaticValues.flareStrongChildDamageCoefficient}% damage</style>.");
            LanguageAPI.Add(prefix + "SECONDARY_PUNCH_NAME", "Zero Point Blast");
            LanguageAPI.Add(prefix + "SECONDARY_PUNCH_DESCRIPTION", Helpers.startPrefix + $"Propel yourself forwards, colliding into the first enemy and dealing <style=cIsDamage>{100f * StaticValues.zeropointpounchDamageCoefficient}% damage</style> in a blast around you. " +
                $" On propulsion, blast enemies behind you for <style=cIsDamage>{100f * StaticValues.zeropointpounchDamageCoefficient}% damage</style>. ");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_CLEANSE_NAME", "Cleanse");
            LanguageAPI.Add(prefix + "UTILITY_CLEANSE_DESCRIPTION", Helpers.startPrefix + $"<style=cIsDamage>Ignite</style> the area around you, dealing <style=cIsDamage>{100f * StaticValues.cleanseDamageCoefficient}%</style> to enemies around you and burning away other status effects. Grants a small boost of speed. ");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_MASOCHIST_NAME", "Masochism");
            LanguageAPI.Add(prefix + "SPECIAL_MASOCHIST_DESCRIPTION", Helpers.startPrefix + $"Damage from being <style=cIsDamage>Ignited</style> turns into <style=cIsHealing>healing</style> for {StaticValues.masochismBuffDuration} seconds. Increases attack speed <style=cIsDamage>{100*StaticValues.igniteAttackSpeedMultiplier}%</style>.");

            LanguageAPI.Add(prefix + "SPECIAL_MASOCHISM_NAME", "Masochism");
            LanguageAPI.Add(prefix + "SPECIAL_MASOCHISM_DESCRIPTION", $"Once enough stacks are accumulated, activate to deal <style=cIsDamage>{100f * Modules.StaticValues.masochismPulseCoefficient}% damage</style>" +
                $" per tick around you while your overheat gauge increases over time. Each stack is equivalent to 1 second of uptime. This state is forcibly exited when the amount of seconds elapsed is greater " +
                $"than the number of stacks accumulated, or if the gauge accumulates too much heat. Exiting the state creates a massive explosion that deals <style=cIsDamage>{100f * Modules.StaticValues.masochismFinalBlastCoefficient}% damage</style>, further multiplied by the number of stacks accumulated.");
            #endregion

            #region Keywords
            LanguageAPI.Add(prefix + "KEYWORD_PASSIVE", $"<style=cKeywordName>Heat</style> " + 
                $"Skills increase/decrease heat. At max heat, arsonist overheats, weakening their skills and causing longer cooldowns. Attack speed increases cooling rate. The selected primary chosen alters the heat gauge. ");
            LanguageAPI.Add(prefix + "KEYWORD_BASEGAUGE", $"<style=cKeywordName>Base Gauge</style> " + 
                $"Heat gauge increase per level as well as with stock-based items. " +
                $"Cooling rate also increases at higher %s of heat.");
            LanguageAPI.Add(prefix + "KEYWORD_CRITICALGAUGE", $"<style=cKeywordName>Supercritical Gauge</style> " + 
                $"Heat gauge does not increase, instead supercritical gauge increases per level as well as with stock-based items. " +
                $"Gain {Modules.StaticValues.blueDamageMultiplier}x damage while heat is in the blue portion of the gauge. ");
            LanguageAPI.Add(prefix + "KEYWORD_FIRESPRAYHEAT", $"<style=cKeywordName>Heat</style> " +
                $"Costs <style=cIsDamage>{Modules.StaticValues.firesprayEnergyCost} heat</style>. Reduced damage and speed in overheat.");
            LanguageAPI.Add(prefix + "KEYWORD_FLAREHEAT", $"<style=cKeywordName>Heat</style> " + 
                $"Reduces Heat by <style=cIsDamage>{Modules.StaticValues.flareHeatReductionMultiplier}x</style> of current heat accumulated. Half damage in overheat.");
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