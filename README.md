# Arsonist, The Manic Incendiary

<img src="https://github.com/Popcorn-Factory/Arsonist-RoR2/blob/master/Thunderstorerelease/arsonistpodsmall.gif?raw=true">

The Arsonist is a close-ranged tank who uses fire as a means to an end. Managing his Overheat meter allows him to deal high amounts of damage to groups of enemies. Balance is crucial to victory.

- Your attacks will be weaker when you’re overheating. If you’re in a tight spot, Cleanse can immediately end the overheating period in exchange for extending its cooldown duration.
- Zero-Point Blast’s distance correlates to movement speed.
- Cleanse can be used to negate the fire damage received by Arsonist's passive.
- Dragon's Fury's fire proc chance scales with your distance to the enemy, close the gap to deal more damage.

Network Compatible! (Unless otherwise found not to be)
For any issues or bug reports, contact me on the RoR2 Modding discord (This preferrably, ping me), or to me directly, also on discord: ethanol10

## Latest Update

- 2.2.2
    - Fixed an issue where Eclipse 5 / Typhoon difficulty didn't trigger a change due to wrong checks.
    - Removed Soft dependencies on SS2 and HIFU's Inferno as it is not required for the unlocks to still work. (You can still unlock firebug with SS2 typhoon.)

- 2.0.0
    - New Content:
        - Two new skins!
            - Mastery - Yuppie
            - Grandmastery - Firebug
        - New Primary! Read below for more info!
        - Achievements for each new item
    - Stat Changes
        - Increased Base damage stat from 8 -> 12
    - Core Skill Changes
        - Passive
            - Arsonist now converts 50% of total damage received as fire damage over time, if a single attack does 30% or more of your total health (Does not apply to Fall Damage unless you are playing with Frailty enabled).
            - Arsonist has resistance to fire damage from all sources
        - Gauge Changes are moved to a passive skill slot
            - All Gauges:
                - Overheat now debuffs attack speed. (50% attack speed debuff)
            - Gauge -> Base Gauge:
                - Cooling rate is affected by the amount of heat in the gauge
                - Config to modify this rate has been added.
            - Gauge -> Supercritical Gauge:
                - Damage done within the gauge before the blue section has a slight damage penalty, dealing 0.9x damage.
                - Damage done within the blue gauge does 3x damage.
        - Primary
            - NEW PRIMARY: Dragon's Breath
                - Fire a constant beam of fire that increases ignite chance the closer you are to an enemy.
                - Attack Speed increases rate of tick.
            - Overheat -> Fireball:
                - Changed name to reflect the skill instead of the gauge type.
                - Changed projectile VFX effect on both normal and overheat projectiles.
                - Changed the SFX to be more impactful.
        - Secondary
            - Flare: Changes
                - Flare now chains another explosion to enemies hit within the first Flare's explosion.
                - After the DoT effect, the afflicted enemies explode sending a number of salvos upwards.
                - Flare reduces heat by 15% of total current heat instead of adding heat when fired.
                - Due to the destructive nature of this upgrade, the cooldown has been increased.
                - Changed the SFX to contain a sizzling and a proper explosion.
                - Should inflict ignite on the final explosion. This is affected by ignition tank.
                - Upgraded VFX 
            - Zero Point Blast
                - Increased the volume of the SFX played on start.
                - Slightly changed the VFX.
        - Utility
            - Cleanse:
                - Does not self burn anymore.
                - Added speed boost on activation
                - Should now apply Ignite correctly. Should be affected by Ignition tank.
                - Added fire effect that emits off Arsonist for the duration of the move.
                - Added VFX for Cleanse Blast when not overheated.
        - Special
            - Masochism
                - Builds up Anticipation stacks while not activated.
                - Now activates a state which radiates heat around Arsonist, dealing ignite damage to enemies in a small radius around you.
                - Increases heat and deals self damage over time during the duration of the move
                - Minimum Heat is raised for the duration of the move.
                - Activatable when required stack amount is reached (Modifiable in Config).
                - Active state ends when maximum heat is reached, or a set period of time is reached (maximum length is the amount of stacks of masochism anticipation).
                - Overheat attack speed debuff is not applied once the overheat state is reached from Masochism.
        - Other:
            - Added an animation for the run cycle, rather than a sped up version of the walk cycle.
            - Added the Lobby animation as an emote. Default key to activate is num 3. You can change the activation in the options.
    - Bug Fixes
        - Fixing Goobo from adding another UI element to the player's screen.
        - Added checks to prevent position count from setting count to a negative value.
        - Changed Zero Point Blast's cancellation priority for Masochism to cancel easier.
        - Added some mitigations on UI to prevent hooks from breaking
        - Added some checks to prevent sounds and VFX from constantly playing even after the game has ended or when the player has died.
    
