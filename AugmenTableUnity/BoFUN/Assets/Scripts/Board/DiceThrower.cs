using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DiceThrower : MonoBehaviour
{
    public bool allowDiceRoll = true;

    public Camera boardCamera;

    public float spawnDistanceFromCorner = .2f;
    public float diceSpawnHeight = 2f;

    public List<Dice> dice = new List<Dice>(4);

    public Transform lowerLeftSpawn;
    public Transform lowerRightSpawn;
    public Transform upperLeftSpawn;
    public Transform upperRightSpawn;

    public BoxCollider topCollider;
    public BoxCollider bottomCollider;
    public BoxCollider leftCollider;
    public BoxCollider rightCollider;

    private UnityEvent<int> onDiceRollFinished;

    Bounds playArea;
    float halfBoardHeight;
    float halfBoardWidth;
    void Start()
    {
        float diceSize = 1f;
        //if(lowerLeft.TryGetComponent<)

        // Calculate and position the spawnpoints
        halfBoardHeight = boardCamera.orthographicSize;
        halfBoardWidth = boardCamera.aspect * halfBoardHeight;

        playArea = new Bounds();
        playArea.center = new Vector3(0, 4.5f, 0);
        playArea.extents = new Vector3(halfBoardWidth + .25f, 5f, halfBoardHeight+.25f);

        lowerLeftSpawn.transform.position = new Vector3(-halfBoardWidth + diceSize, diceSpawnHeight, -halfBoardHeight + diceSize);
        lowerRightSpawn.transform.position = new Vector3(halfBoardWidth - diceSize, diceSpawnHeight, -halfBoardHeight + diceSize);
        upperLeftSpawn.transform.position = new Vector3(-halfBoardWidth + diceSize, diceSpawnHeight, halfBoardHeight - diceSize);
        upperRightSpawn.transform.position = new Vector3(halfBoardWidth - diceSize, diceSpawnHeight, halfBoardHeight - diceSize);


        // Reference spawn point to each Dice
        dice[0].Spawn = lowerLeftSpawn;
        dice[1].Spawn = lowerRightSpawn;
        dice[2].Spawn = upperLeftSpawn;
        dice[3].Spawn = upperRightSpawn;

        // Add camera references
        for (int i = 0; i < dice.Count; i++)
        {
            Dice d = dice[i];
            int tmpInt = i;
            d.onRollBegin.AddListener(() => OnDiceStartRoll(tmpInt));
            d.onRollEnd.AddListener(OnDiceEndRoll);
            d.Camera = boardCamera;
        }

        ResetDices();

        const float thickness = 10f;

        // Configure Colliders
        topCollider.size = new Vector3(halfBoardWidth * 2, 10, thickness);
        topCollider.center = new Vector3(0, 5, halfBoardHeight+ thickness/2);
        bottomCollider.size = new Vector3(halfBoardWidth * 2, 10, thickness);
        bottomCollider.center = new Vector3(0, 5, -halfBoardHeight - thickness/2);
        leftCollider.size = new Vector3(thickness, 10, halfBoardHeight*2);
        leftCollider.center = new Vector3(-halfBoardWidth-thickness/2, 5, 0);
        rightCollider.size = new Vector3(thickness, 10, halfBoardHeight * 2);
        rightCollider.center = new Vector3(halfBoardWidth + thickness/2, 5, 0);

    }

    private void Update()
    {
        foreach (Dice d in dice)
        {
            if (!playArea.Contains(d.Transform.position))
            {
                d.Transform.position = playArea.center;
            }
        }

    }
    /// <summary>
    /// Resets the 4 dice to their spanwpoints
    /// </summary>
    private void ResetDices()
    {
        foreach(var d in dice)
        {
            d.ResetDice();
            d.SetVisible(true);
        }

    }

    // Public Methods
    /// <summary>
    /// Positions the dices
    /// </summary>
    /// <param name="onDiceRolledCallback"></param>
    public void InitializeDice(UnityEvent<int> onDiceRolledCallback)
    {
        ResetDices();
        onDiceRollFinished = onDiceRolledCallback;
    }

    bool awaitingResult = false;

    private void OnDiceStartRoll(int diceIndex)
    {
        // Disable all other dice
        for(int i=0; i<dice.Count; i++)
        {
            if (i == diceIndex)
                continue;

            dice[i].SetVisible(false);
        }
        Debug.Log("Start Roll" + diceIndex);
        awaitingResult = true;
    }


    private void OnDiceEndRoll(int result)
    {
        if (awaitingResult)
        {
            Debug.Log("Result end " + result);
            onDiceRollFinished?.Invoke(result);
            awaitingResult = false;
        }
    }


}
