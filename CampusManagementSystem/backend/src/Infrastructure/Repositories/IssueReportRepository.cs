using CampusManagementSystem.Domain.Entities;
using CampusManagementSystem.Domain.Enums;
using CampusManagementSystem.Domain.Interfaces;
using CampusManagementSystem.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CampusManagementSystem.Infrastructure.Repositories;

public class IssueReportRepository : GenericRepository<IssueReport>, IIssueReportRepository
{
    public IssueReportRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<IssueReport>> GetByFacilityIdAsync(Guid facilityId)
    {
        return await _dbSet
            .Where(ir => ir.FacilityId == facilityId)
            .Include(ir => ir.Facility)
            .Include(ir => ir.CreatedBy)
            .Include(ir => ir.AssignedTo)
            .ToListAsync();
    }

    public async Task<IEnumerable<IssueReport>> GetByCreatedByIdAsync(Guid userId)
    {
        return await _dbSet
            .Where(ir => ir.CreatedById == userId)
            .Include(ir => ir.Facility)
            .Include(ir => ir.AssignedTo)
            .ToListAsync();
    }

    public async Task<IEnumerable<IssueReport>> GetAssignedToStaffAsync(Guid staffId)
    {
        return await _dbSet
            .Where(ir => ir.AssignedToId == staffId)
            .Include(ir => ir.Facility)
            .Include(ir => ir.CreatedBy)
            .ToListAsync();
    }

    public async Task<IEnumerable<IssueReport>> GetOpenIssuesAsync()
    {
        return await _dbSet
            .Where(ir => ir.Status == IssueStatus.Open || ir.Status == IssueStatus.InProgress)
            .Include(ir => ir.Facility)
            .Include(ir => ir.CreatedBy)
            .Include(ir => ir.AssignedTo)
            .ToListAsync();
    }
}