## Trailer
<div>
    <a href="https://www.youtube.com/watch?v=y8EZUXso7Lc">
        <img src="https://github.com/Popcorn-Factory/Arsonist-RoR2/blob/master/Thunderstorerelease/arsonistthumbnailfirebug.png?raw=true">
    </a>    
</div>

<details>
<summary>v1.0 Trailer</summary>
<div>
    <a href="https://www.youtube.com/watch?v=Aez62FNzMTg">
        <img src="https://github.com/Popcorn-Factory/Arsonist-RoR2/blob/master/Thunderstorerelease/arsonistthumbnail.png?raw=true">
    </a>
</div>
</details>

## Screenshots
<img src="https://raw.githubusercontent.com/Popcorn-Factory/Arsonist-RoR2/master/Thunderstorerelease/screenshot1.png?width=1280&height=720">
<img src="https://raw.githubusercontent.com/Popcorn-Factory/Arsonist-RoR2/master/Thunderstorerelease/screenshot2.png?width=1280&height=720">
<img src="https://raw.githubusercontent.com/Popcorn-Factory/Arsonist-RoR2/master/Thunderstorerelease/sceenshot3.png?width=1280&height=720">
<img src="https://raw.githubusercontent.com/Popcorn-Factory/Arsonist-RoR2/master/Thunderstorerelease/screenshot4.png?width=1280&height=720">

<details>
<summary>v1.0 Screenshots</summary>
<img src="https://raw.githubusercontent.com/Popcorn-Factory/Arsonist-RoR2/master/Thunderstorerelease/screenshotold3.png?width=1280&height=720">
<img src="https://raw.githubusercontent.com/Popcorn-Factory/Arsonist-RoR2/master/Thunderstorerelease/screenshotold1.png?width=1280&height=720">
<img src="https://raw.githubusercontent.com/Popcorn-Factory/Arsonist-RoR2/master/Thunderstorerelease/screenshotold2.png?width=1280&height=720">
</details>

## Support me on Ko-fi! 
There's no need for payment for mods, but a coffee would be nice once in awhile!

<a href="https://ko-fi.com/popcornfactory" target="_blank">
  <img width="400" src="https://raw.githubusercontent.com/Popcorn-Factory/Arsonist-RoR2/master/Thunderstorerelease/kofiImg.png"/>
</a>

## Mod Interoperability List:
- EmoteAPI / CustomEmotesAPI
- RiskOfOptions
- Starstorm2 (Allows unlocking certain unlocks)
- HIFU Inferno (Allows unlocking certain unlocks)

## Skills
<img src="https://github.com/Popcorn-Factory/Arsonist-RoR2/blob/master/Thunderstorerelease/arsonistsheet2.png?raw=true">

<details>
<summary>v1.0 Skills</summary>
<img src="https://github.com/Popcorn-Factory/Arsonist-RoR2/blob/master/Thunderstorerelease/arsonistsheet.png?raw=true">
</details>

## Other Mods by Popcorn Factory
<details>
<summary>Check out other mods from the Popcorn Factory team!</summary>
<div>
    <a href="https://thunderstore.io/package/PopcornFactory/DarthVaderMod/">
      <img width="130" src="https://user-images.githubusercontent.com/93917577/180753359-4906ca0b-6ce5-4ff7-9962-bdec3329682c.png"/>
      <p>Darth Vader Mod</p>
    </a>
