//Michael Sandelli
//Intro to Computing for Engineers
//This program is used to calculate range and endurance of aircraft configurations with varying numbers of passengers
//Included in this build are ints, doubles, parse, bools, strings, and math functions

namespace ENGR115_March2023_CourseProject
{
    public class Program
    {
        private static int configsQty = 0;
        private static int mGWeight = 2440;
        private static int eWeight = 1600;
        private static int FuelCap = 50;
        private static int fuelWeightpg = 6;
        private static int passWeight = 170;
        private static int luggage = 25;
        private static int wingArea = 174;
        private static int mUsefulLoad = 840; //Set to the constant number, was mGWeight-eWeight {Kyle}
        //using doubles for non-whole number constants
        //double rangeOutput, endurOutput; Never used {Kyle}
        private static double propEff = .8f;
        private static double specFuelCon = (double)(7.5 * Math.Pow(10, -7f));
        private static double maxCLOverCD = 14.5f;
        private static double maxCLOverCD3halves = 10f;
        private static double airDense = (double)(20.38 * Math.Pow(10, -4f));

        //constants for calculations for all numbers of configurations 
        //Moved out of loop for efficiency and less memory usage {Kyle}

        //[Adam] 2023/04/19 - Global array declarations; We know that the fields will not be null so it's fine here but generally not recommended
        static double[] passAmount;
        static double[] gToWeightR;
        static double[] fuelAvailR;
        static double[] rangeOutputR;
        static double[] endurOutputR;

        static void Main(string[] args)
        {
            //Introducing the program to the user
            Console.WriteLine("This is the Breguet Formula Calculator for finding Range and Endurance for aircraft.");
            //using ints for whole number constants
            configsQty = GetConfigurationQty();

            //[Adam] 2023/04/19 - defines the size of the arrays based upon the user input for configsQty
            passAmount = new double[configsQty];
            gToWeightR = new double[configsQty];
            fuelAvailR = new double[configsQty];
            rangeOutputR = new double[configsQty];
            endurOutputR = new double[configsQty];

            // 2 things - 1) This doesn't need to take an input number, as it only takes a number from the user. {Kyle}
            // 2) I condensed your code to get number of configs via calling the method to one line {Kyle}
            //int numConfigs = 0; Not used {Kyle}

            //utilizing strings to display text and place user inputs into formulas
            //string userinputs; Not used in this method {Kyle}
            //string pressEnter = "Press Enter to continue."; No need for this delay {Kyle}

            //Console.WriteLine(pressEnter); No need for this delay {Kyle}
            //Console.ReadLine();

            Console.WriteLine("Let's move on to the next step, shall we?");

            DoMath(); //Calls the method for all the calculations to be done and fills the arrays with the proper info {Kyle}

            //Moved the info printing out of the loop so the info only prints once {Kyle}
            Console.WriteLine(string.Format("{0,-10}  {1,-10}   {2,-10}   {3,-10}   {4,-10}   {5,-10}", "Aircraft", "Passengers", "T/O Weight", "Fuel", "Range", "Endurance "));
            Console.WriteLine(string.Format("{0,-10}  {1,-10}   {2,-10}   {3,-10}   {4,-10}   {5,-10}", "", "", "(lbs)", "(gals)", "(miles)", "(hrs)"));
            for (int x = 0; x < configsQty; x++)
            {
                Console.WriteLine("{0,-10}  {1,-10}   {2,-10}   {3,-10}   {4,-10}   {5,-10}", x + 1, passAmount[x], gToWeightR[x], fuelAvailR[x], rangeOutputR[x], endurOutputR[x]); //[Adam] 2023/04/19 - Lists can be indexed using brackets once initialized so this will work just fine
            }

            Console.ReadLine(); //[Adam] 2023/04/19 - Get debugger to hold at end of program to see results before closing terminal
        }
        // setting up a method for the configurations quantity
        static int GetConfigurationQty()
        {
            int numConfigs = 0;
            //setting up a while loop for input validation
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
                    //Console.ReadLine(); No need for this delay {Kyle}
                    //break;  //loop being broken once the range conditions are met //[Adam] 2023/04/19 - break not necessary if rightNumconfigs is being set to true due to while condition. 
                }
                else
                {
                    Console.WriteLine("Please ensure your value is between 1 and 5, thank you!");
                }
            }
            //using another while loop with if-else conditions to validate that while the input is numeric, it must be within a given range
            //range sought is 1-5 due to 0-4 passengers
            //@@@ I put all of this into the first while loop to make it run better and catch all errors in one loop {Kyle}
            return numConfigs;
        }
        // setting up a method for getting the passenger quantity for each configuration
        static double GetPassengerQty(int configNum) //Only need one input since you're calling this for each config {Kyle}
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
                    //break; //[Adam] 2023/04/19 Break not necessary for same reason as above. Will already exit loop at correct time.
                }
                else
                {
                    Console.WriteLine("Please ensure your value is between 0 and 4, thank you!");
                }
            }
            //@@@@@ Condensed your while loop to one loop above {Kyle}
            //using while loop with if-else conditions to validate that while the input is numeric, it must be within a given range
            return passQty;
        }
        static void DoMath()
        {
            //using a for loop to increment the array based on # of configurations
            for (int i = 0; i < configsQty; i++)
            {
                //int passQty; Not used {Kyle}
                // getting passenger amounts from GetPassengerQty method
                passAmount[i] = GetPassengerQty(i); //Changed to only one input for passQty method since you call it for each config {Kyle}

                //specific variables for configuration 1
                double passluggweight = (passWeight + luggage) * passAmount[i];
                double fuelAvail = (mUsefulLoad - passluggweight) / fuelWeightpg;
                if (fuelAvail > 50)
                {
                    fuelAvail = FuelCap;
                }//; This semi-colon shouldn't be here {Kyle}
                double totalFuelW = fuelAvail * fuelWeightpg;
                double gToWeight = eWeight + passluggweight + totalFuelW;
                gToWeightR[i] = gToWeight;
                double nofuel = gToWeight - totalFuelW;
                if (gToWeight > 2440)
                {
                    gToWeight = 2440;
                    fuelAvail = (gToWeight - nofuel) / fuelWeightpg;
                }
                fuelAvailR[i] = fuelAvail;

                //perform calculations for configuration 1 using doubles
                //caluclations for range
                double rangeft = propEff / specFuelCon * maxCLOverCD * Math.Log(mGWeight / nofuel);
                double range = rangeft / 5280.0f;
                rangeOutputR[i] = (double)Math.Round(range, 0);

                //calculations for endurance
                double endurance = (propEff / specFuelCon) * maxCLOverCD3halves * Math.Sqrt(2.0f * airDense * wingArea) *
                    (Math.Pow(nofuel, -0.5f) - Math.Pow(mGWeight, -0.5f)) / 3600.0f;
                endurOutputR[i] = (double)Math.Round(endurance, 1);
            }
        }
    }
}