using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoFUN.Entities;
using BoFUN.GameManager;
using BoFUN.Utilities;
using System;
using TMPro;
using BoFUN.Board.MiniGames;

public class BoardScreenManager : MonoBehaviour
{
    public static BoardScreenManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public Camera boardCamera;
    public Camera UICamera;
    

    private enum State
    {
        Pantomime = 0,
        Trivia = 1,
        Pictionary = 2,
        Board
    }
    private State boardState = State.Board;

    [Serializable]
    public struct BoardScreenOptions
    {
        public GameObject screen;
        public List<Step> steps;
        public DiceThrower diceThrower;
        public List<TMP_Text> teamInfo;
        
        [Space(5)]
        public PopUpInfo popUpInfo;
        public Sprite[] minigameInfoBackgrounds;
    }

    [Serializable]
    public struct TriviaScreenOptions
    {
        public GameObject screen;
        public TriviaScreenController controller;
    }

    [Serializable]
    public struct PantomimeScreenOptions
    {
        public GameObject screen;
        public PantomimeScreenController controller;
    }

    [Serializable]
    public struct PictionaryScreenOptions
    {
        public GameObject screen;
        public PictionaryScreenController controller;
    }

    public BoardScreenOptions boardScreenOptions = new BoardScreenOptions
    {
        steps = new List<Step>(),
    };

    [Space(10)]
    public GameObject minigamesScreen;

    public PantomimeScreenOptions pantomimeScreenOptions = new PantomimeScreenOptions
    {

    };
    public TriviaScreenOptions triviaScreenOptions = new TriviaScreenOptions
    {

    };
    public PictionaryScreenOptions pictionaryScreenOptions = new PictionaryScreenOptions
    {

    };


    private Dictionary<Team, TeamPositionIndicator> teamPositionIndicators = new Dictionary<Team, TeamPositionIndicator>();

    private Team nextTeam = null;
    private int lastDiceRollNumber = 0;

    // ========= Public Methods =========

    public void ShowScreen(bool show)
    {
        if (show)
        {
            // Send game started event to server
            NetworkUtilities.Instance.Post(GameManager.Instance.networkSettings.serverURL + "/" + GameManager.Instance.networkSettings.gameAPI.startGamePath, "", (success, result) =>
            {
                if (!success)
                {
                    Debug.LogError("Could not start game", this);
                }
                else
                {
                    // Hide mouse
                    //Cursor.visible = false; // TODO Enable command 

                    ResetBoard();
                }
            });

        }
        else
        {
            Cursor.visible = true;
        }

        boardScreenOptions.screen.SetActive(show);
    }

    // ========= Private Methods =========

    void Start()
    {
        SocketEventHandler.Instance.teamUpdatedEvent.AddListener(RepaintTeamIndicator);
    }

    /// <summary>
    /// Resets the board state and spawns the team indicators
    /// </summary>
    private void ResetBoard()
    {
        boardState = State.Board;

        // Create random Steps
        RandomizeSteps();

        // Set all teams to 0 position
        SpawnTeamIndicators();

        // Get Next Playing Team
        GetNextPlayingTeam();


        // Initialize dice thrower
        boardScreenOptions.diceThrower.AllowDiceRoll = true;
        boardScreenOptions.diceThrower.ResetAndWaitDiceRoll(CreateRound);
        
        // Show board screen
        ShowStateScreen();
    }

    private void RandomizeSteps()
    {
        foreach(Step s in boardScreenOptions.steps)
        {
            s.AssignRandomStepType();
        }
    }