</div>
<div>
    <a href="https://thunderstore.io/package/PopcornFactory/DittoMod/">
        <img src="https://user-images.githubusercontent.com/93917577/168004690-23b6d040-5f89-4b62-916b-c40d774bff02.png"><br>
        <p>DittoMod (TeaL)</p>
    </a>
</div>
<div>
    <a href="https://thunderstore.io/package/TeaL/ShigarakiMod/">
        <img src="https://user-images.githubusercontent.com/93917577/168004591-39480a52-c7fe-4962-997f-cd9460bb4d4a.png"><br>
        <p>ShigarakiMod (TeaL)</p>
    </a>
</div>
<div>
    <a href="https://thunderstore.io/package/TeaL/DekuMod/">
        <img src="https://gcdn.thunderstore.io/live/repository/icons/TeaL-DekuMod-4.1.2.png.128x128_q95.png"><br>
        <p>DekuMod (TeaL)</p>
    </a>
</div>
<div>
    <a href="https://thunderstore.io/package/Ethanol10/Ganondorf_Mod/">
        <img src="https://gcdn.thunderstore.io/live/repository/icons/Ethanol10-Ganondorf_Mod-3.1.5.png.128x128_q95.png"><br>
        <p>Ganondorf Mod (Ethanol 10)</p>
    </a>
</div>
<div>
    <a href="https://thunderstore.io/package/BokChoyWithSoy/Phoenix_Wright_Mod/">
        <img src="https://gcdn.thunderstore.io/live/repository/icons/BokChoyWithSoy-Phoenix_Wright_Mod-1.8.0.png.128x128_q95.png"><br>
        <p>Phoenix Wright Mod (BokChoyWithSoy)</p>
    </a>
</div>
<div>
    <a href="https://thunderstore.io/package/PopcornFactory/Wisp_WarframeSurvivorMod/">
        <img src="https://gcdn.thunderstore.io/live/repository/icons/PopcornFactory-Wisp_WarframeSurvivorMod-3.0.6.png.128x128_q95.png"><br>
        <p>Wisp Mod (Popcorn Factory Team)</p>
    </a>
</div>
<div>
    <a href="https://thunderstore.io/package/PopcornFactory/Rimuru_Tempest_Mod/">
        <img src="https://gcdn.thunderstore.io/live/repository/icons/PopcornFactory-Rimuru_Tempest_Mod-1.0.4.png.128x128_q95.png"><br>
        <p>Rimuru Tempest Mod (Popcorn Factory Team)</p>
    </a>
</div>
<div>
    <a href="https://thunderstore.io/package/BokChoyWithSoy/Bok_Choy_Items/">
        <img src="https://gcdn.thunderstore.io/live/repository/icons/BokChoyWithSoy-Bok_Choy_Items-1.0.3.png.128x128_q95.png"><br>
        <p>Bok Choy Items (BokChoyWithSoy)</p>
    </a>
</div>

</details>

# Old Changelog
<details>
<summary>Click to expand previous patch notes:</summary>

- 2.2.0
    - Updated to support SotS.
    - Fixed an issue where Masochism loop plays constantly when running while changing stage.
- 2.1.10
    - Added SFX slider independent of the inbuilt SFX slider in Voice/Volume config.
    - As suggested before, please use RiskOfOptions to dynamically change this value during runtime!
- 2.1.9
    - Lowered Base animation speed on the sprint cycle.
    - Modified the Overview description.
- 2.1.8
    - Raised base volume on Firebug and Arsonist voice lines to a baseline volume.
    - Added sliders for both voice types that can be controlled by the individual sliders or through the master voice volume slider.
- 2.1.7
    - Added Unique voice line for Firebug skin when firing flare.
    - Separated voice lines from SFX
        - The voice volume slider in the settings can now affect Zero Point Blast, Masochism and Idle lines.
    - Voice lines can be controlled by the volume slider, and is now separate to the SFX slider provided in the settings.
