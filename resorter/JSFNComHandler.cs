using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace resorter {
    // a class to hold the information in a JSFN function
    public class JSFNFunctionSchema {
        public List<object> Arguments;
        public string name;
    }
    public class JSFNComHandler {

        // create the read thread with the com handler as the argument
        private static Thread CreateReadThread(JSFNComHandler jsfnHandler) {
            // create a new thread, and return it
            return new Thread(() => {
                // the new thread will loop until jsfnHandler.continueReadThread is changed to false
                while (jsfnHandler.continueReadThread) {
                    // continue to read data received by the serial port, output the data when hitting an ";"
                    string readData = jsfnHandler.serialPort.ReadTo(";");
                    // call the OnDataRecived function on jsfnHandler with the data as the argument
                    jsfnHandler.OnDataRecived(null, readData.Trim());
                };
            });
        }

        // parse raw text from the arduino
        public static JSFNFunctionSchema ParseArduinoCommand(string rawCommand) {
            // sometimes we recive nothing in the strings for some reason *shrug*
            if (rawCommand == "") {
                return null;
            }
            // get the index of where in the string the "(" is
            int indexArgumentStart = rawCommand.IndexOf('(');
            // get the part of the string from 0 til "("
            string functionName = rawCommand.Substring(0, indexArgumentStart);
            // this function converts the string inside (), into a list of strings as the arguments
            List<string> SplitArgumentString(string argumentString) {
                return argumentString.SplitWithin(',', new char[] { '"', '"' }, new char[] { '[', ']' })
                .Select(x => x.Trim()).ToList();
            }
            // function to parse individual arguments
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
            // get the raw argument string inside the ()
            string rawArguments = rawCommand.Substring(indexArgumentStart + 1, rawCommand.Length - (indexArgumentStart + 2));
            // parse the whole shit with the previous functions
            List<object> arguments = SplitArgumentString(rawArguments).Select(ParseArgument).ToList();
            // return a new object with the whole thing parsed
            return new JSFNFunctionSchema() {
                Arguments = arguments,
                name = functionName,
            };
        }

        // to hold the object for the serial port
        private SerialPort serialPort;
        // variable to check if we should stop the read thread
        private bool continueReadThread = true;
        // the read thread
        private Thread readThread;

        // event for when we recive data from the read thread
        public event EventHandler<string> OnDataRecived;

        // function that other parts of the program use to send functions to the arduino
        public async Task<List<object>> SendFunction(string func, object[] args) {
            // function to construct the raw arguments string from a list of objects
            string constructArgs(object[] a) {
                return string.Join(",", args.Select(x => {
                    // if x is a string 
                    if (x is string) {
                        return "\"" + x + "\"";
                    }
                    // if x is an array
                    if (x.GetType().IsArray) {
                        // do this recursively
                        return "[" + constructArgs((object[])x) + "]";
                    }
                    return x;
                }));
            }
            // create the string to send
            string sendable = func + "(" + constructArgs(args) + ");";

            Console.WriteLine("constructed JSFN function: " + sendable);

            // function to check if we can send the function
            async Task checkForExistingFunction() {
                // if we are waiting for a return with the same function name as the one we are currently trying to send
                if (SendFunctionReturns.ContainsKey(func)) {
                    Console.WriteLine("waiting for return on function: " + func);
                    // create an awaiter
                    TaskCompletionSource<bool> tsc = new TaskCompletionSource<bool>();
                    // when we recive data;
                    void onDataRecived(object _, string data) {
                        // release awaiter
                        tsc.SetResult(true);
                    }
                    // link onDataRecived to when we recive data;
                    OnDataRecived += onDataRecived;
                    // wait for awaiter
                    await tsc.Task;
                    // unlink function from recive data;
                    OnDataRecived -= onDataRecived;
                    // do this function recursivly
                    await checkForExistingFunction();
                }
            }

            // start the prevoius function
            await checkForExistingFunction();

            //we are now sure that we can send

            Console.WriteLine("writing function to arduino: " + sendable);

            // create awaiter;
            TaskCompletionSource<List<object>> tcs = new TaskCompletionSource<List<object>>();
            // add awaiter to directory of functions we are waiting for
            SendFunctionReturns.Add(func, tcs);
            // set the last command variable
            lastCommand = sendable;
            // actually send the things
            serialPort.Write(sendable);
            // return once we have waited for awaiters
            return await tcs.Task;
        }

        private string lastCommand = "";

        // directory to host the functons we are waiting for and the corresponding awaiter
        private Dictionary<string, TaskCompletionSource<List<object>>> SendFunctionReturns = new Dictionary<string, TaskCompletionSource<List<object>>>();

        // directory of function that the arduino can call on this machine
        private Dictionary<string, Func<List<object>, object>> JSFNFunctions;

        //constructor
        public JSFNComHandler(string comPort, Dictionary<string, Func<List<object>, object>> funcs) {
            // add error to function list
            funcs.Add("error", new Func<List<object>, object>((List<object> a) => {
                // send last command
                serialPort.Write(lastCommand);
                return null;
            }));
            // set the directory of function the arduino has access to
            JSFNFunctions = funcs;
            // link to the ondatarecived function
            OnDataRecived += (object _, string data) => {
                {
                    //sometimes there is a linebreak in the input string for some reason *shrug*
                    //split the string where ever the linebreak and only take the last part
                    var a = data.Split('\n');
                    data = a[a.Length - 1];
                }
                // try and parse the data recived
                JSFNFunctionSchema command = null;
                try {
                    command = ParseArduinoCommand(data);
                }
                catch (Exception) { }
                // if the parsing failed, do nothing and log it
                if (command == null) {
                    Console.WriteLine("couldnt parse: " + data);
                    return;
                }

                Console.WriteLine("received function: " + data);

                // if we are awaiting a function of the name we recived
                if (SendFunctionReturns.ContainsKey(command.name)) {
                    Console.WriteLine("found matching return call for function");
                    // resovle the awaiter
                    SendFunctionReturns[command.name].SetResult(command.Arguments);
                    // remove the entry from the directory
                    SendFunctionReturns.Remove(command.name);
                }
                // if the name matches one of the names of the functions the arduino has available
                else if (JSFNFunctions.ContainsKey(command.name)) {
                    Console.WriteLine("found matching function");
                    // do the function that the arduino requested
                    JSFNFunctions[command.name](command.Arguments);
                }
                // otherwise log
                else {
                    Console.WriteLine("no matching function for: " + data);
                }
            };
            // create serial port
            serialPort = new SerialPort {
                PortName = comPort,
                BaudRate = 9600,
            };
            // set timeout
            serialPort.WriteTimeout = 10;
            // start
            serialPort.Open();
            //create read thread 
            readThread = CreateReadThread(this);
            // start read thread
            readThread.Start();
        }
    }
}
