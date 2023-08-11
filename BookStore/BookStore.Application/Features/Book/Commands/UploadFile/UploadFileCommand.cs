using BookStore.Application.DTOs.Response;
using BookStore.Application.Services.CloudServices.Amazon.Models;
using MediatR;

namespace BookStore.Application.Features.Book.Commands.UploadFile
{
    public record UploadFileCommand(UploadFileModel UploadFileModel) : IRequest<S3ResponseDto>;
}