    private void SpawnTeamIndicators()
    {
        // Remove any old indicators
        DestroyTeamIndicators();

        //Vector3 offsetStart = new Vector3(0, 0, -.3f);
        //Vector3 offsetEnd = new Vector3(0, 0, .3f);
        //Vector3 offsetDirection = (offsetEnd - offsetStart).normalized;
        //float offsetPerTeam = (offsetEnd - offsetStart).magnitude / GameManager.Instance.currentGame.teams.Length;

        for (int i = 0; i < GameManager.Instance.currentGame.teams.Length; i++)
        {
            Team t = GameManager.Instance.currentGame.teams[i];
            TeamPositionIndicator indicator = TeamPositionIndicator.Create(t, Vector3.zero);
            indicator.positionInBoard = 0;
            teamPositionIndicators.Add(t, indicator);
        }

        teamIndicatorsInSteps.Clear();
        for (int i = 0; i < boardScreenOptions.steps.Count; i++)
        {
            teamIndicatorsInSteps.Add(new List<TeamPositionIndicator>());
        }
        teamIndicatorsInSteps[0].AddRange(teamPositionIndicators.Values); // Add all indicators to 0 position


        // Move actual gameobjects to the correct positions

        foreach (var indicator in teamPositionIndicators.Values)
        {
            indicator.transform.position = getIndicatorInStepPosition(indicator);
        }
    }

    private void DestroyTeamIndicators()
    {
        foreach (TeamPositionIndicator ind in teamPositionIndicators.Values)
        {
            Destroy(ind);
        }
        teamPositionIndicators.Clear();

        for (int i = 0; i < teamIndicatorsInSteps.Count; i++)
        {
            if (teamIndicatorsInSteps[i] != null)
                teamIndicatorsInSteps[i].Clear();
        }
    }

    private void RepaintTeamIndicator(Team t)
    {
        if (teamPositionIndicators.TryGetValue(t, out TeamPositionIndicator i))
            i.Repaint();
    }

    private void ShowStateScreen()
    {
        switch (boardState) {
            case State.Board:
                ShowBoard();
                break;
            case State.Pantomime:
                ShowPantomime();
                break;
            case State.Pictionary:
                ShowPictionary();
                break;
            case State.Trivia:
                ShowTrivia();
                break;
        } 
    }

    private void ShowBoard()
    {
        this.UICamera.enabled = false;
        this.boardCamera.enabled = true;

        this.boardScreenOptions.screen.SetActive(true);
        this.minigamesScreen.SetActive(false);
    }

    private void ShowPictionary()
    {
        // Initialize with current round
        this.pictionaryScreenOptions.controller.InitializeWithRound(GameManager.Instance.currentGame.GetCurrentRound());

        this.UICamera.enabled = true;
        this.boardCamera.enabled = false;

        this.boardScreenOptions.screen.SetActive(false);

        this.minigamesScreen.SetActive(true);
        this.triviaScreenOptions.screen.SetActive(false);
        this.pantomimeScreenOptions.screen.SetActive(false);
        this.pictionaryScreenOptions.screen.SetActive(true);
    }

    private void ShowPantomime()
    {
        // Initialize with current round
        this.pantomimeScreenOptions.controller.InitializeWithRound(GameManager.Instance.currentGame.GetCurrentRound());
        
        this.UICamera.enabled = true;
        this.boardCamera.enabled = false;

        this.boardScreenOptions.screen.SetActive(false);

        this.minigamesScreen.SetActive(true);
        this.triviaScreenOptions.screen.SetActive(false);
        this.pantomimeScreenOptions.screen.SetActive(true);
        this.pictionaryScreenOptions.screen.SetActive(false);
    }

    private void ShowTrivia()
    {
        // Initialize with current round
        this.triviaScreenOptions.controller.InitializeWithRound(GameManager.Instance.currentGame.GetCurrentRound());


        this.UICamera.enabled = true;
        this.boardCamera.enabled = false;

        this.boardScreenOptions.screen.SetActive(false);

        this.minigamesScreen.SetActive(true);
        this.triviaScreenOptions.screen.SetActive(true);
        this.pantomimeScreenOptions.screen.SetActive(false);
        this.pictionaryScreenOptions.screen.SetActive(false);
    }

    private void GetNextPlayingTeam()
    {
        NetworkUtilities.Instance.Get(GameManager.Instance.networkSettings.serverURL + "/" + GameManager.Instance.networkSettings.gameAPI.getNextTeamPath, "",
            (bool success, string response) => {
                if (success)
                {
                    Team teamResult = Team.CreateFromJSON(response);

                    nextTeam = GameManager.Instance.currentGame.GetTeam(teamResult.id);

                    // Show next playing team
                    ShowNextPlayingTeamInformationText();
                }
                else
                {
                    Debug.LogError("Error: " + response, this);
                }
            });
    }

