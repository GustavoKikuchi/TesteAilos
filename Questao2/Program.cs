using Newtonsoft.Json;
using Questao2;
using Questao2.DTO;

public class Program
{
    public static void Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static int getTotalScoredGoals(string team, int year)
    {
        return GetTeam1TotalGoals(team, year) + GetTeam2TotalGoals(team, year);
    }

    private static int GetTeam1TotalGoals(string team, int year)
    {
        int page = 1;
        int total = 0;
        int totalPages;
        do
        {
            var query = MatchService.GetMatchesByTeamsYearAndPage(team, team2: null, year, page);
            if (query.total == 0)
                return total;

            totalPages = query.total_pages;

            total += GetTotalGoalsFromMatches(query.data, team);

            page++;

        } while (page <= totalPages);

        return total;
    }


    private static int GetTeam2TotalGoals(string team, int year)
    {
        int page = 1;
        int total = 0;
        int totalPages;
        do
        {
            var query = MatchService.GetMatchesByTeamsYearAndPage(team1: null, team, year, page);
            if (query.total == 0)
                return total;

            totalPages = query.total_pages;

            total += GetTotalGoalsFromMatches(query.data, team);

            page++;

        } while (page <= totalPages);

        return total;
    }

    private static int GetTotalGoalsFromMatches(List<MatchResponse> data, string team)
    {
        int totalGoals = 0;
        foreach (var match in data)
        {
            if (match.team1.ToUpper() == team.ToUpper())
                totalGoals += Convert.ToInt32(match.team1goals);
            else
                totalGoals += Convert.ToInt32(match.team2goals);
        }
        return totalGoals;
    }
}