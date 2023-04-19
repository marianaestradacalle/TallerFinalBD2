using Grpc.Core;
using Grpc.Core.Interceptors;
using GrpcApiService.ServiceName.Exceptions;
using Microsoft.Extensions.Logging;

namespace GrpcApi.Exceptions;
public class ExceptiongRPCInterceptor : Interceptor
{
    private readonly ILogger<ExceptiongRPC> _logger;
    private readonly ExceptiongRPC _exceptionHelpers;

    public ExceptiongRPCInterceptor(ILogger<ExceptiongRPC> logger)
    {
        _exceptionHelpers = new(logger);
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (Exception ex)
        {
            throw _exceptionHelpers.Handle(ex.GetType(), ex);
        }
    }

    public override async Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        ServerCallContext context,
        ClientStreamingServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(requestStream, context);
        }
        catch (Exception ex)
        {
            throw _exceptionHelpers.Handle(ex.GetType(), ex);
        }
    }


    public override async Task ServerStreamingServerHandler<TRequest, TResponse>(
        TRequest request,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            await continuation(request, responseStream, context);
        }
        catch (Exception ex)
        {
            throw _exceptionHelpers.Handle(ex.GetType(), ex);
        }
    }

    public override async Task DuplexStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        DuplexStreamingServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            await continuation(requestStream, responseStream, context);
        }
        catch (Exception ex)
        {
            throw _exceptionHelpers.Handle(ex.GetType(), ex);
        }
    }

}
