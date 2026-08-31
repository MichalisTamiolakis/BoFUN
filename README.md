![#BoFUN](https://github.com/MichalisTamiolakis/BoFUN/blob/dev/Fullstack/frontend/src/assets/logo.png?raw=true)

Board games made FUN!

A collection classic casual games in one. 
Designed specifically for the smart living room of HCI Lab for the needs of CS469 course.

# Setup Process

1) Run fullstack template with docker
2) For the augmentable go to AugmentableUnity > bin > BoFUN_Data > StreamingAssets . There you can find all the user available settings. Specifically in Settings folder there are two JSON files. The GameSettings.json File contains information about the min and max players, min and max time per round, the option to enable/disable narrator as well as some animation options. The NetworkSettings.json file is the one you will most probably need to change. The information for the backend URLs and the socket server is stored. Change localhost to the backend's actual URL. The other folders appart from Settings have information about the Grammar used for voice recognition(SRGS folder) and the audio that is playing in the background (Sounds folder). Feel free to change the sounds and SRGS grammar as you like, but keep the file names the same.
3) Run BoFUN.exe found in the bin folder in the table
4) In surroundwall navigate to http://ipaddress:4200/surroundwall
5) The game should work fine now on, have fun!

# Gameplay Images

## AugmenTable

<img width="480" height="270" alt="02_SelectPlayers" src="https://github.com/user-attachments/assets/fc987336-b984-4c18-b48f-05f7350c4868" />
<img width="480" height="270" alt="07_BoardAfterDiceRoll" src="https://github.com/user-attachments/assets/d7d2d4f9-fbf2-438f-9126-7d43f5872136" />
<img width="480" height="270" alt="08_BoardAfterDiceRollInfo" src="https://github.com/user-attachments/assets/4ea88a26-3f44-407a-89db-9bdfd6e37cc1" />
<img width="480" height="270" alt="09_BoardDrawing" src="https://github.com/user-attachments/assets/85329616-bb65-4b52-84f4-0e2f99ecaf9a" />
<img width="480" height="270" alt="10_Question" src="https://github.com/user-attachments/assets/b4260b13-508b-46b2-9c96-097a6673c363" />
<img width="480" height="270" alt="11_Pantomime" src="https://github.com/user-attachments/assets/f2429eb7-9899-4f88-aba8-6e02c9e00388" />

## Smartphone

<img width="187" height="406" alt="iPhone 13 mini - 6" src="https://github.com/user-attachments/assets/3032256d-a889-4d49-a036-57ca9e6b9ef2" />
<img width="187" height="406" alt="iPhone 13 mini - 9" src="https://github.com/user-attachments/assets/6e464862-fc9a-46f8-b51b-cd8af92c9b84" />
<img width="187" height="406" alt="iPhone 13 mini - 11" src="https://github.com/user-attachments/assets/c1773822-ed5c-43aa-9252-56c828e3da7b" />
<img width="187" height="406" alt="iPhone 13 mini - 12" src="https://github.com/user-attachments/assets/86e26635-2f80-4e91-8c54-013b6b6244e3" />

