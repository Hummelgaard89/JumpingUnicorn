namespace JumpingUnicorn.MovingObjects
{
    public class MovingObject
    {
        public string ObstaclePath { get; set; }

        public double ObstaclePosition { get; set; }


        public MovingObject(string obstaclePath, double obstaclePosition)
        {
            ObstaclePath = obstaclePath;
            ObstaclePosition = obstaclePosition;
        }

    }
}
