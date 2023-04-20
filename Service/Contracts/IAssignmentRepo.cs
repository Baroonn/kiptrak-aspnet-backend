using Domain.Models;

namespace Service.Contracts;

public interface IAssignmentRepo
{
    Task<IEnumerable<Assignment>> GetAllAssignmentsAsync(string userId, bool trackChanges);
    Task<Assignment> GetAssignmentAsync(Guid id, bool trackChanges);
    void CreateAssignment(Assignment assignment);
    void DeleteAssignment(Assignment assignment);
}