    private void ShowNextPlayingTeamInformationText()
    {
        if (nextTeam == null)
            return;

        foreach(TMP_Text t in boardScreenOptions.teamInfo)
        {
            t.text = $"{nextTeam.name} plays";
        }
    }

    private void CreateRound(int diceRollNumber) 
    {
        // Get the team's step minigame and create round
        if (teamPositionIndicators.TryGetValue(nextTeam, out TeamPositionIndicator indicator))
        {
            Step currentStep = boardScreenOptions.steps[indicator.positionInBoard];

            MiniGame minigameType = currentStep.GetMinigame();

            // Send create round to server
            NetworkUtilities.Instance.Post($"{GameManager.Instance.networkSettings.serverURL}/{GameManager.Instance.networkSettings.roundAPI.GetNewRoundPath(minigameType)}", "",
            (success, result) =>
            {
                if (!success)
                {
                    Debug.LogError("Could not create round", this);
                }
                else
                {
                    Round newRound = Round.CreateFromJSON(result);

                    // Remove CurrentRoundOnUpdate listener from previous round
                    GameManager.Instance.currentGame.GetCurrentRound()?.onUpdate.RemoveListener(CurrentRoundOnUpdate);

                    // Add round to the game
                    Array.Resize(ref GameManager.Instance.currentGame.rounds, GameManager.Instance.currentGame.rounds.Length + 1);
                    GameManager.Instance.currentGame.rounds[GameManager.Instance.currentGame.rounds.Length - 1] = newRound;

                    lastDiceRollNumber = diceRollNumber;

                    newRound.onUpdate.AddListener(CurrentRoundOnUpdate);

                    // Show player information text
                    ShowNextPlayerInformationText();
                }

            });
        }
        else
        {
            Debug.LogError("Could not find indicator for the team : " + nextTeam.name, this);
        }

    }

    private void ShowNextPlayerInformationText()
    {
        // Get current round
        Round currentRound = GameManager.Instance.currentGame.GetCurrentRound();
        Debug.Log("Searching for player : " + currentRound.player);
        Player currentPlayer = GameManager.Instance.currentGame.GetPlayer(currentRound.player);

        Debug.Log($"{currentPlayer.username} is playing {currentRound.minigame}");

        // Show Next Player and Game Information
        boardScreenOptions.popUpInfo.ShowNotification($"{currentRound.minigame}", $"<b>{currentPlayer.username}</b>, open your phone to begin", boardScreenOptions.minigameInfoBackgrounds[(int)currentRound.minigame], .5f, null);


        // TODO Wait from smartphone response that the game has started        

    }

    private void CurrentRoundOnUpdate(Round currentRound)
    {
        if (currentRound.ended && boardState != State.Board)
        {
            // 1) Close open minigame screen
            boardState = State.Board;
            ShowStateScreen();

            Team t = GameManager.Instance.currentGame.GetTeam(currentRound.team);
            int teamPosition = teamPositionIndicators[t].positionInBoard;
            int newTeamPosition = teamPosition + lastDiceRollNumber;
            // 2) Move team Indicator if victory
            if (currentRound.victory)
            {
                // Has the game finished?
                if(newTeamPosition > boardScreenOptions.steps.Count - 1)
                {
                    // Move the team to the last step and send finish game to server
                    //MoveTeamIndicator
                }
                // Game still on, simply move team to their next step
                else
                {

                }
            }

        }
        else if (currentRound.started && boardState == State.Board)
        {
            // 1) Hide next player and game information
            boardScreenOptions.popUpInfo.HideNotification(0, null);
            // TODO:
            // 2) Transition to the minigame screen and Initialize/Reset it
            boardState = (State)((int)currentRound.minigame);
            ShowStateScreen();
        }
    }

    /// <summary>
    /// Plays end game animation and goes to the 
    /// </summary>
    /// <param name="winner"></param>
    private void PlayEndGameAnimation(Team winner)
    {

    }

