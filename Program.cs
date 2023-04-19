//Michael Sandelli
//Intro to Computing for Engineers
//This program is used to calculate range and endurance of aircraft configurations with varying numbers of passengers
//Included in this build are ints, doubles, parse, bools, strings, and math functions

namespace ENGR115_March2023_CourseProject
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Introducing the program to the user
            Console.WriteLine("This is the Breguet Formula Calculator for finding Range and Endurance for aircraft.");
            int configsQty = GetConfigurationQty();

            Console.WriteLine("Let's move on to the next step, shall we?");

            List<AircraftConfiguration> outputConfigs = new List<AircraftConfiguration>();

            for (int i = 0; i < configsQty; i++)
            {
                AircraftConfiguration config = new AircraftConfiguration(i);
                outputConfigs.Add(config);
            }

            //Moved the info printing out of the loop so the info only prints once {Kyle}
            Console.WriteLine(string.Format("{0,-10}  {1,-10}   {2,-10}   {3,-10}   {4,-10}   {5,-10}", "Aircraft", "Passengers", "T/O Weight", "Fuel", "Range", "Endurance "));
            Console.WriteLine(string.Format("{0,-10}  {1,-10}   {2,-10}   {3,-10}   {4,-10}   {5,-10}", "", "", "(lbs)", "(gals)", "(miles)", "(hrs)"));
            int x = 1;
            foreach(AircraftConfiguration config in outputConfigs)
            {
                Console.WriteLine("{0,-10}  {1,-10}   {2,-10}   {3,-10}   {4,-10}   {5,-10}", x, config.passAmount, config.gToWeight, config.fuelAvail, config.range, config.endurance.ToString("F1")); //[Adam] 2023/04/19 - Lists can be indexed using brackets once initialized so this will work just fine
                x++;
            }

            Console.ReadLine(); //[Adam] 2023/04/19 - Get debugger to hold at end of program to see results before closing terminal
        }
        // setting up a method for the configurations quantity
        public static int GetConfigurationQty()
        {
            int numConfigs = 0;
            bool rightNumconfigs = false;
            while (!rightNumconfigs)
            {
                Console.WriteLine("Please input the number of aircraft configurations you would like to compare.");
                if (!int.TryParse(Console.ReadLine(), out numConfigs))
                {
                    Console.WriteLine("You entered a non-numeric value, please enter a numeric value for your configurations, thank you!");
                }
                else if (0 < numConfigs && numConfigs <= 5)
                {
                    rightNumconfigs = true;
                    Console.WriteLine("Thank you for your input");
                }
                else
                {
                    Console.WriteLine("Please ensure your value is between 1 and 5, thank you!");
                }
            }

            return numConfigs;
        }
    }

    public class AircraftConfiguration
    {
        private int mGWeight = 2440;
        private int eWeight = 1600;
        private int FuelCap = 50;
        private int fuelWeightpg = 6;
        private int passWeight = 170;
        private int luggage = 25;
        private int wingArea = 174;
        private int mUsefulLoad = 840;
        private double propEff = .8f;
        private double specFuelCon = (double)(7.5 * Math.Pow(10, -7f));
        private double maxCLOverCD = 14.5f;
        private double maxCLOverCD3halves = 10f;
        private double airDense = (double)(20.38 * Math.Pow(10, -4f));
        public double passAmount;
        private double passluggweight;
        public double fuelAvail;
        private double totalFuelW;
        public double gToWeight;
        private double nofuel;
        private double rangeft;
        public double range;
        public double endurance;

        public AircraftConfiguration(int configNum)
        {
            passAmount = GetPassengerQty(configNum);
            passluggweight = (passWeight + luggage) * passAmount;
            fuelAvail = (mUsefulLoad - passluggweight) / fuelWeightpg > 50 ? FuelCap : (mUsefulLoad - passluggweight) / fuelWeightpg;
            totalFuelW = fuelAvail * fuelWeightpg;
            gToWeight = eWeight + passluggweight + totalFuelW;
            nofuel = gToWeight - totalFuelW;
            gToWeight = gToWeight > 2440 ? 2440 : gToWeight;
            fuelAvail = (gToWeight - nofuel) / fuelWeightpg;
            rangeft = propEff / specFuelCon * maxCLOverCD * Math.Log(mGWeight / nofuel);
            range = Math.Round(rangeft / 5280.0, 0);
            endurance = (propEff / specFuelCon) * maxCLOverCD3halves * Math.Sqrt(2.0 * airDense * wingArea) * (Math.Pow(nofuel, -0.5f) - Math.Pow(mGWeight, -0.5)) / 3600.0;
        }

        // setting up a method for getting the passenger quantity for each configuration
        public double GetPassengerQty(int configNum) //Only need one input since you're calling this for each config {Kyle}
        {
            double passQty = 0;
            bool passValid = false;
            while (!passValid)
            {
                Console.WriteLine("How many passengers are on configuration {0} ?", configNum + 1);
                if (!double.TryParse(Console.ReadLine(), out passQty))
                {
                    Console.WriteLine("You entered a non-numeric value, please enter a numeric value, thank you!");
                }
                else if (0 <= passQty && passQty <= 4)
                {
                    passValid = true;
                }
                else
                {
                    Console.WriteLine("Please ensure your value is between 0 and 4, thank you!");
                }
            }

            return passQty;
        }

    }
}