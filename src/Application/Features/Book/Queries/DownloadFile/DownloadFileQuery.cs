using Application.DTOs;
using MediatR;

namespace Application.Features.Book.Queries.DownloadFile
{
    public record DownloadFileQuery(AwsDataWithClientUrl request) : IRequest<byte[]>;
}