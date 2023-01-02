using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoFUN.Utilities;

public class Testing : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.U))
        {
            //NetworkUtilities.Instance.Post()
            TestCreatePlayersAndAdd();
        }
    }

    public void TestCreatePlayersAndAdd()
    {
        NetworkUtilities.Instance.Post("http://localhost:8080/BoFUN/game/createPlayer", "{\"positionId\": 0}", (res, data) =>
        {
            NetworkUtilities.Instance.Post("http://localhost:8080/BoFUN/game/createPlayer", "{\"positionId\": 1}", (res, data) =>
            {
                NetworkUtilities.Instance.Post("http://localhost:8080/BoFUN/game/createPlayer", "{\"positionId\": 2}", (res, data) =>
                {
                    NetworkUtilities.Instance.Post("http://localhost:8080/BoFUN/game/createPlayer", "{\"positionId\": 3}", (res, data) =>
                    {
                        AssignPlayersToTeams();
                    });
                });
            });
        });


        //NetworkUtilities.Instance.Post()
    }

    private void AssignPlayersToTeams()
    {
        // Assign players to teams

        NetworkUtilities.Instance.Put("http://localhost:8080/BoFUN/game/assignPlayerToTeam/0", "{\"teamId\": 0}", (res, data) =>
        {
            NetworkUtilities.Instance.Put("http://localhost:8080/BoFUN/game/assignPlayerToTeam/1", "{\"teamId\": 0}", (res, data) =>
            {
                NetworkUtilities.Instance.Put("http://localhost:8080/BoFUN/game/assignPlayerToTeam/2", "{\"teamId\": 1}", (res, data) =>
                {
                    NetworkUtilities.Instance.Put("http://localhost:8080/BoFUN/game/assignPlayerToTeam/3", "{\"teamId\": 1}", (res, data) =>
                    {
                        Debug.Log("Finished Test Scenario");
                    });
                });
            });
        });
    }
}
