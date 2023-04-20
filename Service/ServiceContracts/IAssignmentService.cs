using Service.DTOs;

namespace Service.ServiceContracts;

public interface IAssignmentService
{
    Task<IEnumerable<AssignmentReadDto>> GetAllAssignmentsAsync(string userId, bool trackChanges);
    Task<AssignmentReadDto> GetAssignmentAsync(Guid id, bool trackChanges);
    Task<AssignmentReadDto> CreateAssignmentAsync(string userId, AssignmentCreateDto assignment);
    Task DeleteAssignmentAsync(string userId, Guid id, bool trackChanges);
}