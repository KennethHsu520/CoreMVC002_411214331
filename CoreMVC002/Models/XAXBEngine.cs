namespace CoreMVC002.Models
{
    // 單次猜測紀錄結構
    public class GuessRecord
    {
        public string GuessNumber { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
    }

    public class XAXBEngine
    {
        // 答案數字
        public string TargetAnswer { get; set; } = string.Empty;
        // 歷次猜測紀錄
        public List<GuessRecord> History { get; set; } = new List<GuessRecord>();
        // 是否已猜中
        public bool IsGameOver { get; set; } = false;

        // 初始化/重新開始遊戲
        public void RestartGame()
        {
            Random rand = new Random();
            List<int> numbers = Enumerable.Range(0, 10).OrderBy(x => rand.Next()).ToList();
            // 產生 4 位不重複數字（若第一位是 0 亦可，或依習慣處理）
            TargetAnswer = string.Join("", numbers.Take(4));
            History.Clear();
            IsGameOver = false;
        }

        // 執行猜數字比對
        public string CheckGuess(string guess)
        {
            if (string.IsNullOrEmpty(guess) || guess.Length != 4)
            {
                return "請輸入正確的 4 位數字！";
            }

            int a = 0, b = 0;
            for (int i = 0; i < 4; i++)
            {
                if (guess[i] == TargetAnswer[i])
                {
                    a++;
                }
                else if (TargetAnswer.Contains(guess[i]))
                {
                    b++;
                }
            }

            string result = $"{a}A{b}B";

            // 紀錄歷次結果
            History.Add(new GuessRecord
            {
                GuessNumber = guess,
                Result = result
            });

            if (a == 4)
            {
                IsGameOver = true;
            }

            return result;
        }
    }
}