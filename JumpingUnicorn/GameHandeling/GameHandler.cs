using JumpingUnicorn.Data;
using JumpingUnicorn.Pages;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using System.Reflection;
using JumpingUnicorn.Obstacles;

namespace JumpingUnicorn.GameHandeling
{
    public class GameHandler
    {
        public GameHandler() { }

        private IJSRuntime JSRT;
        public Game objRefGame;
        private DotNetObjectReference<Game> objRefGameHandeling;
        private IJSObjectReference? jsModule;
        string gameScoreString = "";
        double scoreCount = 0;
        public static int extraGamePoints = 0;
        double gameSpeed = 0;
        DateTime gameStart= DateTime.Now;
        TimeSpan gameTime = TimeSpan.Zero;

        private AvatarService AvatarServiceHelper;

        public GameHandler(IJSRuntime jsrt, AvatarService avatarServiceHelper)
        {
            JSRT = jsrt;
            AvatarServiceHelper = avatarServiceHelper;
        }

        public async void OnAfterRenderingGameHandler()
        {
            jsModule = await JSRT.InvokeAsync<IJSObjectReference>("import", "./Pages/Game.razor.js");
            await JSRT.InvokeVoidAsync("instantiateListeners", objRefGameHandeling);
        }

        public async void InitializeGameHandler()
        {
            objRefGameHandeling = DotNetObjectReference.Create(objRefGame);
        }

        public void StartThread()
        {
            Thread obstacleThread = new Thread(new ThreadStart(GameHandeling));
            obstacleThread.Name = "GameThread";
            obstacleThread.Start();
        }

        //The thread method to handle the score, and change the avatar between slow, medium and fast according to the game speed.
        public void GameHandeling()
        {
            while (true)
            {
                if (ObstacleHandler.obstacleSpeed < 0.4)
                {
                    string avatarPathString = AvatarServiceHelper.FindAvatar(Avatar.AvatarSpeed.Slow);
                    Game.avatarPath = new MarkupString(avatarPathString);
                    objRefGame.UpdateState();
                }
                else if (ObstacleHandler.obstacleSpeed > 0.4 && ObstacleHandler.obstacleSpeed < 0.8)
                {
                    string avatarPathString = AvatarServiceHelper.FindAvatar(Avatar.AvatarSpeed.Medium);
                    Game.avatarPath = new MarkupString(avatarPathString);
                    objRefGame.UpdateState();
                }
                else if (ObstacleHandler.obstacleSpeed > 0.8)
                {
                    string avatarPathString = AvatarServiceHelper.FindAvatar(Avatar.AvatarSpeed.Fast);
                    Game.avatarPath = new MarkupString(avatarPathString);
                    objRefGame.UpdateState();
                }
                Thread.Sleep(1000);
                CreateScore();
            }
        }

        //Creates the scoreline, and updates it.
        public void CreateScore()
        {
            if (ObstacleHandler.isPlayerAlive == true)
            {
                gameTime = DateTime.Now - gameStart;
                scoreCount = Math.Round(gameTime.TotalSeconds, MidpointRounding.AwayFromZero) + extraGamePoints;
                gameScoreString = "<h1>Score: " + scoreCount + "</h1>";
                Game.gameScore = new MarkupString(gameScoreString);
                objRefGame.UpdateState();
            }
        }
    }
}
