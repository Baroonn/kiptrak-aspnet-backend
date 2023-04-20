using Microsoft.AspNetCore.Http;
namespace Service.ServiceContracts;

public interface IBlobService
{
    Task UploadImage(IFormFile file, Guid id);
}