<p align="center">
  <img src="https://github.com/Kade-github/BombRushRadio/assets/26305836/0ffccadb-8004-437f-8543-5040c219fff2">
</p>


# BombRushRadio
A Bomb Rush Cyberfunk mod that lets you add custom music into the game!

## How to use

Launch the game once, then navigate to your games root directory, go into **"Bomb Rush Cyberfunk_Data/StreamingAssets/Mods/BombRushRadio/Songs/"** which the mod should have created, then drag your songs into that folder. Make sure it's a supported format.

### Supported Formats
- AIF(F)
- IT
- MOD
- MP2
- MP3
- OGG
- S3M
- WAV
- XM
- FLAC

The format is pretty simple, 

## if the format is not this (it will not work):
# "**Artist-SongTitle**"


It should look like this

![image](https://github.com/Kade-github/BombRushRadio/assets/26305836/c30022e8-703f-4918-9a46-b70a65019be6)


(You can also use folders, like this)

![image](https://github.com/Kade-github/BombRushRadio/assets/26305836/dc977b6b-2e49-461f-94a2-e1a2955041b8)

![image](https://github.com/Kade-github/BombRushRadio/assets/26305836/108a13ba-ce65-4b65-81cb-fb03a7b003ef)

# Config

```
## Settings file was created by plugin Bomb Rush Radio! v1.3.1.0
## Plugin GUID: kade.bombrushradio

[Audio]

## Caches audios to disc (Pros: Memory is lowered significantly, Any startup load time after the first start is lowered significantly, Cons: Stutters on play (depending on audio size), Caching on disc can be expensive on storage (depending on audio size/format))
# Setting type: Boolean
# Default value: false
Caching = false


```

## Installation

### [THUNDERSTORE (CAN BE USED ON R2MODMAN)](https://thunderstore.io/c/bomb-rush-cyberfunk/p/Kade/BombRushRadio/)

Go to the [latest release](https://github.com/Kade-github/BombRushRadio/releases/latest), and download either:

### BepInEx Included

A version of the mod with BepInEx included.

To install:

**Drag'N'Drop the contents of the zip inside of your games root directory**

### Standalone

You must have BepInEx already installed (make sure its 5.4!!)

To install:

**Put the .dll in your BepInEx/Plugins/ folder**

It should look like this at the very end

![image](https://github.com/Kade-github/BombRushRadio/assets/26305836/46ca5d9f-d041-44ee-9ffb-a969f357fa00)

# STEAM DECK USERS

For it to work, you must use this launch property in steam: `WINEDLLOVERRIDES="winhttp=n,b" %command%`



## Reloading

Want to reload songs on the fly? Make some changes in the song folder and press **F1** in game, it'll load any deletions/additions you make! (Not changes to files though)

## Building

Create a Publicized Assembly of the game, through anything (I used BepInEx's one, oh and I can't provide this because its a paid game)

And then open the .sln, and add in most of the missing references.

Then just build it.
