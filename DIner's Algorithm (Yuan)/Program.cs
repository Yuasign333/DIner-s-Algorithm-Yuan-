using System;
using System.Threading;

namespace Diner_s_Algorithm_Yuan_
{
    class Program
    {
        static void Main()
        {
            // ----- Important Variables -----
            
            Random rnd = new Random(); // Random number generator for simulating eating and resting durations

            int[] dinerState = { 0, 0, 0, 0, 0 }; // 0 = waiting, 1 = resting, 2 = eating

            bool[] forks = { true, true, true, true, true }; // shared forks

            int[] dinerEatCount = { 0, 0, 0, 0, 0 }; // Tracks how many times each diner has eaten throughout the simulation

            int[] eatDuration = { 0, 0, 0, 0, 0 }; // duration foe eating

            int[] restDuration = { 0, 0, 0, 0, 0 }; // duration for resting

            int leftFork; // left fork

            int rightFork; // right fork

            int totalEaten = 0; // Counts how many unique diners have eaten at least once (used to end the simulation)

            int cycle = 1; // cycle counter

            while (totalEaten < dinerState.Length) // continue until all diners have eaten at least once
            {

                Console.Clear(); // clear the console for each cycle

                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"\n--- Cycle {cycle} ---"); // display current cycle number
                Console.ResetColor();

                for (int i = 0; i < dinerState.Length; i++) // iterate through each diner ( whole for loop runs once per cycle)
                {

                    leftFork = i; // Diner i's left fork

                    rightFork = (i + 1) % dinerState.Length; // Diner i's is the right fork (that's why we plus it by 1)
                                                             // then using modulo (%) ensures that the last diner’s right fork wraps around to the first fork (index 0), modeling a circular table.

                    if (dinerState[i] == 1) // Resting
                    {
                        restDuration[i]--; // decrement rest duration for diner i

                        if (restDuration[i] <= 0) // diner has finished resting
                        {
                            dinerState[i] = 0; // change state to waiting

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"\nDiner {i + 1} is done resting and is now waiting."); // display message when diner finishes resting
                            Console.ResetColor();
                        }
                        else // diner is still resting
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"\nDiner {i + 1} is resting ({restDuration[i]} cycles left)."); // display message when diner continues resting
                            Console.ResetColor();
                        }
                        continue; // skip to next diner
                    }

                    if (dinerState[i] == 2) // Eating
                    {
                        eatDuration[i]--; // decrement eat duration for diner i

                        if (eatDuration[i] <= 0) // diner has finished eating
                        {
                            dinerState[i] = 1; // change state to resting

                            // release both forks

                            forks[leftFork] = true; 

                            forks[rightFork] = true; 

                            restDuration[i] = rnd.Next(1, 5); // 1 to 4 cycles ( to have better probability diner can eat again because of the few cycles of resting)

                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"\nDiner {i + 1} has finished eating and is now resting for {restDuration[i]} cycles."); // display message when diner finishes eating
                            Console.ResetColor();
                        }
                        else // diner is still eating
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine($"\nDiner {i + 1} continues eating. ({eatDuration[i]} cycles left)"); // display message when diner continues eating
                            Console.ResetColor();
                        }
                        continue; // skip to next diner
                    }

               

                    if (dinerState[i] == 0 && forks[leftFork] && forks[rightFork]) // If diner is waiting and both forks are avaialble
                    {
                        dinerState[i] = 2; // change state to eating

                        // acquire both forks

                        forks[leftFork] = false;

                        forks[rightFork] = false; 

                        eatDuration[i] = rnd.Next(4, 12); // 4 to 11 cycles

                        dinerEatCount[i]++; // increment the count of how many times this diner has eaten

                        if (dinerEatCount[i] == 1) // this condition checks if the diner has eaten for the first time
                        {
                            totalEaten++; // count unique diners who have eaten at least once
                        }


                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"\nDiner {i + 1} starts eating for {eatDuration[i]} cycles."); // display message when diner starts eating
                        Console.ResetColor();
                    }
                    else if (dinerState[i] == 0) // Forks are not available ( diner is still waiting)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine($"\nDiner {i + 1} is waiting (forks not available)."); // display message when diner is waiting
                        Console.ResetColor();
                    }
                }
                Thread.Sleep(1000);

                cycle++; // increment cycle counter

            }
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nAll diners have eaten at least once after {cycle} cycles."); // display message when all diners have eaten at least once
            Console.ResetColor();

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.WriteLine("\n--- Final Diner Summary ---");

            for (int i = 0; i < dinerEatCount.Length; i++) // display summary of how many times each diner has eaten
            {
                Console.WriteLine($"\nDiner {i + 1} has eaten {dinerEatCount[i]} time(s).");
            }
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
