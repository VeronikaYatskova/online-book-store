using BookStore.Domain.Entities;
using MediatR;

namespace BookStore.Application.Features.Book.Commands.DeleteBook
{
    public record DeleteBookCommand(AwsCredentials awsCred, string clientUrl, string bucketName, string bookId) : IRequest;
}
