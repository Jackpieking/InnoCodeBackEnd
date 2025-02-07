﻿using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using InnoCode.Application.Share.Caching;
using InnoCode.Application.Share.Common;
using Microsoft.Extensions.Caching.Distributed;

namespace InnoCode.Redis.Handler;

/// <summary>
///     Implementation of cache handler using redis as storage.
/// </summary>
public sealed class RedisCacheHandler : ICacheHandler
{
    private readonly IDistributedCache _distributedCache;

    public RedisCacheHandler(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<AppCacheModel<TSource>> GetAsync<TSource>(
        string key,
        CancellationToken cancellationToken
    )
    {
        var cachedValue = await _distributedCache.GetStringAsync(
            key: key,
            token: cancellationToken
        );

        if (string.IsNullOrWhiteSpace(value: cachedValue))
        {
            return AppCacheModel<TSource>.NotFound;
        }

        return new()
        {
            Value = JsonSerializer.Deserialize<TSource>(
                json: cachedValue,
                AppCommonConstant.App.DefaultJsonSerializerOptions
            )
        };
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        return _distributedCache.RemoveAsync(key: key, token: cancellationToken);
    }

    public Task SetAsync<TSource>(
        string key,
        TSource value,
        DistributedCacheEntryOptions distributedCacheEntryOptions,
        CancellationToken cancellationToken
    )
    {
        return _distributedCache.SetStringAsync(
            key: key,
            value: JsonSerializer.Serialize(
                value: value,
                AppCommonConstant.App.DefaultJsonSerializerOptions
            ),
            options: distributedCacheEntryOptions,
            token: cancellationToken
        );
    }
}
