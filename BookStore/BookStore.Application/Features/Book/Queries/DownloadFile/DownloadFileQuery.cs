using MediatR;

namespace BookStore.Application.Features.Book.Queries.DownloadFile
{
    public record DownloadFileQuery(string FileName) : IRequest<byte[]>;
}