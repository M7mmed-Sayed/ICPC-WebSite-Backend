using ICPC_WebSite_Backend.Data.Models;
using ICPC_WebSite_Backend.Data.Models.DTO;
using ICPC_WebSite_Backend.Data.Response;

namespace ICPC_WebSite_Backend.Repository;

public interface IMaterialRepository
{
    Task<Response> addMaterial(MaterialDTO MaterialDTO);
    Task<Response> deleteMaterial(int materialId);
    Task<Response<IEnumerable<Material>>> getWeekMaterials(int weekId);
    Task<Response> updateMaterial(int materialId, MaterialDTO MaterialDTO);
}