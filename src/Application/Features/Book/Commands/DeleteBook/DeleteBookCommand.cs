using Domain.Entities;
using MediatR;

namespace Application.Features.Book.Commands.DeleteBook
{
    public record DeleteBookCommand(AwsCredentials awsCred, string clientUrl, string bucketName, string bookId) : IRequest;
}
