namespace DirectoryOfNaturalPersons.Domain.Interface;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken);
}