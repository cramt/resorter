using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace resorter {
    public class ResorterStateHandler {
        public JSFNComHandler ComHandler;
        private int steps;
        private int catcherPosition = 0;
        private int transporterPosition = 0;
        private List<float>[] listOfResistors = new List<float>[] {
            new List<float>(),
            new List<float>(),
            new List<float>(),
            new List<float>()
        };
        public ResorterStateHandler(string port, int steps) {
            Dictionary<string, Func<List<object>, object>> jsfnFuncs = new Dictionary<string, Func<List<object>, object>>();
            jsfnFuncs.Add("print", new Func<List<object>, object>((List<object> l) => {
                Console.WriteLine(string.Join(",", l.ToArray()));
                return null;
            }));
            ComHandler = new JSFNComHandler(port, jsfnFuncs);
            this.steps = steps;
        }

        public async void Start() {
            const int ohmArmStandartPos = 45;
            const int ohmArmThrowPos = 155;
            const int ohmArmReadPos = 0;
            while (true) {
                Console.WriteLine("turning transporter wheel");
                await TransporterTurn(steps / 2, 60, 20);
                Console.WriteLine("turning ohm arm to read resistance");
                await OhmArmPosition(ohmArmReadPos);
                Console.WriteLine("reading resistance");
                float? _res = await ReadResistance();
                Console.WriteLine("resistance: " + _res);
                if (_res == null) {
                    Console.WriteLine("didnt receive any resistance, resetting and trying again");
                    await OhmArmPosition(ohmArmThrowPos);
                    Thread.Sleep(100);
                    await OhmArmPosition(ohmArmStandartPos);
                    continue;
                }
                float res = (float)_res;
                float tolerance = (Program.Settings.ToleranceIsPercentage ? res : 1) * Program.Settings.Tolerance;
                int bestMatching = 0;
                for (int i = 0; i < Program.Settings.Chambers.Length; i++) {
                    if (Math.Abs(Program.Settings.Chambers[bestMatching] - res) > Math.Abs(Program.Settings.Chambers[i] - res)) {
                        bestMatching = i;
                    }
                }
                bool withinRange = res - tolerance < Program.Settings.Chambers[bestMatching] && res + tolerance > Program.Settings.Chambers[bestMatching];
                if (!withinRange) {
                    bestMatching = Program.Settings.Chambers.Length + 1;
                }
                Console.WriteLine("resistance is " + (withinRange ? "" : "not") + "within range");
                Console.WriteLine("adding " + res + " in chamber " + bestMatching + " to registry");
                listOfResistors[bestMatching].Add(res);
                Console.WriteLine("turning catcher wheel to chamber: " + bestMatching);
                await CatcherTurnToChamber(bestMatching, 60, 20);
                Console.WriteLine("opening ohm arm");
                await OhmArmPosition(ohmArmThrowPos);
                Thread.Sleep(100);
                Console.WriteLine("resetting ohm arm");
                await OhmArmPosition(ohmArmStandartPos);
                if(stopWhenPossibleTcs != null) {
                    Console.WriteLine("stopping");
                    stopWhenPossibleTcs.SetResult(listOfResistors);
                    return;
                }
            }
        }

        private TaskCompletionSource<List<float>[]> stopWhenPossibleTcs = null;
        public Task<List<float>[]> StopWhenPossible() {
            Console.WriteLine("stopping once current routine is done");
            stopWhenPossibleTcs = new TaskCompletionSource<List<float>[]>();
            return stopWhenPossibleTcs.Task;
        }

        public async Task CatcherTurnToChamber(int chamberId, int speed, int acceleration) {
            int amountOfChambers = Program.Settings.Chambers.Length + 1;
            int positionToGoTo = steps / amountOfChambers;
            if (Math.Abs(positionToGoTo - catcherPosition) > Math.Abs(positionToGoTo + 200 - catcherPosition)) {
                positionToGoTo += 200;
            }
            await CatcherTurn(positionToGoTo - catcherPosition, speed, acceleration);
        }
        public async Task CatcherTurn(int steps, int speed, int acceleration) {
            await ComHandler.SendFunction("catcherTurn", new object[] { steps, speed, acceleration });
            catcherPosition += steps;
            catcherPosition = catcherPosition % this.steps;
            catcherPosition = (catcherPosition < 0 ? this.steps : 0) + catcherPosition;
        }
        public Task TransporterTurn(int steps, int speed, int acceleration) {
            return ComHandler.SendFunction("transporterTurn", new object[] { steps, speed, acceleration });
        }
        public Task OhmArmPosition(int angle) {
            return ComHandler.SendFunction("ohmArmPosition", new object[] { angle });
        }
        public async Task<float?> ReadResistance() {
            var re = await ComHandler.SendFunction("readResistance", new object[] { });
            if (float.TryParse((string)re[0], out float result)) {
                return result;
            }
            return null;
        }
    }
}