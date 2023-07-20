using System;

public static class RandomHelper
{
    public static DateTime GetRandomDate(int startYear = 2022, int endYear = 2023)
    {
        Random random = new Random();
        DateTime startDate = new DateTime(startYear, 1, 1);
        DateTime endDate = new DateTime(endYear, 12, 31);

        TimeSpan timeSpan = endDate - startDate;
        TimeSpan randomTimeSpan = new TimeSpan(0, random.Next(0, (int)timeSpan.TotalMinutes), 0);

        return startDate + randomTimeSpan;
    }
}
