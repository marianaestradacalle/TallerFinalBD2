using Common.Helpers.Exceptions;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcApiService.ServiceName.Exceptions;
public class ExceptiongRPC
{
    private readonly IDictionary<Type, Func<Exception, RpcException>> _exceptionHandlers;

    private readonly ILogger<ExceptiongRPC> _logger;
    public ExceptiongRPC(ILogger<ExceptiongRPC> logger)
    {
        _exceptionHandlers = new Dictionary<Type, Func<Exception, RpcException>>
        {
            { typeof(BusinessException), HandleBusinessException }
        };
        _logger = logger;
    }

    public RpcException Handle(Type type, Exception exception)
    {

        if (_exceptionHandlers.ContainsKey(type))
        {
            throw _exceptionHandlers[type].Invoke(exception);
        }
        else
        {
            throw HandleDefault(exception);
        }

    }

    private RpcException HandleBusinessException(Exception exception)
    {
        var status = new Status(StatusCode.FailedPrecondition, exception.Message);

        return new RpcException(status);
    }

    private RpcException HandleDefault(Exception exception)
    {
        _logger.LogError(exception, $"An error occurred");
        return new RpcException(new Status(StatusCode.Internal, exception.Message));
    }
}
