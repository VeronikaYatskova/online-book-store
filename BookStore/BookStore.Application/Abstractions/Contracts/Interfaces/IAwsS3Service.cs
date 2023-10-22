using BookStore.Application.DTOs;
using BookStore.Application.DTOs.Response;
using BookStore.Application.Features.Book.Queries.DownloadFile;
using BookStore.Application.Services.CloudServices.Amazon.Models;
using Microsoft.AspNetCore.Http;

namespace BookStore.Application.Abstractions.Contracts.Interfaces
{
    public interface IAwsS3Service
    {
        string GetFilePreSignedUrl(string fileName);
        Task<byte[]> DownloadFileFromAwsAsync(DownloadFileQuery request);
        Task DeleteFileFromCloudAsync(string fileName);
        Task<S3ResponseDto> UploadFileToCloudAsync(UploadFileModel uploadFileModel);
        Task<S3ResponseDto> UploadFileToBucketAsync(IFormFile file);
    }
}
