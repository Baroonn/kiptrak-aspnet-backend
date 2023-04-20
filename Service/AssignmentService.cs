using AutoMapper;
using Domain.Exceptions;
using Domain.Models;
using Service.Contracts;
using Service.DTOs;
using Service.ServiceContracts;

namespace Service
{
    internal sealed class AssignmentService : IAssignmentService
    {
        private readonly IRepoManager _repository;
        private readonly IMapper _mapper;

        public AssignmentService(IRepoManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AssignmentReadDto> CreateAssignmentAsync(string userId, AssignmentCreateDto assignmentCreateDto)
        {
            var assignment = _mapper.Map<Assignment>(assignmentCreateDto);
            assignment.CreatedAt = DateTime.Now;
            assignment.UpdatedAt = DateTime.Now;
            assignment.UserId = userId;
            _repository.Assignment.CreateAssignment(assignment);
            await _repository.SaveAsync();
            var assignmentReadDto = _mapper.Map<AssignmentReadDto>(assignment);
            return assignmentReadDto;
        }

        public async Task DeleteAssignmentAsync(string userId, Guid id, bool trackChanges)
        {
            var assignment = await _repository.Assignment.GetAssignmentAsync(id, trackChanges);
            if (assignment is null)
            {
                throw new AssignmentNotFoundException(id);
            }

            if (assignment.UserId != userId)
            {
                throw new AssignmentNotAuthorizedException();
            }

            _repository.Assignment.DeleteAssignment(assignment);
            await _repository.SaveAsync();
        }

        public async Task<IEnumerable<AssignmentReadDto>> GetAllAssignmentsAsync(string userId, bool trackChanges)
        {
            var following = (await _repository.Profile.GetUserProfileAsync(new Guid(userId), trackChanges)).Following;
            var assignments = await _repository.Assignment.GetAllAssignmentsAsync(userId, trackChanges);
            var ids = following?.Split("|");
            var validAssignments = assignments.ToList();
            if(ids != null)
            {
                foreach(var id in ids)
                {
                    if (id == userId){
                        continue;
                    }
                    validAssignments.AddRange(await _repository.Assignment.GetAllAssignmentsAsync(id, trackChanges));
                }
            }       
            var assignmentsReadDto = _mapper.Map<IEnumerable<AssignmentReadDto>>(validAssignments);
            return assignmentsReadDto
            .OrderByDescending(x => x.CreatedAt)
            .ToList();
        }

        public async Task<AssignmentReadDto> GetAssignmentAsync(Guid id, bool trackChanges)
        {
            var assignment = await _repository.Assignment.GetAssignmentAsync(id, trackChanges); 
            if (assignment is null)
            {
                throw new AssignmentNotFoundException(id);
            } 
            var assignmentReadDto = _mapper.Map<AssignmentReadDto>(assignment); 
            return assignmentReadDto;
        }
    }
}