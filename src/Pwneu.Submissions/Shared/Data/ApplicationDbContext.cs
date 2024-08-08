using Microsoft.EntityFrameworkCore;
using Pwneu.Submissions.Shared.Entities;

namespace Pwneu.Submissions.Shared.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public virtual DbSet<Submission> Submissions { get; init; } = null!;
}