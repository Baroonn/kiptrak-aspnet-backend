using System.Security.Claims;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service.ServiceContracts;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using System;
//using WindowsAzure.Storage;

namespace Presentation.Controllers;

[ApiVersion("1.0")]
[Route("api/{v:apiversion}/assignments")]
[ApiController]
[Authorize]
public class AssignmentsController : ControllerBase
{

    private readonly IServiceManager _service;
    private readonly IBlobService _blobService;
    private readonly UserManager<AppUser> _userManager;

    public AssignmentsController(IServiceManager service, UserManager<AppUser> userManager, IBlobService blobService)
    {
        _service = service;
        _userManager = userManager;
        _blobService = blobService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAssignments()
    {
        // _userManager.GetUserAsync(User);
        var name = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var currUser = await _userManager.FindByNameAsync(name);
        var assignments = await _service.AssignmentService.GetAllAssignmentsAsync(currUser.Id, trackChanges: false);
        return Ok(assignments);
    }

    [HttpGet("{id:guid}", Name="AssignmentById")] 
    public async Task<IActionResult> GetAssignment(Guid id) 
    { 
        var assignment = await _service.AssignmentService.GetAssignmentAsync(id, trackChanges: false); 
        return Ok(assignment); 
    }

    [HttpPost("{id:guid}/uploadimage")]
    public async Task<IActionResult> UploadImage(Guid id, [FromForm]IList<IFormFile> file)
    {

        foreach(var item in file)
        {
            await _blobService.UploadImage(item, id);
            break;
        }
        
        //await _blobService.UploadImage(files[0], id);
        //var containerClient = new BlobContainerClient("DefaultEndpointsProtocol=https;AccountName=kiptrak;AccountKey=O1/oR6AZRuxQYEq+AV9P5AL6wqDPNIQAG0Sk2p0dxtCGiui5RZ66TW/GmXir4ZGtVUslpwmuCCl7+ASt95M23A==;EndpointSuffix=core.windows.net", "images");
        //containerClient.Create();
        
        // foreach(var item in file )
        // {
        //     string ext = System.IO.Path.GetExtension(item.FileName);
        //     BlobClient client = containerClient.GetBlobClient(id.ToString() + ext);
        //     Console.WriteLine(file==null?"0":"1");
        //     if(item.Length > 0){
        //         Console.WriteLine(item.Length);
        //         client.Upload(item.OpenReadStream());
        //     }
            
        // }

        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAssignment([FromBody] AssignmentCreateDto assignmentCreateDto)
    {
        var name = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var currUser = await _userManager.FindByNameAsync(name);
        if (assignmentCreateDto is null)
        {
            throw new AssignmentNullException();
        }
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }
        var assignment = await _service.AssignmentService.CreateAssignmentAsync(currUser.Id, assignmentCreateDto);
        
        return CreatedAtRoute("AssignmentById", new { id = assignment.Id }, assignment);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAssignment(Guid id)
    {
        var name = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var currUser = await _userManager.FindByNameAsync(name);
        await _service.AssignmentService.DeleteAssignmentAsync(currUser.Id, id, trackChanges: false);
        return NoContent();
    }
}