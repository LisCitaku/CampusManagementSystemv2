using CampusManagementSystem.Domain.Entities;
using CampusManagementSystem.Domain.Interfaces;
using CampusManagementSystem.Infrastructure.Persistence.Context;

namespace CampusManagementSystem.Infrastructure.Repositories;

public class FacilityRepository : GenericRepository<Facility>, IFacilityRepository
{
    public FacilityRepository(ApplicationDbContext context) : base(context)
    {
    }
}
