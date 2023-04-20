using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Service.Contracts;

namespace Infrastructure.Repository;

public class AssignmentRepo: RepositoryBase<Assignment>, IAssignmentRepo
{
    public AssignmentRepo(RepositoryContext context)
        :base(context)
        {
            
        }

    public async Task<IEnumerable<Assignment>> GetAllAssignmentsAsync(string userId, bool trackChanges)
    {

        return await FindByCondition(a => a.UserId.Equals(userId) && DateTime.Now <= a.DateDue, trackChanges)
        //.OrderByDescending(a => a.CreatedAt)
        .Include(a => a.User)
        .ToListAsync();
    }

    public async Task<Assignment> GetAssignmentAsync(Guid id, bool trackChanges)
    {
        return await FindByCondition(c => c.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
    }

    public void CreateAssignment(Assignment assignment)
    {
        Create(assignment);
    }

    public void DeleteAssignment(Assignment assignment)
    {
        Delete(assignment);
    }
}