﻿using System;

namespace resorter {
    [Serializable]
    public class Settings {
        public static Settings StandartSettings { get; } = new Settings() {
            Chambers = new int[] { 1000, 10000, 100000, 1000000 },
            Tolerance = 10,
            ToleranceIsPercentage = true,
        };

        public int[] Chambers { get; set; }
        public int Tolerance { get; set; }
        public bool ToleranceIsPercentage { get; set; }
    }
}
