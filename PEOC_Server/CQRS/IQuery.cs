namespace PEOC_Server.CQRS
{
    public interface IQuery<out TResponse>: IRequest<TResponse>
        where TResponse : notnull
    {

    }
}
