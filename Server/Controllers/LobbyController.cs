using BlazorChess.Server.Hubs;
using Microsoft.AspNetCore.Mvc;

namespace BlazorChess.Server.Controllers
{
    [Route("lobby")]

    public class LobbyController : ControllerBase
    {

        [HttpGet("{lobbyID}/join")]
        public string IsLobbyFull(string lobbyID)
        {
            //if lobby doesnt exist return doesnotexist
            if (!GameHub.gameLobbies.TryGetValue(lobbyID, out var gameState))
            {
                return "doesnotexist";
            }
            else
            {
                if (gameState.player1ConnectionID != null && gameState.player2ConnectionID != null)
                {
                    //if both player slots occupied return full
                    return "full";
                }
                // if the lobby exists and has a spot return joinable
                else { return "joinable"; };
            }
        }
    }
}
