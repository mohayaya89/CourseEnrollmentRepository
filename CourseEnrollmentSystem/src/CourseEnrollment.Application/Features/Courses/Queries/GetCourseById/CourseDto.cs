using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace CourseEnrollment.Application.Features.Courses.Queries.GetCourseById
{
    public class CourseDto
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public int Capacity { get; init; }
        public string Status { get; init; } = string.Empty;
        public int ActiveEnrollments { get; init; }
    }

    // (example — not adding to repo)
    public class MemoryCache<TKey, TValue>
    {
        // Use this instead
        private readonly ConcurrentDictionary<object, TValue> _inner = new();

        public void Set(TKey? key, TValue value)
        {
            var mapKey = key is null ? NullKey.Instance : (object)key;
            _inner[mapKey] = value;
        }

        public bool TryGet(TKey? key, out TValue? value)
        {
            var mapKey = key is null ? NullKey.Instance : (object)key;
            return _inner.TryGetValue(mapKey, out value);
        }

        private sealed class NullKey 
        { 
            public static readonly NullKey Instance = new(); 
        }
    }
}
