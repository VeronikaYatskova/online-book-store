namespace BookStore.Domain.Entities
{
    public static class UserRoleConstants
    {
        public static Guid NormalUserRoleId { get; } = Guid.Parse("09f1a17e-a830-415d-8611-7c9595d3dcc5");
        public static Guid PublisherRoleId { get; } = Guid.Parse("4e27e0eb-2033-4db8-85a4-86c40f8122f7");
        public static Guid AuthorRoleId { get; } = Guid.Parse("984a871c-e075-4fea-84d1-672dc4212b32");
    }
}