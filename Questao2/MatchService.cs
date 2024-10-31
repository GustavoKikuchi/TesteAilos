using Questao2.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Questao2
{
    public class MatchService
    {
        private const string BASE_URL = "https://jsonmock.hackerrank.com/api/football_matches";

        public static QueryResponse GetMatchesByTeamsYearAndPage(string team1, string team2, int year, int page)
        {
            using (var httpCLient = new HttpClient())
            {
                var response = httpCLient.GetStringAsync($"{BASE_URL}{GetParameters(team1, team2, year, page)}").Result;
                return JsonSerializer.Deserialize<QueryResponse>(response);
            }
        }

        private static string GetParameters(string team1, string team2, int year, int page)
        {
            string parameters = $"?page={page}&year={year}";
            
            if (!string.IsNullOrEmpty(team1))
                parameters += $"&team1={team1}";
            
            if (string.IsNullOrEmpty(team2))
                parameters += $"&team2={team2}";
            
            return parameters;
        }
    }
}
