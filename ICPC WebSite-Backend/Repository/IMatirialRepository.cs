using ICPC_WebSite_Backend.Models;
using ICPC_WebSite_Backend.Models.DTO;
using ICPC_WebSite_Backend.Data.ReturnObjects.Models;
using ICPC_WebSite_Backend.Data.Models.DTO;

namespace ICPC_WebSite_Backend.Repository
{
    public interface IMatirialRepository
    {
        Task<Response> addMatirial(MatirialDTO matirialDTO);
        Task<Response> deleteMatiral(int materialId);
        Task<Response> getWeekMaterials(int weekId);
        Task<Response> updateMaterial(int materialId, MatirialDTO matirialDTO);
    }
}
