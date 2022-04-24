using ICPC_WebSite_Backend.Data;
using ICPC_WebSite_Backend.Data.Models;
using ICPC_WebSite_Backend.Data.Models.DTO;
using ICPC_WebSite_Backend.Data.ReturnObjects.Models;
using ICPC_WebSite_Backend.Utility;

namespace ICPC_WebSite_Backend.Repository
{
    public class MatirialRepository : IMatirialRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public MatirialRepository(ApplicationDbContext applicationDbContext) {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Response> addMatirial(MatirialDTO matirialDTO) {
            var ret = new Response();
           
            var week =await _applicationDbContext.weeks.FindAsync(matirialDTO.weekId);
            if (week != null) {
                await _applicationDbContext.SaveChangesAsync();
                var matirial = new Matirial() {
                    Created_at = DateTime.Now,
                    URL = matirialDTO.URL,
                    Description = matirialDTO.Description,
                    weekId=matirialDTO.weekId,
                };
                await _applicationDbContext.matirials.AddAsync(matirial);
               await _applicationDbContext.SaveChangesAsync();
             
            }
            else {
                ret.Succeeded = false;
                ret.Errors.Add(ErrorsList.WeekNotFound);
            }
            return ret;
        }
        public async Task<Response> deleteMatiral(int materialId) {
            var ret = new Response();
            var material = await _applicationDbContext.matirials.FindAsync(materialId);
            if(material!= null) {
                 _applicationDbContext.Remove(material);
                await _applicationDbContext.SaveChangesAsync();
            }
            else {
                ret.Succeeded = false;
                ret.Errors.Add(ErrorsList.MaterailNotFound);
            }
            return ret;

        }
        public async Task<Response> getWeekMaterials(int weekId) {
            var ret = new Response();
            var _materials = (_applicationDbContext.matirials.Where(m => m.weekId == weekId)).
                Select(material => new Matirial {
                    Id = material.Id,
                    Description = material.Description,
                    URL = material.URL
                }).ToList();
            ret.Data = _materials;
            return ret;
        }
        public async Task<Response> updateMaterial(int materialId, MatirialDTO matirialDTO) {
            var ret = new Response();
            var material = await _applicationDbContext.matirials.FindAsync(materialId);
            if (material != null) {
               material.Description = matirialDTO.Description;
               material.URL = matirialDTO.URL;
               await _applicationDbContext.SaveChangesAsync();
            }
            else {
                ret.Succeeded = false;
                ret.Errors.Add(ErrorsList.MaterailNotFound);
            }
            return ret;
        }

    }
}
