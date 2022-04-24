using ICPC_WebSite_Backend.Data;
using ICPC_WebSite_Backend.Data.Models;
using ICPC_WebSite_Backend.Data.Models.DTO;
using ICPC_WebSite_Backend.Data.ReturnObjects.Models;
using ICPC_WebSite_Backend.Utility;

namespace ICPC_WebSite_Backend.Repository
{
    public class WeekRepository : IWeekRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public WeekRepository(ApplicationDbContext applicationDbContext) {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Response> AddWeek(WeekDTO weekDTO) {
            var ret = new Response();

            var week = new Week() {
                Name = weekDTO.Name,
                Description = weekDTO.Description,
                IsTemplate = weekDTO.IsTemplate,
                Created_at = DateTime.Now
            };
            await _applicationDbContext.weeks.AddAsync(week);
            await _applicationDbContext.SaveChangesAsync();
            return ret;
        }
        public async Task<Response> UpdateWeek(int weekId, WeekDTO weekDTO) {
            var ret = new Response();
            var week = await _applicationDbContext.weeks.FindAsync(weekId);
            
            if (week != null) {
                week.Description = weekDTO.Description;
                week.IsTemplate = weekDTO.IsTemplate;
                week.Name = weekDTO.Name;
                week.Updated_at = DateTime.Now;
                await _applicationDbContext.SaveChangesAsync();
            }
            else {
                ret.Succeeded = false;
                ret.Errors.Add(ErrorsList.WeekNotFound);
            }
            return ret;
        }
        public async Task<Response> GetAllTemplateWeeks() {
            var ret = new Response();
            var weeks = _applicationDbContext.weeks.Where(W => W.IsTemplate == true)
               .Select(week => new Week() {
                   Id = week.Id,
                   IsTemplate = week.IsTemplate,
                   Description = week.Description,
                   Name = week.Name
               }).ToList();
            ret.Data = weeks;
            return ret;
        }
        public async Task<Response> GetAllWeeks() {
            var ret = new Response();
            var weeks = _applicationDbContext.weeks
               .Select(week => new Week() {
                   Id = week.Id,
                   IsTemplate = week.IsTemplate,
                   Description = week.Description,
                   Name = week.Name
               }).ToList();
            ret.Data = weeks;
            return ret;
        }
        public async Task<Response> GetTheWeek(int weekId) {
            var ret = new Response();
            var week = await _applicationDbContext.weeks.FindAsync(weekId);
            if (week == null) {
                ret.Succeeded = false;
                ret.Errors.Add(ErrorsList.WeekNotFound);
            }
            else {
                ret.Data = new Week() {
                    Id = week.Id,
                    Name = week.Name,
                    Description = week.Description,
                    IsTemplate = week.IsTemplate
                };
            }
            return ret;
        }
        public async Task<Response> createTemplateWeek(int weekId) {
            var ret = new Response();
            var week = await _applicationDbContext.weeks.FindAsync(weekId);
            if (week != null) {
                var newWeek = new Week() {
                    Created_at = DateTime.Now,
                    Description = week.Description,
                    Name = week.Name,
                    IsTemplate = false

                };
                await _applicationDbContext.weeks.AddAsync(newWeek);
                await _applicationDbContext.SaveChangesAsync();
                var weekMateriales = (_applicationDbContext.matirials.Where(m => m.weekId == weekId)).ToList();
                foreach (var material in weekMateriales) {
                    await _applicationDbContext.matirials.AddAsync(new Matirial() {
                        Created_at = DateTime.Now,
                        Description = material.Description,
                        weekId = newWeek.Id,
                        URL = material.URL
                    });
                    await _applicationDbContext.SaveChangesAsync();
                }
            }
            else {
                ret.Succeeded = false;
                ret.Errors.Add(ErrorsList.WeekNotFound);
            }
            return ret;
        }

    }
}
