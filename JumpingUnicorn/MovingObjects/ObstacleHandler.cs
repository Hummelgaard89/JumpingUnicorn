using JumpingUnicorn.Database;
using JumpingUnicorn.MovingObjects;
using JumpingUnicorn.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Web;
using JumpingUnicorn.GameHandeling;

namespace JumpingUnicorn.Obstacles
{
    public class ObstacleHandler
    {        
        public ObstacleHandler() {}

        private IJSRuntime JSRT;
        public Game objRefGame;
        private DotNetObjectReference<Game> objRefObstacleHandeling;
        private List<Obstacle> ObstaclesList = new List<Obstacle>();
        public List<Obstacle> ActiveObstaclesList;
        public MarkupString obstaclePath = new MarkupString();
        private Random rnd = new Random();
        double obstaclePosition = 0;
        private bool isEaten = false;
        private Obstacle activeObstacle;
        private IJSObjectReference? jsModule;
        public static double obstacleSpeed = 0;
        public static bool isPlayerAlive = true;

        public ObstacleHandler(IJSRuntime jsrt)
        {
            JSRT = jsrt;
        }

        
        public async void OnAfterRenderingObstaleHandler()
        {
            jsModule =  await JSRT.InvokeAsync<IJSObjectReference>("import", "./Pages/Game.razor.js");
            await JSRT.InvokeVoidAsync("instantiateListeners", objRefObstacleHandeling);
        }

        public async void InitializeObstacleHandler()
        {
            objRefObstacleHandeling = DotNetObjectReference.Create(objRefGame);
            GetObstacles();
        }

        public void StartThread()
        {
            Thread obstacleThread = new Thread(new ThreadStart(Obstacles));
            obstacleThread.Name = "ObstacleThread";
            obstacleThread.Start();
        }

        //Gets all the obstacles, from the two folder, and sets if they're edible or not.
        private void GetObstacles()
        {
            string[] ediblePaths = Directory.GetFiles(Directory.GetCurrentDirectory() + "/wwwroot/Images/Candy/Edible", "*.png");
            string[] toxicPaths = Directory.GetFiles(Directory.GetCurrentDirectory() + "/wwwroot/Images/Candy/Toxic", "*.png");
            foreach (string ediblePath in ediblePaths) 
            {
                string edibleFilename = Path.GetFileName(ediblePath);
                ObstaclesList.Add(new Obstacle("/Images/Candy/Edible/" + edibleFilename, 0, true));
            }
            foreach (string toxicPath in toxicPaths)
            {
                string toxicFilename = Path.GetFileName(toxicPath);
                ObstaclesList.Add(new Obstacle("/Images/Candy/Toxic/" + toxicFilename, 0, false));
            }
        }
        //Is the thread method, to handle the flow of the obstacles.
        public void Obstacles()
        {
            SpawnNewObstacle();
            while (isPlayerAlive == true)
            {
                while (obstaclePosition < 100 && isPlayerAlive == true)
                {
                    jsModule.InvokeVoidAsync("ChangeObstaclePosition", obstaclePosition.ToString().Replace(",", ".") + "%", "Obstacle1");
                    obstaclePosition = Math.Round(obstaclePosition + 0.5 + obstacleSpeed, 2);
                    CollisionDetection(activeObstacle.ObstacleEdible);
                    Thread.Sleep(10);
                }
                SpawnNewObstacle();
                Thread.Sleep(10);
                obstacleSpeed = obstacleSpeed + 0.01;
                obstaclePosition = 0;
            }
        }
        //Is the method to detect if the avatar hits the obstacle, and if it hits it, check if it's edible to get more points or toxic to end game.
        public void CollisionDetection(bool isEdible)
        {
            if (obstaclePosition > 78 && obstaclePosition < 95 && Game.avatarBottomHeight < 16)
            {
                if (isEdible == true)
                {
                    GameHandler.extraGamePoints = GameHandler.extraGamePoints + 5;
                    Console.WriteLine("You just got more points");
                    obstaclePosition= 0;
                    SpawnNewObstacle();
                }
                else if (isEdible == false)
                {
                    isPlayerAlive = false;
                    GameHandler.extraGamePoints = 0;
                }
            }
        }
        //Method for spawning a new opstacle
        public void SpawnNewObstacle()
        {
            activeObstacle = ObstaclesList[rnd.Next(0, (ObstaclesList.Count - 1))];
            obstaclePath = new MarkupString(activeObstacle.ObstaclePath);
            objRefGame.UpdateState();
        }
    }
}
