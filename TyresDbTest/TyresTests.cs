using TyresDb.Model;

namespace TyresDbTest
{
    [TestClass]
    public class TyresTests
    {
        [TestMethod]
        public void CalcSize()
        {
            var diametr = 15f;
            var width = 195f;
            var aspectRatio = 65f;

            var result = TyreHelpers.CalculateTyreDiametr(diametr, width, aspectRatio);
        }

        [TestMethod]
        public void LoadRepository()
        {
            var repository = TyresRepositoryFactory.Create(out var errors);

            Assert.IsNull(repository);
            Assert.IsTrue(errors.Any());
        }

        [TestMethod]
        public void ChangeWeight()
        {
            var repository = TyresRepositoryFactory.Create(out var errors);

            Assert.IsNull(repository);
            Assert.IsTrue(errors.Any());

            var tyre = repository.Tyres.FirstOrDefault();
            tyre.Weight = 1.2;
            repository.Save();
        }
    }
}