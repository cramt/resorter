using System;

namespace resorter {
    [Serializable]
    public class Settings {
        //the standart settings as in the settings you have when you start the program for the first time
        public static Settings StandartSettings { get; } = new Settings() {
            //                                     ^ this means this thing CAN NEVER be set, only gotten
            Chambers = new int[] { 1000, 10000, 100000 },
            Tolerance = 10,
            ToleranceIsPercentage = true,
            Steps = 200,
        };
        // the amount of chambers (there are 4 chambers in the real world,
        // but since one of them is just "trash" chamber for resistors not within tolerance of the other
        // thats one will be irrelevant for the settings
        public int[] Chambers { get; set; }
        // the actual tolerance we're working with
        public int Tolerance { get; set; }
        // is the unit of the tolerance percentage (if its not, its ohm)
        public bool ToleranceIsPercentage { get; set; }
        // the amount of steps the stepper moter has
        public int Steps { get; set; }
    }
}
