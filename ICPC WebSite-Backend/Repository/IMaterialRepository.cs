using ICPC_WebSite_Backend.Data.Models;
using UtilityLibrary.ModelsDTO;
using UtilityLibrary.Response;

namespace ICPC_WebSite_Backend.Repository;

public interface IMaterialRepository
{
    Task<Response<Material>> AddMaterial(MaterialDto materialDto);
    Task<Response> DeleteMaterial(int materialId);
    Task<Response<IEnumerable<Material>>> GetWeekMaterials(int weekId);
    Task<Response> UpdateMaterial(int materialId, MaterialDto materialDto);
}