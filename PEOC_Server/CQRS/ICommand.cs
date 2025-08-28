namespace PEOC_Server.CQRS
{
    public interface ICommand: ICommand<Unit>
    {
        
    }

    public interface ICommand<out TResponse> : IRequest<TResponse>
    {

    }
}
