//Michael Sandelli
//Intro to Computing for Engineers
//This program is used to calculate range and endurance of aircraft configurations with varying numbers of passengers
//Included in this build are ints, doubles, parse, bools, strings, and math functions
using System.IO;
using System;
using CsvHelper;
using System.Globalization;
namespace ENGR115_March2023_CourseProject
{
    public class Program
    {

        static void Main(string[] args)

        {
            //Introducing the program to the user
            Console.WriteLine("This is the Breguet Formula Calculator for finding Range and Endurance for aircraft.");
            Console.WriteLine();

            int configsQty = GetConfigurationQty(); //method set up to get the # of aircraft configurations

            Console.WriteLine("Let's move on to the next step, shall we?");
            Console.WriteLine();

            Aircraft[] outputConfigs = new Aircraft[configsQty]; //set up an array that calls the Aircraft class and has a length of the inputted amount of configs

            for (int i = 0; i < configsQty; i++) //for loop set up to run the Aircraft class the amount of times that equals the # of configs
            {
                Aircraft config = new Aircraft(i);
                outputConfigs[i] = config;
            }

            //even spacing added to the display table header
            Console.WriteLine(string.Format("{0,-10}  {1,-10}   {2,-10}   {3,-10}   {4,-10}   {5,-10}", "Aircraft", "Passengers", "T/O Weight", "Fuel", "Range", "Endurance "));
            Console.WriteLine(string.Format("{0,-10}  {1,-10}   {2,-10}   {3,-10}   {4,-10}   {5,-10}", "", "", "(lbs)", "(gals)", "(miles)", "(hrs)"));

            int x = 1;
            foreach (Aircraft config in outputConfigs)
            //for each loop set up to display each configuration's #, passenger amount, and calculated results
            //loop stops when the amount of configurations inputted are reached
            {
                //display values are now pulled from the AircraftConfiguration class using config.<variable>
                Console.WriteLine("{0,-10}  {1,-10}   {2,-10}   {3,-10}   {4,-10}   {5,-10}", x, config.passAmount, config.gToWeight, config.fuelAvail, config.range, config.endurance);
                x++; //using x and x++ to increment the aircraft #
            }

            var docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\output.txt";
            using (var streamwriter = new StreamWriter(docPath))
            {
                using (var csvwriter = new CsvWriter(streamwriter, CultureInfo.InvariantCulture)) 
                {
                    //var aircraft = Aircraft.GetConfigurationQty();
                    csvwriter.WriteRecords(outputConfigs);
                }
            }
            string[] headings = { "Aircraft", "Passengers", "T/O Weight", "Fuel", "Range", "Endurance " };
            string separator = ",";
            string[] outputCsv;


            Console.ReadLine(); //Adding a pause between results and asking for more calculations

            //setting up a bool to restart the program if the user would like to compare more configurations
            string userinputs;
            bool moreCalc;
            moreCalc = false;
            Console.WriteLine();
            Console.WriteLine("Would you like to perform more calculations?");
            userinputs = Console.ReadLine();
            if (userinputs == "y" || userinputs == "Y" || userinputs == "yes" || userinputs == "YES" || userinputs == "Yes") //various forms of 'yes'
            {
                moreCalc = true;
                Console.Clear();
                Main(args);
            }
            else //exiting the program when any input other than what's in the above if statement is given
            {
                Console.WriteLine("Thank you for using this calculator!");
                System.Environment.Exit(0);
            }
        }
       

        public static int GetConfigurationQty()  // setting up a method for the configurations quantity
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

    public class Aircraft : Program  //setting up a public class for calculations and for being called in the Program class
    {
        //using ints for whole numbers and floats for values that contain decimals
        //made constants and variables that are used only within the class private
        //made formula outputs public so they can be called in the Program class for the output display table
        private int mGWeight = 2440;
        private int eWeight = 1600;
        private int FuelCap = 50;
        private int fuelWeightpg = 6;
        private int passWeight = 170;
        private int luggage = 25;
        private int wingArea = 174;
        private int mUsefulLoad = 840;
        private float propEff = .8f;
        private float specFuelCon = (float)(7.5 * Math.Pow(10, -7f));
        private float maxCLOverCD = 14.5f;
        private float maxCLOverCD3halves = 10f;
        private float airDense = (float)(20.38 * Math.Pow(10, -4f));
        public float passAmount;
        private float passluggweight;
        public float fuelAvail;
        private float totalFuelW;
        public float gToWeight;
        private float nofuel;
        private float rangeft;
        public float range;
        private float enduranceHrs;
        public float endurance;







        public Aircraft(int configNum) //created a constructor within the class that handles all of the calculations
        {
            passAmount = GetPassengerQty(configNum);
            passluggweight = (passWeight + luggage) * passAmount;
            fuelAvail = (mUsefulLoad - passluggweight) / fuelWeightpg;
            fuelAvail = (mUsefulLoad - passluggweight) / fuelWeightpg;
            if (fuelAvail > 50)
            {
                fuelAvail = FuelCap;
            }
            totalFuelW = fuelAvail * fuelWeightpg;
            gToWeight = eWeight + passluggweight + totalFuelW;
            nofuel = gToWeight - totalFuelW;
            if (gToWeight > 2440)
            {
                gToWeight = mGWeight;
            }
            fuelAvail = (gToWeight - nofuel) / fuelWeightpg;
            rangeft = (float)(propEff / specFuelCon * maxCLOverCD * Math.Log(mGWeight / nofuel));
            range = (float)Math.Round(rangeft / 5280.0, 0);
            enduranceHrs = (float)((propEff / specFuelCon) * maxCLOverCD3halves * Math.Sqrt(2.0 * airDense * wingArea) * (Math.Pow(nofuel, -0.5f) - Math.Pow(mGWeight, -0.5)) / 3600.0f);
            endurance = (float)Math.Round(enduranceHrs, 1);
        }


        public float GetPassengerQty(int configNum) // setting up a method for getting the passenger quantity for each configuration

        //moved this method to Aircraft class as all of the calculations depend on # of passengers inputted
        {
            float passQty = 0;
            bool passValid = false;
            while (!passValid)
            {
                Console.WriteLine("How many passengers are on configuration {0} ?", configNum + 1);
                if (!float.TryParse(Console.ReadLine(), out passQty))
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