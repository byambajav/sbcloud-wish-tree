# Wish Tree

It's Christmas. Let's bring Santa Claus to our home and make the kids' wishes come true!

## Basic usage
- Parents: setup contact information
- Kid approaches the setup and make a wish (oral) for X'mas
- The wish get sent to the parents, along with suggestions on what to buy to fulfill kid's wish

## Technology applied
- 3D hologram display technique: Pepper's Ghost (https://en.wikipedia.org/wiki/Pepper%27s_ghost)
- 3D game using Unity
- Human motion detector using Arduino
- Chatbot using Facebook API
- Speech recognition & Synthesis
- 3D design & fabrication with laser cutter
- and more... (update later)

## Dependencies and resources
- Unity assets
  * FREE Christmas Assets / Low Poly: https://www.assetstore.unity3d.com/en/#!/content/13102
  * 3D Games Effects Pack Free: https://www.assetstore.unity3d.com/en/#!/content/42285
  * Hail Particles Pack: https://www.assetstore.unity3d.com/en/#!/content/62038
- Others (update later)

## Build instructions:
- Clone/download source code
- Open it in Unity 5.5 or higher
- Go to File -> Build Settings...
- Choose correct platform and architecture
- Hit Build and Run
- Select output path

## Human Detection - Arduino
- Sensor: 焦電型赤外線（人感）センサーモジュール SB412A (http://akizukidenshi.com/catalog/g/gM-09002/)
- Upload human_detect.ino to Arduino UNO R3
- Connect SB412A as mention in datasheet: From SB412A to Arduino:
	- Pin 1 to GND
	- Pin 2 to A0
	- Pin 3 to 5V

## Backend WebApp
- Main language
  - Python 3.5.2
- Server
  - Aliyun + `aliyuncli`
  - [Flask 0.11.1](http://flask.pocoo.org/)
  - [nginx](https://www.nginx.com/)
  - [uWSGI](https://uwsgi-docs.readthedocs.io/en/latest/)
- Text-to-speech
  - [gTTS](https://github.com/pndurette/gTTS)
  - [ffmpeg](https://www.ffmpeg.org/)


## Future works
- Show gifts for parents using AliExpress & Amazon using the respective affiliate APIs.
- Make the hardware setup more lightweight and mass-producable.
