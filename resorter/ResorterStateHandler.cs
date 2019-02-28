using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace resorter {
    public class ResorterStateHandler {
        // the com handler for communicating the arduino
        public JSFNComHandler ComHandler;
        // amount of steps the motor has.
        private int steps = 0;
        // current position of the catcher
        private int catcherPosition = 0;
        // current position fo the transporter
        private int transporterPosition = 0;
        // list of what resistors are in what chamber
        private List<float>[] listOfResistors = new List<float>[] {
            new List<float>(),
            new List<float>(),
            new List<float>(),
            new List<float>()
        };
        // constructor
        public ResorterStateHandler(string port, int steps) {
            // create the functions that the arduino has available
            Dictionary<string, Func<List<object>, object>> jsfnFuncs = new Dictionary<string, Func<List<object>, object>>();
            // add a print function that the arduino can use
            jsfnFuncs.Add("print", new Func<List<object>, object>((List<object> l) => {
                Console.WriteLine(string.Join(",", l.ToArray()));
                return null;
            }));
            // create the com handler
            ComHandler = new JSFNComHandler(port, jsfnFuncs);
            // set the steps
            this.steps = steps;
        }

        // start the while f'ing process
        public async void Start() {
            // positions for the ohmmeter arm
            const int ohmArmStandartPos = 45;
            const int ohmArmThrowPos = 155;
            const int ohmArmReadPos = 0;
            // loop dat shit
            while (true) {
                Console.WriteLine("turning transporter wheel");
                // turn the transporter half way, this will put one resistor on the ohmmeter
                await TransporterTurn(steps / 2, 60, 20);
                Console.WriteLine("turning ohm arm to read resistance");
                // turn the ohmmeter arm to read position
                await OhmArmPosition(ohmArmReadPos);
                Console.WriteLine("reading resistance");
                // read resistance
                float? _res = await ReadResistance();
                Console.WriteLine("resistance: " + _res);
                // if its null
                if (_res == null) {
                    Console.WriteLine("didnt receive any resistance, resetting and trying again");
                    //throw (in case there is anything on it)
                    await OhmArmPosition(ohmArmThrowPos);
                    // wait a little
                    Thread.Sleep(100);
                    // in standart pos
                    await OhmArmPosition(ohmArmStandartPos);
                    // do the loop again
                    continue;
                }
                // get the resistance as a variable that is not a float
                float res = (float)_res;
                // calculate the tolerance
                float tolerance = (Program.Settings.ToleranceIsPercentage ? res : 1) * Program.Settings.Tolerance;
                // we find the best matching chamber by finding the one where the absolute value of
                // the chamber resistance - the measured is lowest
                int bestMatching = 0;
                for (int i = 0; i < Program.Settings.Chambers.Length; i++) {
                    if (Math.Abs(Program.Settings.Chambers[bestMatching] - res) > Math.Abs(Program.Settings.Chambers[i] - res)) {
                        bestMatching = i;
                    }
                }
                // is the best matching chamber is within the tolerance?
                bool withinRange = res - tolerance < Program.Settings.Chambers[bestMatching] && res + tolerance > Program.Settings.Chambers[bestMatching];
                // if it is not within range, but it in the misc. chamber
                if (!withinRange) {
                    bestMatching = Program.Settings.Chambers.Length + 1;
                }
                Console.WriteLine("resistance is " + (withinRange ? "" : "not") + "within range");
                Console.WriteLine("adding " + res + " in chamber " + bestMatching + " to registry");
                // add the resistance the corresponding chamber list
                listOfResistors[bestMatching].Add(res);
                Console.WriteLine("turning catcher wheel to chamber: " + bestMatching);
                // turn the catcher to the chamber
                await CatcherTurnToChamber(bestMatching, 60, 20);
                Console.WriteLine("opening ohm arm");
                // throw the resistor
                await OhmArmPosition(ohmArmThrowPos);
                // wait a little
                Thread.Sleep(100);
                Console.WriteLine("resetting ohm arm");
                // put the ohmmeter arm back in the standart position
                await OhmArmPosition(ohmArmStandartPos);
                // wait we should stop
                if (stopWhenPossibleTcs != null) {
                    Console.WriteLine("stopping");
                    //we stopping bois
                    stopWhenPossibleTcs.SetResult(listOfResistors);
                    stopWhenPossibleTcs = null;
                    return;
                }
            }
        }

        private TaskCompletionSource<List<float>[]> stopWhenPossibleTcs = null;
        public Task<List<float>[]> StopWhenPossible() {
            Console.WriteLine("stopping once current routine is done");
            //create an awaiter
            //the loop in start will check if it should stop if the await exists
            stopWhenPossibleTcs = new TaskCompletionSource<List<float>[]>();
            //return the awaiter's rask
            return stopWhenPossibleTcs.Task;
        }

        // turns the catcher to a specific chamber
        public async Task CatcherTurnToChamber(int chamberId, int speed, int acceleration) {
            // the amount of chambers is actually equal to one more than what is in the array
            // cause there is also the dummy chamber for all the resistors that dont fit into any other chambers within tolerance
            int amountOfChambers = Program.Settings.Chambers.Length + 1;
            //define where exactly to go
            int positionToGoTo = steps / amountOfChambers;
            // this here is best described as an examble
            // steps is 400
            // current position is 399
            // we need to go to 1 (which is equivalent, but the code just sees it as 1)
            // there is abselutly no reason to go from 399 to 1, when going from 399 to 401 is faster
            // this checks if the position to go to father away from the current position than the position to go + steps is from the position
            // basically which way around is the fastest
            if (Math.Abs(positionToGoTo - catcherPosition) > Math.Abs((positionToGoTo + steps) - catcherPosition)) {
                // if its the other way around thats the fastest, add the amount of steps to it
                positionToGoTo += steps;
            }
            // turn the thing that many steps to that position
            await CatcherTurn(positionToGoTo - catcherPosition, speed, acceleration);
        }
        public async Task CatcherTurn(int steps, int speed, int acceleration) {
            // ask the JSFNCOMHandler to nicely tell the arduino to turn the catcher
            await ComHandler.SendFunction("catcherTurn", new object[] { steps, speed, acceleration });
            // add the amount of steps turned to the out position variable
            catcherPosition += steps;
            // make sure its not larger than the amount of steps, if it scale it dont (kinda like angles)
            catcherPosition = catcherPosition % this.steps;
            // if its negative, then dont (kinda like angles again)
            catcherPosition = (catcherPosition < 0 ? this.steps : 0) + catcherPosition;
        }
        //then the transporter
        public Task TransporterTurn(int steps, int speed, int acceleration) {
            //ask the JSFNCOMHandler to nicely tell the arduino to turn the catcher
            return ComHandler.SendFunction("transporterTurn", new object[] { steps, speed, acceleration });
        }
        public Task OhmArmPosition(int angle) {
            //ask the JSFNCOMHandler to nicely tell the arduino to turn the ohmmeter arm
            return ComHandler.SendFunction("ohmArmPosition", new object[] { angle });
        }
        public async Task<float?> ReadResistance() {
            //ask the JSFNCOMHandler to nicely tell the arduino to tell the JSFNCOMHandler to tell you the current resistance
            var re = await ComHandler.SendFunction("readResistance", new object[] { });
            // if it can be parsed
            if (float.TryParse((string)re[0], out float result)) {
                // return it
                return result;
            }
            // if it cannot be parsed, return null;
            return null;
        }
    }
}