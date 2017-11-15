using System;
using System.Collections.Generic;
using System.Text;

namespace RecipesCore.Services
{
    public interface IWordStatisticsService
    {
        List<Tuple<string, int>> GetCookingTimeAndFreq();
    }
}