- 2.1.6
    - Fixed some applications of burn damage was not applying the proper attacker and therefore not granting money on kill.
        - e.g: Flare DoT was not granting money.
    - Fixed the Passive from not reducing fire damage. Reduces the total damage of the DoT proc to the config value provided (50%)
    - Fixed the Fireball projectile from despawning early, should last 10s at least.
- 2.1.5
    - Changed some descriptions on some moves to be more succinct
    - Added UI element to show how many Anticipations stocks are shown 
        - Up to 10, doesn't show more than 10 but if the config is changed it will still calculate in the background
            - The buff counter at the bottom still counts the stacks.
    - Fixing images on Readme, broken cause I was stupid and used Discord as a CDN lmao
- 2.1.4
    - Purity stacking was reducing the energy cost of abilities to 0, changed to only affect heat changed in masochism controller.
        - More purity = More heat changed without showing the heat changed in the gauge.
    - Null checks on effects before executing any changes.
    - Preparations for other things (not a big update don't get hyped.)
- 2.1.3
    - Added Config option to turn off the Heat Haze effects on Dragon's Fury/Masochism.
    - Attempted to fix the bright orange material on spawn in multiplayer instances.
- 2.1.2
    - Added tokens for Arsonist specific text:
        - Overheating Text
        - Cooling Text
        - Overheating EX Text
    - Dragon's Fury changes
        - Increasing flame chance back from 70% to 90% chance to flame at close range.
        - Energy Cost is now 15 units instead of 20 units per second.
    - Attempting to scale heat gauge with actual stocks instead of specific items.
    - Fixed Cleanse and possibly other issues from propping up not activating the intended effect.
- 2.1.1
    - Fixing the Zero Point Blast from not applying the cooldown correctly on Overheat.
    - Base ZPB Cooldown decreased from 11 seconds -> 9 seconds
        - Similarly the ZPB cooldown in non-overheat is still 60% of the base cooldown (6.6s -> 5.4s)
    - ZPB Damage coefficient 400% -> 200% 
    - ZPB Blast Radius on contact 5 units -> 3 units
    - ZPB Starting speed 600% -> 700% of current speed.
    - Attempted to mitigate Arsonist from bonking against his own fireball. (This may still happen, but should happen less. Particulary an issue when aiming down, but should no longer be too much of an issue. May happen on higher attack speeds.)
    - Fixing Cooldowns from being set incorrectly when overheating and a move isn't used but still applies the longer cooldown.
    - Fixed an issue where pyromania would activate even though a player had Safer Spaces equipped. Safer Spaces should be used first before Pyromania passive is triggered.
    - Dragon's Breath now should consume heat at a constant rate, as it is no longer tied to tick rate or attack speed.
        - As such, now the rate is 20 units of heat a second. (Or so at least that's what I think it's around, I dunno it's goddamn 2am in the morning and I spent so long trying to find a solution GOD PLEASE I JUST NEED SOME SLEEP AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA.)
        - Activating Dragon's Breath disables heat regeneration for 0.2 seconds.
- 2.1.0
    - Added a voiceline that plays by chance (chance is configurable) on Signal Flare. (Only applicable to Arsonist and Yuppie skins, not Firebug.)
    - Added "Better AI". Should make Umbras actually a challenge, and goobos actually do something.
- 2.0.4
    - Supercritical Gauge was not setting damage correctly for clients, this should be fixed now.
- 2.0.3
    - Tweaking Supercritical Gauge:
        - 3x -> 2.5x damage buff in blue portion of the gauge
        - 0.9x -> 0.7x damage debuff in white portion of the gauge
        - Reduced the maximum amount of blue that can be built up from stock items from 90% -> 75% of available heat.
        - Modified the description to indicate the downside.
        - Starting segments for white and blue portions have been changed
            - White: 60% -> 65% of gauge
            - Blue: 30% -> 25% of gauge
    - Tweaking Dragon's Fury:
        - Normal damage coefficient 70% -> 90%
        - Overheating in Dragon's Fury damage coefficient 65% -> 60%
        - Range 25 -> 35 units
        - Overheated Range 16.5 -> 23.1 units
        - Heat Cost 5 -> 6
            - Heat build up is now independant of tick. So this needed to be increased.
        - Reduced Fire Chance, 90% -> 70% chance per tick to inflict ignite when right up in their face. (If hitting all ticks in a given attack, 0.001% -> 0.00729% chance of not burning.
            - Needless to say, it's pretty a damn low chance that you'll not burn the enemy *at least once* after blasting them in the face with this attack.)
        - Increased Flamethrower radius 1 -> 1.25, should be easier to hit enemies with the flamethrower.
        - Turned on smart collision for bullet attacks. Should hit enemies better.
        - Scaling for distance to burn targets has been buffed (TLDR; burning an enemy has a higher chance at a further distance.)
            - Furthest distance (at 50 units away from target) 0% -> 13% chance to burn
            - Closest distance for max burn chance changed from 0 units to 10 units.
- 2.0.2
    - Flamethrower unlock was bugged, Should be fixed now.
        - Was me being stupid. Sorry about that. Feel free to unlock it using Cheat Unlocks.
    - MORE NULL REFERENCE ERROR FIXES
        - Fixed NRE returned from hitting Dragon's Fury on an object with a character body but with no master.
- 2.0.1
    - Fixing some NullReferenceExceptions...
- 2.0.0
    - New Content:
        - Two new skins!
            - Mastery - Yuppie
            - Grandmastery - Firebug
        - New Primary! Read below for more info!
        - Achievements for each new item
    - Stat Changes
        - Increased Base damage stat from 8 -> 12
    - Core Skill Changes
        - Passive
            - Arsonist now converts 50% of total damage received as fire damage over time, if a single attack does 30% or more of your total health (Does not apply to Fall Damage unless you are playing with Frailty enabled).
            - Arsonist has resistance to fire damage from all sources
        - Gauge Changes are moved to a passive skill slot
            - All Gauges:
                - Overheat now debuffs attack speed. (50% attack speed debuff)
            - Gauge -> Base Gauge:
                - Cooling rate is affected by the amount of heat in the gauge
                - Config to modify this rate has been added.
            - Gauge -> Supercritical Gauge:
                - Damage done within the gauge before the blue section has a slight damage penalty, dealing 0.9x damage.
                - Damage done within the blue gauge does 3x damage.
        - Primary
            - NEW PRIMARY: Dragon's Breath
                - Fire a constant beam of fire that increases ignite chance the closer you are to an enemy.
                - Attack Speed increases rate of tick.
            - Overheat -> Fireball:
                - Changed name to reflect the skill instead of the gauge type.
                - Changed projectile VFX effect on both normal and overheat projectiles.
                - Changed the SFX to be more impactful.
        - Secondary
            - Flare: Changes
                - Flare now chains another explosion to enemies hit within the first Flare's explosion.
                - After the DoT effect, the afflicted enemies explode sending a number of salvos upwards.
                - Flare reduces heat by 15% of total current heat instead of adding heat when fired.
                - Due to the destructive nature of this upgrade, the cooldown has been increased.
                - Changed the SFX to contain a sizzling and a proper explosion.
                - Should inflict ignite on the final explosion. This is affected by ignition tank.
                - Upgraded VFX 
            - Zero Point Blast
                - Increased the volume of the SFX played on start.
                - Slightly changed the VFX.
        - Utility
            - Cleanse:
                - Does not self burn anymore.
                - Added speed boost on activation
                - Should now apply Ignite correctly. Should be affected by Ignition tank.
                - Added fire effect that emits off Arsonist for the duration of the move.
                - Added VFX for Cleanse Blast when not overheated.
        - Special
            - Masochism
                - Builds up Anticipation stacks while not activated.
                - Now activates a state which radiates heat around Arsonist, dealing ignite damage to enemies in a small radius around you.
                - Increases heat and deals self damage over time during the duration of the move
                - Minimum Heat is raised for the duration of the move.
                - Activatable when required stack amount is reached (Modifiable in Config).
                - Active state ends when maximum heat is reached, or a set period of time is reached (maximum length is the amount of stacks of masochism anticipation).
                - Overheat attack speed debuff is not applied once the overheat state is reached from Masochism.
        - Other:
            - Added an animation for the run cycle, rather than a sped up version of the walk cycle.
            - Added the Lobby animation as an emote. Default key to activate is num 3. You can change the activation in the options.
    - Bug Fixes
        - Fixing Goobo from adding another UI element to the player's screen.
        - Added checks to prevent position count from setting count to a negative value.
        - Changed Zero Point Blast's cancellation priority for Masochism to cancel easier.
        - Added some mitigations on UI to prevent hooks from breaking
        - Added some checks to prevent sounds and VFX from constantly playing even after the game has ended or when the player has died.
- 1.0.4 -> Small Updates
    - Added Icon to RiskOfOptions
    - Removed references to PhotoMode in code since someone decided to change the bloody mod GUID. UI disables when all UI is disabled.
    - Heat bar UI scales with resolution now
- 1.0.3 -> Bug Fix
    - Added value to `cachedName` property to enable Eclipse progress.
- 1.0.2 -> Forgot to do one last update:
    - Swapped animation for Left and Right strafe
- 1.0.1 -> Changes:
    - New feature:
        - Added a suicide button (Default off, turn on and press 9 in game.)
    - Slight visual/audio changes
        - Heat gauge text now vibrates on overheat.
        - Overheat overlay color changed dependant on Primary selected.
        - Doubled Attenuation range (100 -> 200) on all sounds.
        - Added some Footstep VFX (Still needs work and some sound too) 
    - Bug fixes
        - Changed Flinch animation to Additive instead of Override.
        - Changing Priority on moves so Primary is overrided if Secondary is pressed.
        - Fixing issue where flare tries to apply damage to the already dead body of an enemy causing an NRE.
        - Fixing NRE on Flare shot due to some boilerplate code that adds an effect component when it didn't need one.
    - Balance Changes
        - Health Growth from 15 -> 40 per level
        - Masochism health percentage reduced from 0.05% -> 0.03% of max health per tick.  
        - Masochism buff duration reduced from 8 -> 6.5 seconds
        - Changed Overheat's (Default M1) damage coefficient from 300% -> 350% for non-overheated projectiles.
        - Changed Overdrive's (Alt M1) damage coefficient from 300% -> 200% for non-overheated projectiles.
        - Changed Overdrive's (Alt M1) damage coefficient from 150% -> 80% for overheated projectiles.
        - Changed Overdrive's (Alt M1) base heat cost to fire a projectile from 8 -> 12
- 1.0.0 - Initial Release
 
</details>

## Known Issues
- The UI Gauge is affected by the Guilded Coast water for some reason. Purely cosmetic, only affects how the UI is rendered.
- If someone can tell me how to implement a rope affected by gravity between two fixed objects (gun and the backpack), I'd like some help.
- Odd issues could possibly require a restart or file validation. Please try that and contact me (ethanol10) for bugs with a log file.
- Some options do not do anything (I think), If it doesn't do anything immediately consider reverting that option to avoid unintended effects.
    (One such option is the enableOldLoadout toggle, though I do believe this is disabled.)
 
## Credits
- Concept, Icons, 2D Art, SFX, Voice talent:
  - harmonchaos
  - <a href="https://twitter.com/DragonRoIlZ">DragonRollZ</a>
  - <a href="https://twitter.com/AnOddHermit">An Odd HermitVA</a> -> Arsonist VA
  - <a href="https://twitter.com/Alycoris">Alycoris</a> -> Firebug VA
- 3D Modelling, Rigging, Animation:
  - dotflare
- Programming - Popcorn Factory team (Contact on Discord):
  - ethanol10
  - teal5571
  - bokchoywithsoy
- Miscellaneous
  - Rob - Character template