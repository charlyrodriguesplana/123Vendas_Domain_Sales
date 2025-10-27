using _123Vendas.Domain.Sales.Domain.ValueObjects;
using Bogus;

namespace _123Vendas.Domain.Sales.Unit.Builders
{
    public class CustomerBuilder
    {
        private static readonly Faker Faker = new();

        private Guid _id = Guid.NewGuid();
        private string _name = Faker.Person.FullName;

        public CustomerBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public CustomerBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public CustomerBuilder WithEmptyId()
        {
            _id = Guid.Empty;
            return this;
        }

        public CustomerBuilder WithNullName()
        {
            _name = null!;
            return this;
        }

        public CustomerBuilder WithWhitespaceName()
        {
            _name = "   ";
            return this;
        }

        public Customer Build()
        {
            return new Customer(_id, _name);
        }

        public static Customer BuildValid() => new CustomerBuilder().Build();
    }
}
