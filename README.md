# AR Tower Defence
**AR Tower Defence** is an augmented-reality mobile game made with Unity to deliver a sample experience where AR is a core mechanic, not an optional feature.

I developed this app as the final project of the Augmented Reality class of the University of Milan.

Software adopted:
- Unity 2018.4.0f1 (game development)
- Blender 2.82 (object modeling, external renderer)
- GIMP 2.10 (sprite/texture editing)
- Audacity 2.3.2 (audio clip editing)

[Download](https://github.com/liggiorgio/ar-tower-defence/releases/download/v1.0.1/ARTD.apk) the game for free (requires Android 7.0+ with AR Core support).

[![Youtube trailer](https://img.youtube.com/vi/IYuTAqtbqIk/0.jpg)](https://youtu.be/IYuTAqtbqIk)

![Screenshot](https://raw.githubusercontent.com/liggiorgio/ar-tower-defence/master/Screenshots/Screenshot.jpg)

## Gameplay overview
Players start by building a *Command Post*, which must defend from waves of incoming enemies, and then deploy turrets that attack enemies at sight. There's a 30-second intermission before each next wave, during which players can deploy more turrets and repair/upgrade existing ones, using currency earned while playing.

There's a total of 25 waves. Every 5th wave is a Boss Wave, and every 3rd consecutive wave is a Bonus Wave. Boss Waves include a randomised Boss amongst other enemies, Bonus Waves reward players if they accomplish the bonus objective.

## Main features
### Progression system
Every feat in the game awards experience points and this let players progress through 40 different ranks. Spending money during the game makes unit types progress as well, unlocking new upgrade levels.

### In-game currency
Players can earn and spend credits only while playing, since the game doesn't include IAP (In-App-Purchases), and money resets every time a new game starts.

### Rewards
Players can earn medals while playing if they meet the right conditions. There's a total of 9 unique medals.

### Game stats
The players' profile includes a detailed report of all feats performed while playing, such as total game time, progression percentage, number of enemies killed.

### Game options
Players can set up several options before starting to play, such as the starting wave, the game difficulty, and up to five game mutators. Such options alter enemies' behaviour as well as credits earned and points scored.

### Achievements and unlockable items
Some game options are not available from the beginning and must be unlocked. The game also includes eight achievements that players earn during game progression.

## Assets
While I did almost all the programming, I give credits to the many artists and sound makers whose open resources I used in my software, as well as other resources. Feel free to check their artwork. All their work was used under the CC 3.0 or 4.0 License, unless it was in the CC0 public domain. None of the following creators endorses this project. Only minor edits were made to some of this assets whenever needed.

### Audio
- Menu music: [**Epic Fall**](https://opengameart.org/content/epic-fall) by [*Alexandr Zhelanov*](https://soundcloud.com/alexandr-zhelanov)
- Menu button click: [**UI SFX Pack 2 Sample**](https://opengameart.org/content/ui-sfx-pack-2-sample) by *David McKee (VIRiX Dreamcore)*
- Next wave: [**TOMS EFFECT 01**](https://freesound.org/people/sandyrb/sounds/35648/) by *sandyrb*
- Notification: [**Deep Whoosh #2**](https://freesound.org/people/Kinoton/sounds/351259/) by *Kinoton*
- Countdown end: [**Hard Cinematic Hit**](https://freesound.org/people/Vendarro/sounds/328448/) by *Vendarro*
- Wave start: [**woosh**](https://freesound.org/people/farbe1001/sounds/400358/) by *farbe1001*
- Boss wave start: [**Epic Horn Hit 01**](https://freesound.org/people/Magmi.Soundtracks/sounds/432243/) by *Magmi.Soundtracks*
- Countdown ticks: [**Kill Switch (Large Breaker Switch)**](https://freesound.org/people/ModulationStation/sounds/131599/) by *ModulationStation*
- Notification 2: [**AVA - Instinct - Whoosh Bang - Stampede**](https://freesound.org/people/AVA_MUSIC_GROUP/sounds/397147/) by *AVA_MUSIC_GROUP*
- Notification 3: [**Cash Register Purchase**](https://freesound.org/people/Zott820/sounds/209578/) by *Zott820*
- Winning music 1: [**Here We Come**](https://freesound.org/people/Link-Boy/sounds/414835/) by *Link-Boy*
- Winning music 2: [**battle-march action loop**](https://freesound.org/people/haydensayshi123/sounds/138681/) by *haydensayshi123*
- Winning music 3: [**Totalitarian obediance**](https://freesound.org/people/jobro/sounds/147811/) by *jobro*
- Losing music 1: [**Jingle Lose 00**](https://freesound.org/people/LittleRobotSoundFactory/sounds/270467/) by *Little Robot Sound Factory*
- Losing music 2: [**Jingle Lose 00**](https://freesound.org/people/LittleRobotSoundFactory/sounds/270529/) by *Little Robot Sound Factory*
- Command Post deploy: [**Electronic Powerup**](https://freesound.org/people/StephenSaldanha/sounds/132560/) by *StephenSaldanha*
- Turret deploy/repair: [**Robotic mechanic step sounds**](https://opengameart.org/content/robotic-mechanic-step-sounds) by [*Lee Barkovich*](http://www.lbarkovich.com)
- Enemy ship smoke trails: [**Thick Fog**](https://opengameart.org/content/thick-fog) by [*LFA*](http://www.lfa.com/)
- Gatling fire: [**Sci-Fi Sound Effects Library**](https://opengameart.org/content/sci-fi-sound-effects-library) by *Little Robot Sound Factory*
- Gauss fire: [**shipboard railgun**](https://freesound.org/people/deleted_user_1941307/sounds/155790/) by *deleted_user_1941307*
- Laser fire: [**Laser pistol/gun**](https://freesound.org/people/steshystesh/sounds/336501/) by *steshystesh*
- Enemy fire: [**02156 laser shot**](https://freesound.org/people/Robinhood76/sounds/107613/) by *Robinhood76*
- Enemy shield gained: [**short deep humming**](https://freesound.org/people/DrMaysta/sounds/349704/) by *DrMaysta*
- Enemy shield lost: [**Deflector Shield**](https://freesound.org/people/Metzik/sounds/459782/) by *Metzik*
- Orbital strike charge: [**SciFi Gun - Mega Charge Cannon**](https://freesound.org/people/dpren/sounds/440147/) by *dpren*
- Orbital strike hit: [**HQ Explosion**](https://freesound.org/people/Quaker540/sounds/245372/) by *Quaker540*
- EMP charge: [**Big Sci-Fi Explosion/Bomb (Close)**](https://freesound.org/people/EFlexMusic/sounds/393374/) by *EFlexMusic*
- Orbital strike, ready: [**Computer Chirp 2**](https://freesound.org/people/pointparkcinema/sounds/407237/) by *pointparkcinema*
- Orbital strike, engaging: [**Click Electronic 02**](https://freesound.org/people/LittleRobotSoundFactory/sounds/288950/) by *Little Robot Sound Factory*
- Orbital strike, target lost: [**beeps**](https://freesound.org/people/atari66/sounds/64119/) by *atari66*
- Orbital strike, no target: [**b1 dree**](https://freesound.org/people/JarAxe/sounds/172691/) by *JarAxe*
- Orbital strike, engaged: [**Robotic Sound FX**](https://freesound.org/people/Ekuhvielle/sounds/211071/) by *Ekuhvielle*
- Score count: [**SCORE COUNT**](https://freesound.org/people/xtrgamr/sounds/253546/) by *xtrgamr*
- Turret upgrade: [**UI, Mechanical, Notification, 01, FX**](https://freesound.org/people/InspectorJ/sounds/458586/) by *InspectorJ*

### 2D Graphics
- Main UI: [**Space Shooter Game User Interface**](https://opengameart.org/content/space-shooter-game-user-interface) by *CraftPix.net*
- Medals: [**Medals**](https://opengameart.org/content/medals-2) by *Kenney.nl*
- Player rank icons: [**Ranks pack (70×)**](https://opengameart.org/content/ranks-pack-70%C3%97) by *Kenney.nl*
- Commendation icons: [**95 game icons**](https://opengameart.org/content/95-game-icons) by *sbed*
- Render skyboxes: [**Ulukai's space skyboxes**](https://opengameart.org/content/ulukais-space-skyboxes) by *Calinou*
- Render terrain: [**Seamless Space Rocks Textures pack (512px) - Mine Rocks CH16**](https://opengameart.org/content/seamless-space-rocks-textures-pack-512px-mine-rocks-ch16png) by *mafon2*
- Energy Shield / EMP waves: [**Seamless Space Rocks Textures pack (512px) - Lava Planet CH16**](https://opengameart.org/content/seamless-space-rocks-textures-pack-512px-lava-planet-ch16png) by *mafon2*
- Mutator icons: [**Simple shooter icons**](https://opengameart.org/content/simple-shooter-icons) by *qubodup*
- Sparks: [**Spark particle set of 8**](https://opengameart.org/content/spark-particles-set-of-8) by *Keith333*
- Fire flames: [**Flame Particle set - 4 in total**](https://opengameart.org/content/flame-particle-set-4-in-total) by *Keith333*
- Black smoke: [**Smoke particle assets**](https://opengameart.org/content/smoke-particle-assets) by *Kenney.nl*
- Explosion: [**explosion**](https://opengameart.org/content/explosion) by *Cuzco*
- Scorch marks: [**Scorch Marks**](https://dlpng.com/png/6922942) on [*DLPNG*](http://dlpng.com)
- Bullets: [**Golgotha Effects Textures: fireball-side**](https://opengameart.org/content/golgotha-effects-textures-fireball-sidejpg) by *Crack.com*
- Flare/shine effect: [**Golgotha Effects Textures: blue_flare**](https://opengameart.org/content/golgotha-effects-textures-blueflarejpg) by *Crack.com*
- Orbital strike beam: [**Laser effect Sheet**](https://opengameart.org/content/laser-effect-sheet) by *netcake3*
- Laurel outline in commendations: [**Laurels**](https://opengameart.org/content/laurels) by *Blarumyrran*
- EMP icon/button animation: [**FX charge**](https://opengameart.org/content/fx-charge) by *VSG*

### 3D Models
- Command Post: [**Reactor**](https://opengameart.org/content/reactor) by *nazzyc*
- Turrets: [**Sci-fi turret**](https://opengameart.org/content/sci-fi-turret) by *Irondust*
- Turret muzzle: [**Muzzle flash (with model)**](https://opengameart.org/content/muzzle-flash-with-model) by *Julius*
- Enemy spaceships: [**5 space ships**](https://opengameart.org/content/5-space-ships) by *Unnamed*

### Other
- Outlined shader for turrets: [**Outlined Diffuse Shader Fixed for Unity 5.6**](https://github.com/Shrimpey/Outlined-Diffuse-Shader-Fixed) (Custom Outline Constant Width) by *Luke Kabat*
- Enemy steering behaviours: [**Unity Movement AI**](https://github.com/antonpantev/unity-movement-ai) by *Anton Pantev*
- Texture scrolling code snippet: [**Animating Tiled texture**](https://wiki.unity3d.com/index.php/Animating_Tiled_texture), from Unify Community Wiki, by *Joachim Ante*

