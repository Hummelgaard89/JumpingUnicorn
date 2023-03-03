namespace JumpingUnicorn.MovingObjects
{
    public class Obstacle : MovingObject
    {
        public bool ObstacleEdible { get; set; }

        public Obstacle(string obstaclePath, double obstaclePosition, bool obstacleEdible) : base(obstaclePath, obstaclePosition)
        {
            ObstacleEdible = obstacleEdible;
        }
    }
}
