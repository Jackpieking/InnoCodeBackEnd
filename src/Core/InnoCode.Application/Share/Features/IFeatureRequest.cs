using FastEndpoints;

namespace InnoCode.Application.Share.Features;

public interface IFeatureRequest<out TResponse> : ICommand<TResponse>
    where TResponse : class, IFeatureResponse { }
