using System;

namespace resorter {
    [Serializable]
    public class Settings {
        public static Settings StandartSettings { get; } = new Settings() {
            Chambers = new int[] { 1000, 10000, 100000 },
            Tolerance = 10,
            ToleranceIsPercentage = true,
            Steps = 200,
        };

        public int[] Chambers { get; set; }
        public int Tolerance { get; set; }
        public bool ToleranceIsPercentage { get; set; }
        public int Steps { get; set; }
    }
}
