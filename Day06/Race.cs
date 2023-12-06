namespace Day06
{
    internal class Race
    {
        public long Time { get; set; }
        public long Distance { get; set; }

        public long GetNumberOfWaysToBeatTheRecord()
        {
            // Find number of times where: time * (Time - time) > Distance

            // Quadratic formula
            // ax^2 + bx + c = 0
            // x1 = (-b + sqrt(b^2 -4ac)) / 2a
            // x2 = (-b - sqrt(b^2 -4ac)) / 2a

            // minTime and maxTime are times where distance record is not yet or not anymore beaten
            var minTime = Convert.ToInt64(Math.Floor((Time - Math.Sqrt(Time * Time - 4 * Distance)) / 2));
            var maxTime = Convert.ToInt64(Math.Ceiling((Time + Math.Sqrt(Time * Time - 4 * Distance)) / 2));

            return maxTime - minTime - 1;
        }
    }
}
