﻿using ICPC_WebSite_Backend.Data.Models;
using UtilityLibrary.ModelsDTO;
using UtilityLibrary.Response;

namespace ICPC_WebSite_Backend.Repository
{
    public interface IWeekRepository
    {
        Task<Response> AddWeek(WeekDto weekDto);
        Task<Response> UpdateWeek(int weekId, WeekDto weekDto);
        Task<Response<IEnumerable<Week>>> GetWeeksByCommunity(int communityId);
        Task<Response<IEnumerable<Week>>> GetWeeksByTraining(int trainingId);
        Task<Response<Week>> GetTheWeek(int weekId);
        Task<Response> deleteWeek(int weekId);
        Task<Response> LinkSheet(int weekId, int sheetId);
      
        Task<Response> UnLinkSheet(int weekId, int sheetId);
    }
}
