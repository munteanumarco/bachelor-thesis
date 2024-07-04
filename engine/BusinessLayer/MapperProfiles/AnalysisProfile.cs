using AutoMapper;
using BusinessLayer.DTOs.Analysis;
using DataAccessLayer.Entities;

namespace BusinessLayer.MapperProfiles;

public class AnalysisProfile : Profile
{
    public AnalysisProfile()
    {
        CreateMap<LandCoverAnalysisDto, LandCoverAnalysis>();
    }
}