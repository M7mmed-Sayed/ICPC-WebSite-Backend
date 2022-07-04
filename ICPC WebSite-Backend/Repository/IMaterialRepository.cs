using ICPC_WebSite_Backend.Data.Models;
using UtilityLibrary.ModelsDTO;
using UtilityLibrary.Response;

namespace ICPC_WebSite_Backend.Repository;

public interface IMaterialRepository
{
    /// <summary>
    /// add a material to a week
    /// </summary>
    /// <param name="materialDto"> data of the material like weekId</param>
    /// <returns>returns failed or succeeded</returns>
    Task<Response<Material>> AddMaterial(MaterialDto materialDto);
    /// <summary>
    /// add a material from a week
    /// </summary>
    /// <param name="materialId"> the id of the material</param>
    /// <returns>returns failed or succeeded</returns>
    Task<Response> DeleteMaterial(int materialId);
    /// <summary>
    /// get all materials that exist in a week
    /// </summary>
    /// <param name="weekId">the id of week</param>
    /// <returns>returns list of materials</returns>
    Task<Response<IEnumerable<Material>>> GetWeekMaterials(int weekId);
    /// <summary>
    /// update a material with new data
    /// </summary>
    /// <param name="materialId">the id of the material</param>
    /// <param name="materialDto">the new data of the material</param>
    /// <returns>returns failed or succeeded</returns>
    Task<Response> UpdateMaterial(int materialId, MaterialDto materialDto);
}