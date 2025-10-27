using _123Vendas.Domain.Sales.Domain.ValueObjects;
using Bogus;

namespace _123Vendas.Domain.Sales.Unit.Builders
{
    public class BranchBuilder
    {
        private static readonly Faker Faker = new();

        private Guid _id = Guid.NewGuid();
        private string _name = Faker.Company.CompanyName();

        public BranchBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public BranchBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public BranchBuilder WithEmptyId()
        {
            _id = Guid.Empty;
            return this;
        }

        public BranchBuilder WithNullName()
        {
            _name = null!;
            return this;
        }

        public BranchBuilder WithWhitespaceName()
        {
            _name = "   ";
            return this;
        }

        public Branch Build()
        {
            return new Branch(_id, _name);
        }

        public static Branch BuildValid() => new BranchBuilder().Build();
    }
}
