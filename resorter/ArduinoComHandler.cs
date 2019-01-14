using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace resorter {
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


        private SerialPort serialPort;
        private bool continueReadThread = true;
        private Thread readThread;

        public void OnDataRecived(string data) {
            int indexArgumentStart = data.IndexOf('(');
            string functionName = data.Substring(0, indexArgumentStart);
            List<string> arguments = data.Substring(indexArgumentStart + 1, data.Length - 1).Split(',').Select(x => x.Trim()).ToList();
            object ParseArgument(string argument) {
                //string parsing
                if (argument.Substring(0, 1) == "\"" && argument.Substring(argument.Length - 1, argument.Length) == "\"") {
                    return argument.Substring(1, argument.Length - 1);
                }
                //array parsing
                if (argument.Substring(0, 1) == "[" && argument.Substring(argument.Length - 1, argument.Length) == "]") {
                    return argument.Substring(1, argument.Length - 1);
                }
                return null;
            }
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
