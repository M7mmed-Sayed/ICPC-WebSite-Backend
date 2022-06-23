﻿using ICPC_WebSite_Backend.Data.Models;
using ICPC_WebSite_Backend.Data.Models.DTO;
using ICPC_WebSite_Backend.Data.Response;

namespace ICPC_WebSite_Backend.Repository;

public interface IMaterialRepository
{
    Task<Response> AddMaterial(MaterialDto materialDto);
    Task<Response> DeleteMaterial(int materialId);
    Task<Response<IEnumerable<Material>>> GetWeekMaterials(int weekId);
    Task<Response> UpdateMaterial(int materialId, MaterialDto materialDto);
}