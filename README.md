# LevelGenerator
Level Generator made with Unity

Generates Minecraft-like world
- uses Perlin noise for randomising
- can be modified to have water or trees
    - if water level is set to have higher number, it will generate islands
- instantiates only visible blocks
- all the blocks are saved into an array if some exact place is needed to find later
    - for example if some block needs to be removed
    - or if some object like tree is generated and the space is not empty anymore
