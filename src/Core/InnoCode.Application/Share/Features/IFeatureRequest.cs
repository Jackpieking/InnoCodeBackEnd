namespace InnoCode.Application.Share.Features;

public interface IFeatureRequest<out TResponse>
    where TResponse : class, IFeatureResponse { }
