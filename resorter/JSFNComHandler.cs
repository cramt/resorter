using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace resorter {
    class JSFNFunctionSchema {
        public List<object> Arguments;
        public string name;
    }
    class JSFNComHandler {

        private static Thread CreateReadThread(JSFNComHandler jsfnHandler) {
            return new Thread(() => {
                while (jsfnHandler.continueReadThread) {
                    var a = jsfnHandler.serialPort.ReadTo(";");
                    jsfnHandler.OnDataRecived(null, a.Trim());
                };
            });
        }

        public static JSFNFunctionSchema ParseArduinoCommand(string rawCommand) {
            if(rawCommand == "") {
                return null;
            }
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
            var a = rawCommand.Substring(indexArgumentStart + 1,  rawCommand.Length - (indexArgumentStart+1));
            List<object> arguments = SplitArgumentString(a).Select(ParseArgument).ToList();
            return new JSFNFunctionSchema() {
                Arguments = arguments,
                name = functionName,
            };
        }

        private SerialPort serialPort;
        private bool continueReadThread = true;
        private Thread readThread;

        public event EventHandler<string> OnDataRecived;

        public async Task<List<object>> SendFunction(string func, object[] args) {
            string constructArgs(object[] a) {
                return string.Join(",", args.Select(x => {
                    if (x.GetType().GUID == typeof(string).GUID) {
                        return "\"" + x + "\"";
                    }
                    if (x.GetType().IsArray) {
                        return "[" + constructArgs((object[])x) + "]";
                    }
                    return x;
                }));
            }
            string constructSendable() {
                return func + "(" + constructArgs(args) + ");";
            }

            if (SendFunctionReturns.ContainsKey(func)) {

                TaskCompletionSource<bool> tsc = new TaskCompletionSource<bool>();
                void onDataRecived(object _, string data) {
                    tsc.SetResult(true);
                }
                OnDataRecived += onDataRecived;
                await tsc.Task;
                OnDataRecived -= onDataRecived;
            }
            TaskCompletionSource<List<object>> tcs = new TaskCompletionSource<List<object>>();
            SendFunctionReturns.Add(func, tcs);
            serialPort.Write(constructSendable());
            return await tcs.Task;
        }

        private Dictionary<string, TaskCompletionSource<List<object>>> SendFunctionReturns = new Dictionary<string, TaskCompletionSource<List<object>>>();

        private Dictionary<string, Func<List<object>, object>> JSFNFunctions;

        public JSFNComHandler(string comPort, Dictionary<string, Func<List<object>, object>> funcs) {
            JSFNFunctions = funcs;
            OnDataRecived += (object _, string data) => {
                Task.Factory.StartNew(() => {
                    JSFNFunctionSchema command = null;
                    try {
                        command = ParseArduinoCommand(data);
                    }
                    catch (Exception) { }
                    if(command == null) {
                        return;
                    }

                    if (SendFunctionReturns.ContainsKey(command.name)) {
                        SendFunctionReturns[command.name].SetResult(command.Arguments);
                        SendFunctionReturns.Remove(command.name);
                    }
                    else if (JSFNFunctions.ContainsKey(command.name)) {
                        JSFNFunctions[command.name](command.Arguments);
                    }
                });
            };
            serialPort = new SerialPort {
                PortName = comPort,
                BaudRate = 9600,
            };
            serialPort.WriteTimeout = 10;
            serialPort.Open();

            readThread = CreateReadThread(this);
            readThread.Start();
        }
    }
}
