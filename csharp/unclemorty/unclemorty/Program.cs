using System;
using System.Linq;
using System.Collections.Generic;

namespace unclemorty
{
    public class LottoPick {
        private static LottoPick mInstance = null;

        public static LottoPick GetInstance() {
            if (mInstance == null) {
                mInstance = new LottoPick();
            }
            return mInstance;
        }

        private LottoPick() {}

        /*
         * Lotto Pick algorithm
         * - get permutations of input number string in 1 and 2 digit numbers.
         * - uses recursion and yield to walk through all possible combinations of splits.
         * - filter out non-lotto sequence, including 7 numbers only, duplicates, and numbers from 1 to 59.
         * @param inputStr - lotto number sequence to check for possible valid lotto pick combinations
         * @return - 1 list containing 7 numbers of a lotto pick.
         */
		public List<string> GetLottoPick(string inputStr)
		{
			int debug = 0;
			int numInASet = 7;
			int minStrLen = numInASet * 1;
			int maxStrLen = numInASet * 2;
			int minLottoNum = 1;
			int maxLottoNum = 59;

			int inputLen = inputStr.Length;

			// validate if string length matches basic criteria for getting a single successful pick combination
			if (inputLen < minStrLen || inputLen > maxStrLen)
			{
				return null;
			}

			// calculate permutation of all possible single and double digit combinations
			var perm = GetCombinations(inputStr);
			// filter out combinations resulting not exactly 7 unique numbers (as required by Lotto pick)
			perm = perm.Where(r => r.Count() == 7).Select(r => r);
			if (debug == 1) Debug(perm);
			if (perm == null || perm.Count() <= 0) return null;

			// filter out combinations that contain duplicated number in the pick
			perm = perm.Where(r => r.GroupBy(n => n).All(c => c.Count() == 1)).Select(r => r);
			if (debug == 1) Debug(perm);
			if (perm == null || perm.Count() <= 0) return null;

			// the split algorithm cannot decern the difference between a 2 digit number string
			// starting with zero being illegal. Therefore a filter is needed to filter out list containing item pattern "0X"
			perm = from r in perm
				   where r.All(s => int.Parse(s) >= minLottoNum && int.Parse(s) <= maxLottoNum && s.ElementAt(0) != '0')
				   select r;

			if (debug == 1) Debug(perm);
			if (perm == null || perm.Count() <= 0) return null;

			// return only 1 solution, pick the first one
			// note: if there are additional requirements, such as maximize number of double digits in a pick etc
			// we can add additional filter to achieve that here


			return perm.ElementAt(0);
		}

		public void Debug(IEnumerable<List<string>> l)
		{
			foreach (var r in l)
			{
				Console.WriteLine(string.Join(",", r));
			}
		}

		private IEnumerable<List<string>> GetCombinations(string input)
		{
			return Split(input, new List<string>());
		}

        /*
         *  This function will use C# yield to return all possible combinations of 
         *  string, once it is splitted in single or double digit
         */
		private IEnumerable<List<string>> Split(string input, List<string> current)
		{
            // when the string finishes splitting
			if (input.Length == 0)
			{
				yield return current;
			}

            // first try to split numbers into single digit numbers
			if (input.Length >= 1)
			{
				var copy = current.ToList();
				copy.Add(input.Substring(0, 1));
				foreach (var r in Split(input.Substring(1), copy))
					yield return r;
			}

            // then try to split numbers into 2 digit numbers
			if (input.Length >= 2)
			{
				var copy = current.ToList();
				copy.Add(input.Substring(0, 2));
				foreach (var r in Split(input.Substring(2), copy))
					yield return r;
			}
		}
    }

	public class MainClass
	{
		public static void Main(string[] args)
		{
			string[] possiblePicks = new string[] { "569815571556", "4938532894754", "1234567", "472844278465445" };

			Console.WriteLine("Standard test cases");
			foreach (string inputStr in possiblePicks)
			{
				List<String> lottoPick = LottoPick.GetInstance().GetLottoPick(inputStr);
				if (lottoPick != null)
				{
					Console.WriteLine(inputStr + " -> " + string.Join(",", lottoPick));
				}
			}

			
            string[] possiblePicks2 = new string[]{ "101122241229", "1011222412209", "1011202241229", "1011222401229" };

            Console.WriteLine("\nAdditional test cases, with 0");
            foreach (string inputStr in possiblePicks2) {
                List<String> lottoPick = LottoPick.GetInstance().GetLottoPick(inputStr);
                if (lottoPick != null) {
                    Console.WriteLine(inputStr + " -> " + string.Join(",", lottoPick));
                }               
            }           

            string[] possiblePicks3 = new string[]{ "0101122241229", "10011222412209", "111222412290", "10012345", "12345607" };

            Console.WriteLine("\nAdditional negative test cases, with 0");
            foreach (string inputStr in possiblePicks3) {
                List<String> lottoPick = LottoPick.GetInstance().GetLottoPick(inputStr);
                if (lottoPick != null) {
                    Console.WriteLine(inputStr + " -> " + string.Join(",", lottoPick));
                }               
            }

		}

	}
}
