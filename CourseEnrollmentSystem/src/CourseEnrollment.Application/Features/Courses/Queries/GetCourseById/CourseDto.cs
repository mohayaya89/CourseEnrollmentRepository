using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace CourseEnrollment.Application.Features.Courses.Queries.GetCourseById
{
    public class CourseDto
    {
        public Guid Id { get; init; }
        public string Title { get; init; }
        public int Capacity { get; init; }
        public string Status { get; init; }
        public int ActiveEnrollments { get; init; }
    }

    public class MemoryCache<TKey, TValue>
    {
        private class CacheItem
        {
            public TValue Value { get; set; }
            public DateTime Expiration { get; set; }
        }

        private readonly ConcurrentDictionary<TKey, CacheItem> _cache = new();

        public void Set(TKey key, TValue value, TimeSpan ttl)
        {
            var item = new CacheItem
            {
                Value = value,
                Expiration = DateTime.UtcNow.Add(ttl)
            };

            _cache[key] = item;
        }

        public bool TryGet(TKey key, out TValue value)
        {
            value = default;

            if (_cache.TryGetValue(key, out var item))
            {
                if (item.Expiration > DateTime.UtcNow)
                {
                    value = item.Value;
                    return true;
                }

                _cache.TryRemove(key, out _);
            }

            return false;
        }
    }

}
