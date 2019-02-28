using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace resorter {
    // static class for different function we might end up needing
    public static class Utilities {
        //split within so to split a string but only when the character you spit with (for example, splitting a comma seperated string with a comma) is not within 2 other character (for example, you dont wanna split your comma seperated string with a comma whenever the comma is within a double qoute or single qoute or similar)
        //str is the string we are splitting
        //splittingChar is the char we are splitting in
        //and p is the 2 dimentional array of char's we arent splitting when inbetween
        //the second order of array will always be the size of 2, since there are 2 char's to be between
        public static List<string> SplitWithin(this string str, char splittingChar, params char[][] p) {
            //this variable describes whenever we are inbetween or not
            //this is an array since there are multible char's not split inbetween
            //its called triggeredP cause that specific p is not "trigged" as it is in use, kinda, something
            bool[] triggeredP = new bool[p.Length];
            //this is the indexes in the strng we need to split
            List<int> splitPointsList = new List<int>();
            //this is pretty selfexplainitory of you read last for loop before the return statement
            splitPointsList.Add(-1);
            //this is where we go throught the whole thing and check if we are inbetween or not and write stuff to the list of places we should cut
            //loop throught the string
            for (int i = 0; i < str.Length; i++) {
                //loop thought the p
                for (int j = 0; j < p.Length; j++) {
                    //if the 2 chars we are not splitting inbetween is the same
                    //(like double quote and single qoute)
                    //we will just switch the triggeredP for this index
                    //since when we run into one, it'll then be true and when run into one again it'll then be false and so on and so on
                    if (p[j][0] == p[j][1]) {
                        if (p[j][0] == str[i]) {
                            triggeredP[j] = !triggeredP[j];
                        }
                    }
                    //if it is 2 different chars
                    else {
                        //if the current char we are checking for is equal to the first set the triggeredP to true
                        if (p[j][0] == str[i]) {
                            triggeredP[j] = true;
                        }
                        //and if its the second, well then set it to false
                        if (p[j][1] == str[i]) {
                            triggeredP[j] = false;
                        }
                    }
                }
                //if all of the triggeredP's are false
                if (!triggeredP.Any(x => x)) {
                    //and the the char we are currently checking for is the same as the splitting char
                    if (str[i] == splittingChar) {
                        //add the current index to the list of cuts
                        splitPointsList.Add(i);
                    }
                }
            }
            //add the last point to the list
            splitPointsList.Add(str.Length);
            //make the list of cuts an array (its more efficient to loop throught with a for loop)
            int[] splitPoints = splitPointsList.ToArray();
            //make list of return string where we have cut
            List<string> re = new List<string>();
            //loop throught the cuts
            for (int i = 0; i < splitPoints.Length - 1; i++) {
                //cut from just after our current index
                //(the first one will be 0, since the first index in the splitPointsList was set to -1)
                int a = splitPoints[i] + 1;
                //to the next index
                int b = splitPoints[i + 1];
                //add the substring if where those cuts are, to the return list
                re.Add(str.Substring(a, b - a));
            }
            //return the list of where we cut
            return re;
        }
    }
}
