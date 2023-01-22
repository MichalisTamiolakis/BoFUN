using System;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;
using BoFUN.GameManager;
using System.Text.RegularExpressions;
using Crosstales.RTVoice;

namespace BoFUN.Menu {
    public class SpeechGameCreation : MonoBehaviour
    {
        public string[] commandGrammarAssetStreamingPaths =
        {
            "/SRGS/TeamAndPlayersCommand.xml",
            "/SRGS/DurationCommand.xml"
        };

        public AudioSource audioSource;

        GrammarRecognizer m_Recognizer;

        public void Start()
        {
            VoiceReply("Welcome to Bo FUN. I am Bo, your voice assistant.");
        }


        public void StartListening()
        {
            Speaker.Instance.OnSpeakStarted.AddListener(DisableRecognition);
            Speaker.Instance.OnSpeakCompleted.AddListener(EnableRecognition);

            currentCommand = 0;
            StartCommand();
        }

        public void StopListening()
        {
            Debug.Log("Disabling Bo");
            m_Recognizer?.Stop();
            m_Recognizer?.Dispose();
            m_Recognizer = null;
        }

        private int currentCommand = 0;
        // 0: Ok Bo create a game with 2 Players and 2 Teams
        // 1: 2 Minutes and 10 seconds


        private void GoToNextCommand()
        {
            //Debug.Log("Next Command");
            currentCommand++;

            StartCommand();
        }

        private void GoToPreviousCommand()
        {
            currentCommand--;

            StartCommand();
        }

        private void StartCommand()
        {
            if (currentCommand >= commandGrammarAssetStreamingPaths.Length)
                return;

            // Remove last Grammar
            m_Recognizer?.Stop();
            m_Recognizer?.Dispose();
            m_Recognizer = new GrammarRecognizer(Application.streamingAssetsPath + commandGrammarAssetStreamingPaths[currentCommand], ConfidenceLevel.Low);

            switch (currentCommand)
            {
                case 0:
                    m_Recognizer.OnPhraseRecognized += TeamAndPlayerCommandsRecognized;
                    break;
                case 1:
                    m_Recognizer.OnPhraseRecognized += TimeCommandRecognized;
                    break;
            }

            m_Recognizer.Start();
        }

        // Command Recognized Handlers

        void TeamAndPlayerCommandsRecognized(PhraseRecognizedEventArgs args)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
            builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
            builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
            Debug.Log(builder.ToString());

            // Find number of players and number of teams
            string command = args.text;
            ExtractNumberOfPlayersAndTeamsFromCommands(command, out int numberOfPlayers, out int numberOfTeams);

            if(numberOfTeams>=GameManager.GameManager.Instance.gameSettings.minNumberOfTeams && numberOfTeams <= GameManager.GameManager.Instance.gameSettings.maxNumberOfTeams)
            {
                if(numberOfPlayers >= GameManager.GameManager.Instance.gameSettings.minNumberOfPlayersPerTeam * numberOfTeams)
                {
                    if(numberOfPlayers <= GameManager.GameManager.Instance.gameSettings.maxNumberOfPlayersTotal)
                    {
                        VoiceReply($"I will create a game with {numberOfTeams} teams and {numberOfPlayers} players. Please, tell me the time per round, or if you wish to restart the process say \"Cancel Voice Game\", and start from the beginning.");
                    }
                    else
                    {
                        numberOfPlayers = 8;
                        VoiceReply($"At the time a maximum 8 players are supported. I will create a game with {numberOfTeams} teams and {numberOfPlayers} players. Please, tell me the time per round, or if you wish to restart the process say \"Cancel Voice Game\", and start from the beginning.");
                    }

                    GameManager.GameManager.Instance.gameCreationDescriptor.totalTeams = numberOfTeams;
                    GameManager.GameManager.Instance.gameCreationDescriptor.totalPlayers = numberOfPlayers;
                    MenuScreenManager.Instance.Repaint();
                    MenuScreenManager.Instance.GoToPage(MenuScreenManager.MenuPage.GameSettings);

                    GoToNextCommand();
                }
                else
                {
                    VoiceReply($"The minimum requirement of {GameManager.GameManager.Instance.gameSettings.minNumberOfPlayersPerTeam} players per team is not met for the given values of {numberOfPlayers} players and {numberOfTeams} teams. Repeat the command and try to create a game with at least, {GameManager.GameManager.Instance.gameSettings.minNumberOfPlayersPerTeam * numberOfTeams} players and {numberOfTeams} teams.");
                }
            }
            else
            {
                VoiceReply($"Please repeat the command with the teams beeing between {GameManager.GameManager.Instance.gameSettings.minNumberOfTeams} and {GameManager.GameManager.Instance.gameSettings.maxNumberOfTeams}");
            }


        }

