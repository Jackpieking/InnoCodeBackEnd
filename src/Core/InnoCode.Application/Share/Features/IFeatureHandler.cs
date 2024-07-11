using FastEndpoints;

namespace InnoCode.Application.Share.Features;

public interface IFeatureHandler<TRequest, TResponse> : ICommandHandler<TRequest, TResponse>
    where TRequest : class, IFeatureRequest<TResponse>
    where TResponse : class, IFeatureResponse { }
