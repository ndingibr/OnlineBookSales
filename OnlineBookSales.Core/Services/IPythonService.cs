using OnlineBookSales.Core.Entities;
using System.Collections.Generic;

namespace OnlineBookSales.Core
{
    public interface IPythonService
    {
        List<int> GetDailyPredictionAsync(Dictionary<string, string> predictionParameters);
        List<string> GetAttributes();
        int InsertPatrolData();
    }
}
