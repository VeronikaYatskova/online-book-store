using BookStore.Application.DTOs.Response;
using BookStore.Domain.Entities;
using MediatR;

namespace BookStore.Application.Features.Book.Commands.UploadFile
{
    public record UploadFileCommand(S3Object s3obj, AwsCredentials awsCred, string clientUrl) : IRequest<S3ResponseDto>;
}