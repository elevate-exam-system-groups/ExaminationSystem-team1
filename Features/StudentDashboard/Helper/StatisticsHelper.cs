namespace ExaminationSystem.Features.StudentDashboard.Helper
{
    public static class StatisticsHelper
    {
        public static decimal CalculatePercentage(int total, int part)
        {
            return total > 0 ?
                Math.Round((decimal)part / total * 100, 1)
                : 0;
        }

        public static decimal RoundScore(decimal score)
        {
            return Math.Round(score, 1);
        }

      
    }
}
