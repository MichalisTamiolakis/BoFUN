# BoFUN
Board games made FUN. A collection of all the classic games in one, designed specifically for the smart living room of HCI Lab.

# Setup Process
1) Run fullstack template with docker
2) For the augmentable go to AugmentableUnity > bin > BoFUN_Data > StreamingAssets . There you can find all the user available settings. Specifically in Settings folder there are two JSON files. The GameSettings.json File contains information about the min and max players, min and max time per round, the option to enable/disable narrator as well as some animation options. The NetworkSettings.json file is the one you will most probably need to change. The information for the backend URLs and the socket server is stored. Change localhost to the backend's actual URL. The other folders appart from Settings have information about the Grammar used for voice recognition(SRGS folder) and the audio that is playing in the background (Sounds folder). Feel free to change the sounds and SRGS grammar as you like, but keep the file names the same.
3) Run BoFUN.exe found in the bin folder in the table
4) In surroundwall navigate to http://ipaddress:4200/surroundwall
5) The game should work fine now on, have fun!