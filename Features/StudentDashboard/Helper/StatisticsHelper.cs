namespace ExaminationSystem.Features.StudentDashboard.Helper
{
    public static class StatisticsHelper
    {
        public static decimal CalculatePassRate(int total, int passedCount)
        {
            if (total == 0)
                return 0;

            return Math.Round((decimal)passedCount / total * 100, 1);
        }

        public static decimal RoundScore(decimal score)
        {
            return Math.Round(score, 1);
        }

        public static decimal CalculateProgress(int total, int completed)
        {
             return total > 0 ? 
                Math.Round((decimal)completed / total * 100, 1) 
                : 0;
        }
    }
}
