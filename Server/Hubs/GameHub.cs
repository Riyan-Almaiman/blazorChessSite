using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using BlazorChess.Shared;
using System.Text.Json;

namespace BlazorChess.Server.Hubs
{
    public class GameHub : Hub
    {
        public static readonly ConcurrentDictionary<string, GameState> gameLobbies = new ConcurrentDictionary<string, GameState>();
        public static readonly ConcurrentDictionary<string, string> ConnectionIdToLobbyId = new ConcurrentDictionary<string, string>();

        public async Task JoinOrCreateLobby(string connectionID, string lobbyID)
        {
            if (!gameLobbies.TryGetValue(lobbyID, out var gameState))
            {
                // If the lobby doesn't exist create a new one.
                gameState = new GameState();
                gameState.player1ConnectionID = connectionID;
                gameLobbies.TryAdd(lobbyID, gameState);

                ConnectionIdToLobbyId.TryAdd(connectionID, lobbyID);
                await Clients.Client(connectionID).SendAsync("ReceiveGameState", gameState);

            }
            else

            {
                // If the lobby already exists add the player 
                if (gameState.player1ConnectionID == null)
                {
                    gameState.player1ConnectionID = connectionID;
                    await Clients.Client(connectionID).SendAsync("ReceiveGameState", gameState);

                }
                else if (gameState.player2ConnectionID == null)
                {
                    gameState.player2ConnectionID = connectionID;
                    await Clients.Client(connectionID).SendAsync("ReceiveGameState", gameState);

                }
                else
                {
                    // If both players are occupied
                    throw new InvalidOperationException("The game lobby is full.");
                }
            }
            
            await NotifyPlayersOfJoin(gameState, connectionID, lobbyID);
        }

        private async Task NotifyPlayersOfJoin(GameState gameState, string joiningPlayerID, string lobbyID)
        {
            if (gameState.player1ConnectionID != null && gameState.player2ConnectionID != null)
            {
                await Clients.Client(gameState.player1ConnectionID).SendAsync("playerJoined", gameState);
                await Clients.Client(gameState.player2ConnectionID).SendAsync("playerJoined", gameState);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string connectionId = Context.ConnectionId;

            Console.WriteLine($"{connectionId} disconnected");

            // Find the remaining player in the lobby and notify them of the disconnection
            string lobbyID = FindLobbyByConnectionId(connectionId);

            if (lobbyID != null && gameLobbies.TryGetValue(lobbyID, out var gameState))
            {
                var remainingPlayer = gameState.player1ConnectionID == connectionId ? gameState.player2ConnectionID : gameState.player1ConnectionID;
                if (remainingPlayer != null)
                {
                    await Clients.Client(remainingPlayer).SendAsync("playerDisconnected", connectionId + " left " + lobbyID);
                }
            }

            // Remove the player from the lobby
            await RemovePlayerFromLobby(connectionId, lobbyID);

            await base.OnDisconnectedAsync(exception);
        }

        private string FindLobbyByConnectionId(string connectionID)
        {
            foreach (var lobby in gameLobbies)
            {
                if (lobby.Value.player1ConnectionID == connectionID || lobby.Value.player2ConnectionID == connectionID)
                {
                    return lobby.Key;
                }
            }

            return null;
        }

        private async Task RemovePlayerFromLobby(string connectionID, string lobbyID)
        {
            Console.WriteLine($"Removing connection ID: {connectionID}");

            if (lobbyID != null && gameLobbies.TryGetValue(lobbyID, out var gameState))
            {
                Console.WriteLine("testing");

                if (gameState.player1ConnectionID == connectionID)
                {
                    gameState.player1ConnectionID = null;
                }
                else if (gameState.player2ConnectionID == connectionID)
                {
                    gameState.player2ConnectionID = null;
                }
            }
        }

        public async Task MakeMove(string lobbyID, int player, int sourceRow, int sourceCol, int destRow, int destCol)
        {
            // Find the game state by lobby ID
            if (!gameLobbies.TryGetValue(lobbyID, out var gameState))
            {
                throw new InvalidOperationException("The game lobby does not exist.");
            }

            // Make the move
            GameState.Tile sourceTile = gameState.Board[sourceRow, sourceCol];
            GameState.Tile destinationTile = gameState.Board[destRow, destCol];

            destinationTile.Piece = sourceTile.Piece;
            destinationTile.PieceColor = sourceTile.PieceColor;

            sourceTile.Piece = "fa-sharp fa-solid";
            sourceTile.PieceColor = "";

            // send the updated game state to both players
            if (gameState.player1ConnectionID != null)
            {
                await Clients.Client(gameState.player1ConnectionID).SendAsync("ReceiveGameState", gameState);
            }

            if (gameState.player2ConnectionID != null)
            {
                await Clients.Client(gameState.player2ConnectionID).SendAsync("ReceiveGameState", gameState);
            }
        }


    }
}