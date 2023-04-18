//Michael Sandelli
//Intro to Computing for Engineers
//This program is used to calculate range and endurance of aircraft configurations with varying numbers of passengers
//Included in this build are ints, floats, parse, bools, strings, and math functions

using System.ComponentModel.DataAnnotations;

namespace ENGR115_March2023_CourseProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //using ints for whole number constants
            int configsQty = GetConfigurationQty(); 
            // 2 things - 1) This doesn't need to take an input number, as it only takes a number from the user. {Kyle}
            // 2) I condensed your code to get number of configs via calling the method to one line {Kyle}
            //int numConfigs = 0; Not used {Kyle}

            //utilizing strings to display text and place user inputs into formulas
            //string userinputs; Not used in this method {Kyle}
            //string pressEnter = "Press Enter to continue."; No need for this delay {Kyle}

            //Introducing the program to the user
            Console.WriteLine("This is the Breguet Formula Calculator for finding Range and Endurance for aircraft.");

            //Console.WriteLine(pressEnter); No need for this delay {Kyle}
            //Console.ReadLine();

            Console.WriteLine("Let's move on to the next step, shall we?");

            float[] passAmount = new float[configsQty];
            float[] gToWeightR = new float[configsQty];
            float[] fuelAvailR = new float[configsQty];
            float[] rangeOutputR = new float[configsQty];
            float[] endurOutputR = new float[configsQty];
            float[] configsAmt = new float[configsQty];

            //using a for loop to increment the array based on # of configurations
            for (int i = 0; i < configsQty; i++)
            {
                int passQty;
                // getting passenger amounts from GetPassengerQty method
                passAmount[i] = GetPassengerQty(i); //Changed to only one input for passQty method since you call it for each config {Kyle}

                //constants for calculations for all numbers of configurations
                int mGWeight = 2440;
                int eWeight = 1600;
                int FuelCap = 50;
                int fuelWeightpg = 6;
                int passWeight = 170;
                int luggage = 25;
                int wingArea = 174;
                int mUsefulLoad = mGWeight - eWeight;
                //using floats for non-whole number constants
                float rangeOutput, endurOutput;
                float propEff = .8f;
                float specFuelCon = (float)(7.5 * Math.Pow(10, -7f));
                float maxCLOverCD = 14.5f;
                float maxCLOverCD3halves = 10f;
                float airDense = (float)(20.38 * Math.Pow(10, -4f));

                //specific variables for configuration 1
                float passluggweight = (passWeight + luggage) * passAmount[i];
                float fuelAvail = (mUsefulLoad - passluggweight) / fuelWeightpg;
                if (fuelAvail > 50)
                {
                    fuelAvail = FuelCap;
                }//; This semi-colon shouldn't be here {Kyle}
                float totalFuelW = fuelAvail * fuelWeightpg;
                float gToWeight = eWeight + passluggweight + totalFuelW;
                gToWeightR[i] = gToWeight;
                float nofuel = gToWeight - totalFuelW;
                if (gToWeight > 2440)
                {
                    gToWeight = 2440;
                    fuelAvail = (gToWeight - nofuel) / fuelWeightpg;
                }
                fuelAvailR[i] = fuelAvail;

                //perform calculations for configuration 1 using floats
                //caluclations for range
                float rangeft = propEff / specFuelCon * maxCLOverCD * MathF.Log(mGWeight / nofuel);
                float range = rangeft / 5280.0f;
                rangeOutputR[i] = (float)Math.Round(range, 0);

                //calculations for endurance
                float endurance = (propEff / specFuelCon) * maxCLOverCD3halves * MathF.Sqrt(2.0f * airDense * wingArea) *
                    (MathF.Pow(nofuel, -0.5f) - MathF.Pow(mGWeight, -0.5f)) / 3600.0f;
                endurOutputR[i] = (float)Math.Round(endurance, 1);

                Console.WriteLine(string.Format("{0,-10}  {1,-10}   {2,-10}   {3,-10}   {4,-10}   {5,-10}", "Aircraft", "Passengers", "T/O Weight", "Fuel", "Range", "Endurance "));
                Console.WriteLine(string.Format("{0,-10}  {1,-10}   {2,-10}   {3,-10}   {4,-10}   {5,-10}", "", "", "(lbs)", "(gals)", "(miles)", "(hrs)"));
                for (int x = 0; x <= i; x++)
                {
                    Console.WriteLine("{0,-10}  {1,-10}   {2,-10}   {3,-10}   {4,-10}   {5,-10}", configsAmt[x], passAmount[x], gToWeightR[x], fuelAvailR[x], rangeOutputR[x], endurOutputR[x]);
                }
            }
        }
        // setting up a method for the configurations quantity
        static int GetConfigurationQty()
        {
            int numConfigs = 0;
            string userinputs;
            //setting up a while loop for input validation
            bool rightNumconfigs = false;
            while (!rightNumconfigs)
            {
                Console.WriteLine("Please input the number of aircraft configurations you would like to compare.");
                userinputs = Console.ReadLine();
                if(!int.TryParse(userinputs, out numConfigs)){
                    Console.WriteLine("You entered a non-numeric value, please enter a numeric value for your configurations, thank you!");
                }
                else if(0 < numConfigs && numConfigs <= 5){
                    rightNumconfigs = true;
                    Console.WriteLine("Thank you for your input");
                    //Console.ReadLine(); No need for this delay {Kyle}
                    break;  //loop being broken once the range conditions are met
                }
                else{
                    Console.WriteLine("Please ensure your value is between 1 and 5, thank you!");
                }
            }
            //using another while loop with if-else conditions to validate that while the input is numeric, it must be within a given range
            //range sought is 1-5 due to 0-4 passengers
            //@@@ I put all of this into the first while loop to make it run better and catch all errors in one loop {Kyle}
            return numConfigs;
        }
        // setting up a method for getting the passenger quantity for each configuration
        static float GetPassengerQty(int configNum) //Only need one input since you're calling this for each config {Kyle}
        {
            string userinputs;
            float passQty = 0;
            bool passValid = false;
            while (!passValid)
            {
                Console.WriteLine("How many passengers are on configuration {0} ?", configNum + 1);
                userinputs = Console.ReadLine();
                if(!float.TryParse(userinputs, out passQty)){
                    Console.WriteLine("You entered a non-numeric value, please enter a numeric value, thank you!");
                }
                else if(0 <= passQty && passQty <= 4){
                    passValid = true;
                    break;
                }
                else{
                    Console.WriteLine("Please ensure your value is between 0 and 4, thank you!");
                }
            }
            //@@@@@ Condensed your while loop to one loop above {Kyle}
            //using while loop with if-else conditions to validate that while the input is numeric, it must be within a given range
            return passQty;      
        }
    }
}