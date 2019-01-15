using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace resorter {
    class ArduinoCommand {
        public List<object> Arguments;
        public string name;
    }
    class ArduinoComHandler {

        private static Thread CreateReadThread(ArduinoComHandler arduinoComHandler) {
            return new Thread(() => {
                string currentData = "";
                while (arduinoComHandler.continueReadThread) {
                    currentData += arduinoComHandler.serialPort.ReadLine();
                    if (currentData.Contains(";")) {
                        List<string> splitData = currentData.Split(';').ToList();
                        currentData = splitData[splitData.Count - 1];
                        splitData.RemoveAt(splitData.Count - 1);
                        splitData.ForEach(command => {
                            arduinoComHandler.OnDataRecived(command);
                        });
                    }
                }
            });
        }

        public static ArduinoCommand ParseArduinoCommand(string rawCommand) {
            int indexArgumentStart = rawCommand.IndexOf('(');
            string functionName = rawCommand.Substring(0, indexArgumentStart);
            List<string> SplitArgumentString(string argumentString) {
                return argumentString.SplitWithin(',', new char[] { '"', '"' }, new char[] { '[', ']' })
                .Select(x => x.Trim()).ToList();
            }
            object ParseArgument(string argument) {
                //string parsing
                if (argument.Substring(0, 1) == "\"" && argument.Substring(argument.Length - 1, argument.Length) == "\"") {
                    return argument.Substring(1, argument.Length - 1);
                }
                //array parsing
                if (argument.Substring(0, 1) == "[" && argument.Substring(argument.Length - 1, argument.Length) == "]") {
                    return SplitArgumentString(argument.Substring(1, argument.Length - 1)).Select(ParseArgument).ToArray();
                }
                //float parsing
                if (argument.Contains(".") && float.TryParse(argument, out float floatresult)) {
                    return floatresult;
                }
                //int parsing
                if (int.TryParse(argument, out int intresult)) {
                    return intresult;
                }
                return null;
            }
            List<object> arguments = SplitArgumentString(rawCommand.Substring(indexArgumentStart + 1, rawCommand.Length - 1)).Select(ParseArgument).ToList();
            return new ArduinoCommand() {
                Arguments = arguments,
                name = functionName,
            };
        }

        private SerialPort serialPort;
        private bool continueReadThread = true;
        private Thread readThread;

        public void OnDataRecived(string data) {
            Task.Factory.StartNew(() => {
                ArduinoCommand command = ParseArduinoCommand(data);
            });
        }

        public ArduinoComHandler(string comPort) {
            serialPort = new SerialPort {
                PortName = comPort,
                BaudRate = 9600,
            };
            serialPort.Open();
            readThread = CreateReadThread(this);
        }
    }
}
