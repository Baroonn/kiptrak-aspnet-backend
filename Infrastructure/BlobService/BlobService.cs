using Service.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using System;

namespace Infrastructure.BlobService;

public class BlobService : IBlobService
{

    public BlobService()
    {

    }
    
    public async Task UploadImage(IFormFile file, Guid id)
    {
        var containerClient = new BlobContainerClient("DefaultEndpointsProtocol=https;AccountName=kiptrak;AccountKey=O1/oR6AZRuxQYEq+AV9P5AL6wqDPNIQAG0Sk2p0dxtCGiui5RZ66TW/GmXir4ZGtVUslpwmuCCl7+ASt95M23A==;EndpointSuffix=core.windows.net", "images");
        string ext = System.IO.Path.GetExtension(file.FileName);
        BlobClient client = containerClient.GetBlobClient(id.ToString() + ext);
        client.Upload(file.OpenReadStream());
        // using(var ms = new MemoryStream())
        // {
        //     file.CopyTo(ms);
        //     var fileBytes = ms.ToArray();
        //     BinaryData data = new BinaryData(fileBytes);
        //     await client.UploadAsync(data, true);
        // }
    }
}