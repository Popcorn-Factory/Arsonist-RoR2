# Arsonist, The Manic Incendiary

<img src="https://github.com/Popcorn-Factory/Arsonist-RoR2/blob/master/Thunderstorerelease/arsonistpodsmall.gif?raw=true">

The Arsonist is a close-ranged tank who uses fire as a means to an end. Managing his Overheat meter allows him to deal high amounts of damage to groups of enemies. Balance is crucial to victory.

- Your attacks will be weaker when you’re overheating. If you’re in a tight spot, Cleanse can immediately end the overheating period in exchange for extending its cooldown duration.
- Zero-Point Blast’s distance correlates to movement speed.
- Cleanse can be used to negate the fire damage received by Arsonist's passive.
- Dragon's Fury's fire proc chance scales with your distance to the enemy, close the gap to deal more damage.

Network Compatible! (Unless otherwise found not to be)
For any issues or bug reports, contact me on the RoR2 Modding discord (This preferrably, ping me.), or to me directly, also on discord: ethanol10

## Latest Update

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
<img src="https://cdn.discordapp.com/attachments/399901440023330816/1146436804682207242/image.png?width=1280&height=720">
<img src="https://cdn.discordapp.com/attachments/399901440023330816/1146442780139344033/image.png?width=1280&height=720">
<img src="https://cdn.discordapp.com/attachments/399901440023330816/1146439985797538034/image.png?width=1280&height=720">
<img src="https://cdn.discordapp.com/attachments/399901440023330816/1146442040800976948/image.png?width=1280&height=720">

<details>
<summary>v1.0 Screenshots</summary>
<img src="https://media.discordapp.net/attachments/928130606662049892/1086175951932641340/image.png?width=1280&height=720">
<img src="https://media.discordapp.net/attachments/928130606662049892/1086175952809246760/image.png?width=1280&height=720">
<img src="https://media.discordapp.net/attachments/928130606662049892/1086175953551630357/image.png?width=1280&height=720">
</details>

## Support me on Ko-fi! 
There's no need for payment for mods, but a coffee would be nice once in awhile!

<a href="https://ko-fi.com/popcornfactory" target="_blank">
  <img width="400" src="https://cdn.discordapp.com/attachments/928130606662049892/952521134526590996/unknown.png"/>
</a>

## Mod Interoperability List:
- EmoteAPI / CustomEmotesAPI
- RiskOfOptions

## Skills
<img src="https://github.com/Popcorn-Factory/Arsonist-RoR2/blob/v2.0/Thunderstorerelease/arsonistsheet2.png?raw=true">
<details>
<summary>v1.0 Skills</sumarry>
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
    <a href="https://thunderstore.io/package/PopcornFactory/ShigarakiMod/">
        <img src="https://user-images.githubusercontent.com/93917577/168004591-39480a52-c7fe-4962-997f-cd9460bb4d4a.png"><br>
        <p>ShigarakiMod (TeaL)</p>
    </a>
</div>
<div>
    <a href="https://thunderstore.io/package/TeaL/DekuMod/">
        <img src="https://cdn.discordapp.com/attachments/399901440023330816/960043614036168784/TeaL-DekuMod-3.1.1.png.128x128_q95.png"><br>
        <p>DekuMod (TeaL)</p>
    </a>
</div>
<div>
    <a href="https://thunderstore.io/package/Ethanol10/Ganondorf_Mod/">
        <img src="https://cdn.discordapp.com/attachments/399901440023330816/960043613428011079/Ethanol10-Ganondorf_Mod-2.1.5.png.128x128_q95.png"><br>
        <p>Ganondorf Mod (Ethanol 10)</p>
    </a>
</div>
<div>
    <a href="https://thunderstore.io/package/BokChoyWithSoy/Phoenix_Wright_Mod/">
        <img src="https://cdn.discordapp.com/attachments/399901440023330816/960054458790850570/BokChoyWithSoy-Phoenix_Wright_Mod-1.6.2.png.128x128_q95.png"><br>
        <p>Phoenix Wright Mod (BokChoyWithSoy)</p>
    </a>
</div>
<div>
    <a href="https://thunderstore.io/package/PopcornFactory/Wisp_WarframeSurvivorMod/">
        <img src="https://cdn.discordapp.com/attachments/399901440023330816/960043613692239942/PopcornFactory-Wisp_WarframeSurvivorMod-1.0.2.png.128x128_q95.png"><br>
        <p>Wisp Mod (Popcorn Factory Team)</p>
    </a>
</div>
<div>
    <a href="https://thunderstore.io/package/PopcornFactory/Rimuru_Tempest_Mod/">
        <img src="https://cdn.discordapp.com/attachments/399901440023330816/1086161045283950602/PopcornFactory-Rimuru_Tempest_Mod-0.png"><br>
        <p>Rimuru Tempest Mod (Popcorn Factory Team)</p>
    </a>
</div>
<div>
    <a href="https://thunderstore.io/package/BokChoyWithSoy/Bok_Choy_Items/">
        <img src="https://cdn.discordapp.com/attachments/399901440023330816/1086162390783111198/BokChoyWithSoy-Bok_Choy_Items-1.png"><br>
        <p>Bok Choy Items (BokChoyWithSoy)</p>
    </a>
</div>

</details>

# Old Changelog
<details>
<summary>Click to expand previous patch notes:</summary>

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