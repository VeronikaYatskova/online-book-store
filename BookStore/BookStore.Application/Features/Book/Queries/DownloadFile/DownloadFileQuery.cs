using BookStore.Application.DTOs;
using MediatR;

namespace BookStore.Application.Features.Book.Queries.DownloadFile
{
    public record DownloadFileQuery(AwsDataWithClientUrl request) : IRequest<byte[]>;
}