﻿namespace Quain.Domain.Models
{
    using Microsoft.EntityFrameworkCore;

    public class Points
    {
        public Guid PointsId { get; private set; }

        public Guid CustomerId { get; private set; }

        public Customer Customer { get; private set; }

        [Precision(10, 2)]
        public decimal CurrentValue { get; private set; }

        public ICollection<PointsChanges> PointsChanges { get; private set; }

        public DateTimeOffset? LastUpdate { get; private set; }

        public DateTimeOffset CreatedDate { get; private set; }

        public Points()
        {
            
        }

        public Points(decimal currentValue, string ncomp)
        {
            CurrentValue = currentValue;
            CreatedDate = DateTimeOffset.UtcNow;

            this.AddChange(new PointsChanges(currentValue, ncomp));
        }

        public void UpdatePoints(decimal amount, string ncomp)
        {
            CurrentValue += amount;
            LastUpdate = DateTimeOffset.UtcNow;

            this.AddChange(new PointsChanges(amount, ncomp));
        }

        public void AddChange(PointsChanges pointsChange)
        {
            if (PointsChanges is null)
                PointsChanges = new List<PointsChanges>();

            PointsChanges.Add(pointsChange);
        }

    }
}
