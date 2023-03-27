namespace TyresDb.Model
{
    public static class TyresRepositoryFactory
    {
        public static ITyresRepository Create(out string errors)
        {
            errors = string.Empty;
            var respository = new TyresCsvRepository();
            respository.Load(out errors);
            return respository;
        }
    }
}
