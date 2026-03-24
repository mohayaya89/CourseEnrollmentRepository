using System;
using System.Collections.Generic;
using System.Text;
using CourseEnrollment.Application.Common.Interfaces;
using CourseEnrollment.Domain.Common;
using CourseEnrollment.Domain.Persistence;

namespace CourseEnrollment.Infrastructure.UnitOfWork
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IDomainEventDispatcher _dispatcher;

        public EfUnitOfWork(AppDbContext context, IDomainEventDispatcher dispatcher)
        {
            _context = context;
            _dispatcher = dispatcher;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct)
        {
            var domainEntities = _context.ChangeTracker
                .Entries<BaseEntity>()
                .Where(x => x.Entity.DomainEvents.Any())
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            var result = await _context.SaveChangesAsync(ct);

            await _dispatcher.DispatchAsync(domainEvents);

            foreach (var entity in domainEntities)
            {
                entity.Entity.ClearDomainEvents();
            }

            return result;
        }
    }

}
