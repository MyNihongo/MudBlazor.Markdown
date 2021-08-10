# Enderal - My Way

- [Enderal - My Way](#enderal---my-way)
- [Preamble](#preamble)
- [Installation](#installation)
  - [Pre-Installation](#pre-installation)
    - [Installing Microsoft Visual C++ Redistributable Package](#installing-microsoft-visual-c-redistributable-package)
    - [Steam Config](#steam-config)
      - [Disable the Steam Overlay](#disable-the-steam-overlay)
      - [Change Steams Update Behavior](#change-steams-update-behavior)
      - [Set the Game language to English](#set-the-game-language-to-english)
    - [Clean Skyrim](#clean-skyrim)
    - [Start Skyrim](#start-skyrim)
  - [Using Wabbajack](#using-wabbajack)
    - [Preparations](#preparations)
    - [Downloading and Installing](#downloading-and-installing)
      - [Problems with Wabbajack](#problems-with-wabbajack)
  - [Post-Installation](#post-installation)
    - [Copy Game Folder Files](#copy-game-folder-files)
    - [Launch EMW](#launch-emw)
  - [In-Game MCM Options](#in-game-mcm-options)
  - [Tweaking Performance](#tweaking-performance)
    - [ENB: PiCHO for NLA + Velvet Reshade](#enb-picho-for-nla--velvet-reshade)
    - [Tweaking the ENB](#tweaking-the-enb)
    - [Tweaking the Game Settings](#tweaking-the-game-settings)
- [Updating](#updating)
  - [Removing the Modlist](#removing-the-modlist)
- [Noteworthy Mods](#noteworthy-mods)
  - [Core Gameplay Overhaul](#core-gameplay-overhaul)
  - [APPAREL & WEAPONS](#apparel--weapons)
  - [Content Addon](#content-addon)
  - [Creatures](#creatures)
  - [Interface](#interface)
  - [Miscellaneous](#miscellaneous)
  - [NPC Overhauls](#npc-overhauls)
  - [Skeleton & Animations](#skeleton--animations)
  - [Sound Effects](#sound-effects)
- [FAQ](#faq)
- [Credits and Thanks](#credits-and-thanks)
- [Contact](#contact)
- [Contributing](#contributing)
- [Changelog](#changelog)

# Preamble

![emw-banner](extra/emw.png)
![build-status](https://img.shields.io/endpoint?label=Status&style=for-the-badge&url=https%3A%2F%2Fbuild.wabbajack.org%2Flists%2Fstatus%2Femw%2Fbadge.json)

Enderal - My Way is a balanced but hardcore take on the Enderal game experience with upgraded environments, improved/replaced equippable appearances and better looking NPCs.

Various bug fixes have been made and quality of life features have been included.

The mechanics of the game have been completely overhauled by Enderal Gameplay Overhaul with further balancing done specifically for this list.

# Installation

## Pre-Installation

These steps are only needed if you install this Modlist for the first time. If you update the Modlist, jump straight to [Updating](#updating).

### Installing Microsoft Visual C++ Redistributable Package

I doubt you need to do this since you likely already have this installed. The package is required for MO2 and you can download it from [Microsoft](https://support.microsoft.com/en-us/help/2977003/the-latest-supported-visual-c-downloads). Download the x64 version under "Visual Studio 2015, 2017 and 2019". [Direct link](https://aka.ms/vs/16/release/vc_redist.x64.exe) if you can't find it.

### Steam Config
#### Disable the Steam Overlay

The Steam Overlay can cause issues with ENB and is recommended to be turned off.

Open the Properties window (right click the game in your Library->Properties), navigate to the _General_ tab and un-tick the _Enable the Steam Overlay while in-game_ checkbox.

#### Change Steams Update Behavior

SSE is still being updated by Bethesda (they only add Creation Club content). Whenever the game updates, the entire modding community goes silent for the next one or two weeks because some mods need to be updated to the latest game runtime version.

To ensure that Steam does not automatically updates the game for you, head over to the Properties window, navigate to the _Updates_ tab and change _Automatic updates_ to _Only update this game when I launch it_. You should also disable the Steam Cloud while you're at it.

#### Set the Game language to English

Just do it. This entire Modlist is in English and 99% of all mods you will find are also in English. I highly recommend playing the game in English and **I will not give support to people with a non-English game**.

Open the Steam Properties window, navigate to the _Language_ tab and select _English_ from the dropdown menu.

### Clean Skyrim

I highly recommend uninstalling the game through Steam, deleting the game folder and reinstalling it. You should also clean up the `Skyrim Special Edition` folder in `Documents/My Games/`.

### Start Skyrim

After you have done everything above and got a clean SSE installation ready, start the Launcher and open the _Options_ menu.

1. Click on _High_
2. Set the _Aspect Ratio_ and _Resolution_ to your monitor's native values
3. Set _Antialiasing_ to _Off_
4. Check _Windowed Mode_ and _Borderless_

Start the game and exit once you're in the main menu.

## Using Wabbajack

### Preparations

Grab the latest release of Wabbajack from [here](https://github.com/wabbajack-tools/wabbajack/releases) and place the `Wabbajack.exe` file in a blank folder on the root of your drive such as C:/Wabbajack. This is your _working folder_ when the term is used again.

Let's get to the actual installation. Grab the latest release of `Enderal - My Way` by running Wabbajack and selecting Browse Modlists.

If Downloading From Browse Modlists
After clicking Browse Modlists, scroll until you find `Enderal - My Way` and click the download modlist button, an arrow pointing down with a horizontal arrow. After downloading it will take you to a new screen to select Download Location and Installation Location.


### Downloading and Installing

The download and installation process can take a very long time depending on your system specs. Wabbajack will calculate the amount of threads it will use at the start of the installation. To have the highest amount of threads and thus the fastest speed, it is advised to have the working folder on an SSD.

1. Open Wabbajack
1. Load the Modlist by clicking the download button on the Browse Modlists page inside the Wabbajack app. 
1. Adjust the download and installation paths to where you wish to have the list installed and for its downloads to be stored. A single download location can be used for multiple lists if you wish to do so.
1. Click the Go/Begin button
1. Wait for Wabbajack to finish

The Installation Location must NOT be your Skyrim folder. Your Download Location by default will be a folder inside your Installation Location. Using the same Download Location for multiple lists will help prevent downloading mods shared by the lists multiple times but is not necessary.

#### Problems with Wabbajack

There are a lot of different scenarios where Wabbajack will produce an error. I recommend re-running Wabbajack before posting anything. Wabbajack will continue where it left off so you loose no progress.

**Could not download x**:

If a mod updated and the old files got deleted, it is impossible to download them. In this case just wait till I update the Modlist. Some mods are known to have difficulty downloading through Wabbajack for some people and are linked here for convenience.

**x is not a whitelisted download**:

This can happen when I update the modlist. Check if a new update is available and wait if there is none.

**Wabbajack could not find my game folder**:

Wabbajack will not work with a pirated version of the game. If you own the game on Steam, go back to the [Pre-Installation](#pre-installation) step.

## Post-Installation

### Copy Game Folder Files

Locate the directory named Game Folder Files that is inside your Enderal install directory.  Copy the contents of this folder to your Skyrim installation directory which contains Skyrim.exe.

### Launch EMW

1. Open the installation folder and launch the `ModOrganizer.exe` executable
2. If you have the `Enderal: Forgotten Stories (Special Edition)` game in your Steam Library then activate the `Enderal SE - Steam Time Tracking and Achievements` mod by checking it in the left-hand pane of the now open `Mod Organizer 2` (MO2) application.
3. Locate the dropdown box on the top right of MO2.
4. Ensure that `EMW - Push Run To Play` is selected.
5. Press the `Run` button.
   
## In-Game MCM Options

If an MCM page or a setting on an individual page is not listed then it is either already fully pre-configured by changes I have made or it does not need adjustment from it's default state.

- CGO
  - Settings
    - Unlocked Grip
      - Customize your keybind
      - Sweeping 2H Hitboxes [ ]
    -  Leaning
      - Lean Multiplier (1st Person)  0.50
      - Lean Multiplier (3rd Person)  0.50
    -  Camera Noise
      -  Camera Noise Mult (1st Person) 0.50
    -  Dual Wield Blocking
      - Customize your keybind
    - Unlocked Movement
      - Power Attacks (3rd Person)  [ ]

- True Directional Movement
  - Target Lock
    - Controls
      - Customize your hotkey
  - Boss Bar Widget
    - Visibility Settings
      - Show Boss Bar [ ]


## Tweaking Performance

My Setup:

- Ryzen 3700x
- 1080ti
- 32GB DDR4-3200 RAM (CL 14)
- Game and MO2 running on a Samsung 970 EVO Plus M.2 NVME SSD

### ENB: PiCHO for NLA + Velvet Reshade

Enderal SE does not currently have any dedicated ENB offerings but this combo feels rather nice in most situations.  ENB is all personal choice; feel free to customize as you wish.

One thing to keep in mind is that Enderal ***heavily*** relies upon Fade to Black and most ENB presets break this feature.   The one chosen for EMW does not.

### Tweaking the ENB

This should always be the first thing you tweak. Disabling an ENB entirely can give you anything from 5 to >70 FPS depending on what you're using. Open the ENB GUI using `Right Shift + Enter` (`Right Shift` is under the `Enter` key). This interface will allow you to enable and disable certain effects in the left panel.

- `Bloom`: Can give you up to 3 FPS, will make light sources less bright
- `DepthOfField`: Can give you up to 10 FPS.
- `Ambient Occlusion`: This one is a big hitter. You can get up to 20 FPS by disabling this but the effect is very noticeable
- `Distant/DetailedShadow`: Those two can really give you a lot of FPS back depending on your shader settings (game settings). They are very noticeable.
- `ComplexFire/ParticleLights`: You won't see a lot of difference at first when disabling those two, but when particles are on screen (eg using magic or near light sources such as fires), they can _burn_ through your FPS

### Tweaking the Game Settings

I highly recommend using [BethINI](https://www.nexusmods.com/skyrimspecialedition/mods/4875) which is included in this Modlist (can be found in `MO2/tools/BethINI`). I recommend tweaking the `Detail` section for more FPS:

- `Shadow Resolution`: Very big one. A good balance is `2048` which is the borderline between high FPS drainage and garbage looking shadows.
- `Ambient Occlusion`: Highly recommended to leave this at `None`. The ENB this Modlist comes with, uses the ENB SAO.
- `Detailed Draw Distance`: Maybe try `2000` instead of `2800` but you won't notice a lot of FPS gain (maybe 1-3)
- `Remove Shadows`: If you really struggle, use this. This will disable all Shadows (not recommended)

# Updating

If this Modlist receives an update please check the Changelog before doing anything. Always backup your saves or start a new game after updating.

**Wabbajack will delete all files that are not part of the Modlist when updating!**

This means that any additional mods you have installed on top of the Modlist will be deleted. Your downloads folder will not be touched!

Updating is like installing. You only have to make sure that you select the same path and tick the _overwrite existing Modlist_ button.

## Removing the Modlist

You can just remove the MO2 folder and be done with it. SKSE and ENB files will still be in your game folder so I recommend using [ENB and ReShade Manager](https://www.nexusmods.com/skyrimspecialedition/mods/4143) if you want to remove the ENB.

# Noteworthy Mods

Every mod included in `Enderal - My Way` has been updated to work properly with Enderal SE and balanced with EGO in mind.  In some cases the changes are minor and in others they had to nearly be re-built from the ground up.  In either case the various mods listed below are just a mildly interesting selection of the whole.

## Core Gameplay Overhaul

[EGO SE - Enderal SE Gameplay Overhaul](https://www.nexusmods.com/enderal/mods/248) is a complete overhaul of the gameplay mechanics of Enderal. This includes a ton of balance changes, improvements to the ai, new gameplay mechanics, new weapons/spells/potions and much more.

## APPAREL & WEAPONS

[Unique Amulets and Masks](https://www.nexusmods.com/enderal/mods/214) gives unique models to some of the rare amulets such as the Ancient mask and Qalian's Last Smile.

## Content Addon

[Amnesia Shrine](https://www.nexusmods.com/enderal/mods/17) allows the player to rebalance their base stats (health, mana, stamina) or redo their combat and crafting skills (1-handed, archery, light armor, sneak, alchemy, etc) and reset memory point allocation.

[Enderal Apothekarii Monastery](https://www.nexusmods.com/enderal/mods/167) adds the Apothekarii Monastery to the game. In its core a new location / dungeon mod. Unique rewards for exploration included.

[Enderal - Nobles Quarter Player Home Redone](https://www.nexusmods.com/enderal/mods/71) overhauls player home in Nobles Quarter.

[Enderal Outposts](https://www.nexusmods.com/enderal/mods/184) edits some locations into Order Guard Outposts. NPC dialog suggest there should be some.

[Fire of the Mountain](https://www.nexusmods.com/enderal/mods/125)

[House at the Marketplace - Redone](https://www.nexusmods.com/skyrim/mods/86349) remakes interior of the House at the Marketplace.  This mod was edited significantly to support the updated assets in this list as well as to be better balanced for EGO.

[The Bank of Ark](https://www.nexusmods.com/enderal/mods/93) simple mod that makes changes to the safe area of ​​the bank in Ark. New models for hallways, safes and numbers. New lighting and additional detail.

[Yero's Cave Full](https://www.nexusmods.com/enderal/mods/115) changes the side quest dungeon 'Yero's secret cave' so it isn't only partially accessible. Normally this dungeon is full of invisible walls as in the back there are holes in the ground. These issues have been fixed.

## Creatures

[Better Donkey Texture and Model for Enderal](https://www.nexusmods.com/skyrim/mods/102792) gives donkeys in Enderal (and your mighty steed Whirlwind) a new retexture and model upgrade.

[Even Less Skyrim Creatures](https://www.nexusmods.com/enderal/mods/216) replacers for Deer, goats, and deerstalker (sabre cats)

[No more Skyrim-looking Creatures](https://www.nexusmods.com/enderal/mods/212) changes the appearance of some of the creatures so they are unique to Enderal.

[Old Arps](https://www.nexusmods.com/enderal/mods/85) reverts the appearance of Arps to before SureAI turned them into Falmer.

[Pets of Enderal](https://www.nexusmods.com/enderal/mods/77) adds... pets... to Enderal.

[Restored Vatyr Variants](https://www.nexusmods.com/enderal/mods/152) restores the removed Vatyr variants.

## Interface

[Better Container Controls for SkyUI](https://www.nexusmods.com/skyrimspecialedition/mods/25271) adds several new useful features to the container menu.

[Dear Diary - Paper SkyUI Menus Replacer SE](https://www.nexusmods.com/skyrimspecialedition/mods/23010) is the paper interface replacer for almost all SkyUI menus and most UI elemets not covered by SkyUI. 

[Skyrim Souls RE - Updated](https://www.nexusmods.com/skyrimspecialedition/mods/27859) unpauses game menus; configured to have a 50% slowtime effect while menus are open. 

[SmoothCam](https://www.nexusmods.com/skyrimspecialedition/mods/41252) is a highly configurable third-person camera, with smooth frame-interpolation and a raycasting crosshair to help you aim.

## Miscellaneous


## NPC Overhauls

***The Children of Enderal*** is a custom child overhaul for Enderal created mostly by repurposing faces and assets from [Simple Children](https://www.nexusmods.com/skyrimspecialedition/mods/22789) by [tetchystar](https://www.nexusmods.com/skyrimspecialedition/users/8743406)

All NPCs have had their faces regenerated using the same high quality assets used for the player character.  This results in the intended vanilla Enderal look of the NPCs but with a higher quality outcome.

## Skeleton & Animations

Much effort has been spent to improve immersion and the combat experience via better animations.  Only the most impactful changes have been listed here. In some cases major changes had to be made to these mods for them to work for Enderal.

[Dynamic Animation Replacer](https://www.nexusmods.com/skyrimspecialedition/mods/33746) dynamically replaces the actors' animations depending on various conditions.

[Flinching](https://www.nexusmods.com/skyrimspecialedition/mods/42550) is a little mod that allows the player and npcs to flinch when hit

[Hitstop](https://www.nexusmods.com/skyrimspecialedition/mods/42811) gives the player visual feedback when target attacked, making it feel like your weapon really hit something. Just like other ‘hit stop’ mods do, but implement in a different way: It's script-free and fast, which means it can slow down and restore the weapon speed in correct timing.

[Movement Behavior Overhaul](https://www.nexusmods.com/skyrimspecialedition/mods/38950) adds realistic movement like never seen before with enhanced behaviors allowing for proper direction transitions, momentum stop, and much more!

## Sound Effects

[Reverb and Ambiance Overhaul - Enderal](https://www.nexusmods.com/skyrim/mods/82701) makes loud sounds (magic, traps, shouts, big animals) more realistic for player and NPCs. Improves and balances ambiance and reverb to be more realistic and lively. Increases the diversity and dynamics of in-game sound. Fixes numerous issues.

# FAQ

- I'm a 21:9 resolution user and some of my my screens looks weird
  - Search 21x9 in mod organizer and activate all of those mods.
- I'm not a 21:9 resolution user and some of my screens looks weird
  - Search 21x9 in mod organizer and de-activate all of those mods.
- I purchased the house and all kinds of things are floating everywhere?!
  - You need to purchase the furnishings using the thing on the wall by the door.  Once this has been done various tables and such will appear to make everything look as intended.
- Where's my Achievements?!?!!?
  - Please return to the [Launch EMW](#launch-emw) section and read the provided instructions.

# Credits and Thanks

- _YOU_ for actually reading the readme. Thanks a ton!!
- Kratores#4997 for his awesome work optimizing the list for potato rigs!
- Lotus by erri120 - Repository template
- Halgari and everyone the WJ Team - Wabbajack is awesome and so are they

# Contact

While I'm always available on the [Wabbajack Discord](https://discord.gg/wabbajack), I would advise checking the [Issues](https://github.com/jdsmith2816/emw/issues) (open **and** closed ones) on GitHub first if you have any problems. The same goes for _Enhancements_ or _Feature/Mod Requests_. **DO NOT DM ME ON DISCORD. I WILL NOT PROVIDE SUPPORT FOR YOU IN DMS AND I WILL BLOCK YOU**.

# Contributing

See [Contributing](CONTRIBUTING.md).

# Changelog

See [Changelog](CHANGELOG.md).