    private List<List<TeamPositionIndicator>> teamIndicatorsInSteps = new List<List<TeamPositionIndicator>>();

    private LTDescr MoveTeamIndicator(Team t, int toStepIndex, Action onMoveFinished)
    {
        if(teamPositionIndicators.TryGetValue(t, out TeamPositionIndicator indicator)){

            int fromStepIndex = indicator.positionInBoard;
            
            // Start from the position we are now
            List<Vector3> path = new List<Vector3>();
            path.Add(indicator.transform.position);
            path.Add(indicator.transform.position);
            
            // Add the intermediate steps
            for (int i=fromStepIndex+1; i<toStepIndex; i++)
            {
                path.Add(boardScreenOptions.steps[i].transform.position + new Vector3(0, 0, 0.05f));
            }

            indicator.positionInBoard = toStepIndex;
            teamIndicatorsInSteps[fromStepIndex].Remove(indicator);
            teamIndicatorsInSteps[toStepIndex].Add(indicator);

            // Animate move of other indicators in start/end steps
            // Rearrange other teams if any in starting/ending step

            for(int indicatorInStartStep=0; indicatorInStartStep<teamIndicatorsInSteps[fromStepIndex].Count; indicatorInStartStep++)
            {
                TeamPositionIndicator currIndicator = teamIndicatorsInSteps[fromStepIndex][indicatorInStartStep];
                List<Vector3> rearrangePath = new List<Vector3>();
                rearrangePath.Add(currIndicator.transform.position);
                rearrangePath.Add(currIndicator.transform.position);

                Vector3 newPos = getIndicatorInStepPosition(currIndicator);
                rearrangePath.Add(newPos);
                rearrangePath.Add(newPos);

                indicator.Move(rearrangePath.ToArray(), 2 * Mathf.Abs(toStepIndex - fromStepIndex));

            }

            for (int indicatorInEndStep = 0; indicatorInEndStep < teamIndicatorsInSteps[fromStepIndex].Count-1; indicatorInEndStep++)
            {
                TeamPositionIndicator currIndicator = teamIndicatorsInSteps[toStepIndex][indicatorInEndStep];
                List<Vector3> rearrangePath = new List<Vector3>();
                rearrangePath.Add(currIndicator.transform.position);
                rearrangePath.Add(currIndicator.transform.position);

                Vector3 newPos = getIndicatorInStepPosition(currIndicator);
                rearrangePath.Add(newPos);
                rearrangePath.Add(newPos);

                indicator.Move(rearrangePath.ToArray(), 2 * Mathf.Abs(toStepIndex - fromStepIndex));
            }

            // Add final steps
            Vector3 finalPos = getIndicatorInStepPosition(indicator);
            path.Add(finalPos);
            path.Add(finalPos);

            // Animate main indicator move
            return indicator.Move(path.ToArray(), 2 * Mathf.Abs(toStepIndex - fromStepIndex), onMoveFinished);
        }
        return null;
    }

    /// <summary>
    /// It calculates the teams that are on this step and retunrs the offset from the center a new team should be positioned
    /// </summary>
    /// <returns></returns>
    private Vector3 getIndicatorInStepPosition(TeamPositionIndicator ind)
    {
        if (teamIndicatorsInSteps[ind.positionInBoard] != null)
        {
            // Ofset values for no overlaps
            Vector3 offsetMin = new Vector3(0, 0f, -.3f);
            Vector3 offsetMax = new Vector3(0, -0.001f, .3f);


            //Vector3 offsetDirection = (offsetMax - offsetMin).normalized;
            //float offsetPerTeam = (offsetMax - offsetMin).magnitude / GameManager.Instance.currentGame.teams.Length;
            float lerpValue = 1f / (teamIndicatorsInSteps[ind.positionInBoard].Count + 1f);

            int indIndexInStep = teamIndicatorsInSteps[ind.positionInBoard].IndexOf(ind);

            return boardScreenOptions.steps[ind.positionInBoard].transform.position + Vector3.Lerp(offsetMin, offsetMax, lerpValue * (indIndexInStep+1));
        }
        
        return Vector3.zero;
    }

}
