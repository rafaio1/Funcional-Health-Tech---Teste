using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FHT.Domain.Core
{
    public class EntityBase
    {
        #region atributos

        private List<INotification> _domainEvents;
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

        #endregion atributos

        #region constructor
        protected EntityBase()
        {
            ValidationResult = new ValidationResult();
        }
        #endregion constructor
        #region properties
        public long Id { get; private set; }

        [NotMapped]
        public ValidationResult ValidationResult { get; private set; }

        #endregion properties

        #region methods
        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        public void AddDomainNotification(string property, string message)
        {
            ValidationResult.Errors.Add(new ValidationFailure(property, message));
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is EntityBase))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            EntityBase item = (EntityBase)obj;

            return item.Id == Id;
        }

        public void Validate<TModel>(TModel model, AbstractValidator<TModel> validator)
        {
            validator.Validate(model).Errors.ToList().ForEach(ValidationResult.Errors.Add);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();

        }


        #endregion methods

        #region operators

        public static bool operator ==(EntityBase left, EntityBase right)
        {
            return Equals(left, null) ? Equals(right, null) ? true : false : left.Equals(right);
        }

        public static bool operator !=(EntityBase left, EntityBase right)
        {
            return !(left == right);
        }

        #endregion operators
    }
}