        void TimeCommandRecognized(PhraseRecognizedEventArgs args)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
            builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
            builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
            Debug.Log(builder.ToString());


            string command = args.text;
            if (command == "Cancel Voice Game")
            {
                MenuScreenManager.Instance.GoToPage(MenuScreenManager.MenuPage.NumberOfPlayersAndTeams);
                GoToPreviousCommand();
            }
            else
            {
                ExtractTime(command, out int minutes, out int seconds);

                int minTimeMinutes = GameManager.GameManager.Instance.gameSettings.minRoundDurationSeconds / 60;
                int minTimeSeconds = GameManager.GameManager.Instance.gameSettings.minRoundDurationSeconds - minTimeMinutes * 60;
                int maxTimeMinutes = GameManager.GameManager.Instance.gameSettings.maxRoundDurationSeconds / 60;
                int maxTimeSeconds = GameManager.GameManager.Instance.gameSettings.maxRoundDurationSeconds - maxTimeMinutes * 60;

                int givenDuration = minutes * 60 + seconds;

                // Valid duration
                if (givenDuration<= GameManager.GameManager.Instance.gameSettings.maxRoundDurationSeconds && givenDuration>= GameManager.GameManager.Instance.gameSettings.minRoundDurationSeconds)
                {
                    GameManager.GameManager.Instance.gameCreationDescriptor.duration = givenDuration;
                    MenuScreenManager.Instance.Repaint();
                    VoiceReply($"I have successfully created a new game with {GameManager.GameManager.Instance.gameCreationDescriptor.totalTeams} teams, {GameManager.GameManager.Instance.gameCreationDescriptor.totalPlayers} players and a time per round of, {minutes} minutes and {seconds} seconds.");
                    CreateGameByVoice();
                }
                else
                {
                    VoiceReply($"The given time per round of {minutes} minutes and {seconds} seconds is not valid. The minimum allowed duration is {minTimeMinutes} minutes and {minTimeSeconds} seconds while, the maximum allowed duration is {maxTimeMinutes} minutes and {maxTimeSeconds} seconds. Please repeat the command with a valid time per round.");
                }
            }
        }

        
        // Helper Functions

        static void ExtractNumberOfPlayersAndTeamsFromCommands(string command, out int numOfPlayers, out int numOfTeams)
        {
            numOfPlayers = 0;
            numOfTeams = 0;

            Regex regex = new Regex(@"(\d+) players?", RegexOptions.IgnoreCase);
            Match match = regex.Match(command);
            if (match.Success)
            {
                numOfPlayers = int.Parse(match.Groups[1].Value);
            }

            regex = new Regex(@"(\d+) teams?", RegexOptions.IgnoreCase);
            match = regex.Match(command);
            if (match.Success)
            {
                numOfTeams = int.Parse(match.Groups[1].Value);
            }


        }

        static void ExtractTime(string sentence, out int minutes, out int seconds)
        {
            int[] time = new int[2];

            minutes = 0;
            seconds = 0;

            Regex regex = new Regex(@"(\d+) minute(?:s)?", RegexOptions.IgnoreCase);
            Match match = regex.Match(sentence);
            if (match.Success)
            {
                minutes = int.Parse(match.Groups[1].Value);
            }

            regex = new Regex(@"(\d+) second(?:s)?", RegexOptions.IgnoreCase);
            match = regex.Match(sentence);
            if (match.Success)
            {
                seconds = int.Parse(match.Groups[1].Value);
            }
        }

        private void CreateGameByVoice()
        {
            GameManager.GameManager.Instance.CreateGame();
            StopListening();
        }

        private void VoiceReply(string voiceText)
        {
            Debug.Log("Voice Reply: " + voiceText);
            Speaker.Instance.SpeakNative(voiceText);
        }
    
        public void DisableRecognition(string _)
        {
            m_Recognizer?.Stop();
        }

        public void EnableRecognition(string _)
        {
            m_Recognizer?.Start();
        }
    }
}
