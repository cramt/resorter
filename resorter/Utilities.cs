using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace resorter {
    public static class Utilities {
        public static List<string> SplitWithin(this string str, char splittingChar, params char[][] p) {
            bool[] triggeredP = new bool[p.Length];
            List<int> splitPointsList = new List<int>();
            splitPointsList.Add(-1);
            for (int i = 0; i < str.Length; i++) {
                for (int j = 0; j < p.Length; j++) {
                    if (p[j][0] == p[j][1]) {
                        if (p[j][0] == str[i]) {
                            triggeredP[j] = !triggeredP[j];
                        }
                    }
                    else {
                        if (p[j][0] == str[i]) {
                            triggeredP[j] = true;
                        }
                        if (p[j][1] == str[i]) {
                            triggeredP[j] = false;
                        }
                    }
                }
                if (!triggeredP.Any(x => x)) {
                    if (str[i] == splittingChar) {
                        splitPointsList.Add(i);
                    }
                }
            }
            splitPointsList.Add(str.Length);
            int[] splitPoints = splitPointsList.ToArray();
            List<string> re = new List<string>();
            for (int i = 0; i < splitPoints.Length - 1; i++) {
                int a = splitPoints[i] + 1;
                int b = splitPoints[i + 1];
                re.Add(str.Substring(a, b - a));
            }
            return re;
        }
    }
}
