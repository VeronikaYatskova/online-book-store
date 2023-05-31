using Application.DTOs.Response;
using Domain.Entities;
using MediatR;

namespace Application.Features.Book.Commands.UploadFile
{
    public record UploadFileCommand(S3Object s3obj, AwsCredentials awsCred, string clientUrl) : IRequest<S3ResponseDto>;
}