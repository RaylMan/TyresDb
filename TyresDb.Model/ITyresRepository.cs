namespace TyresDb.Model
{
    public interface ITyresRepository
    {
        List<Tyre> Tyres { get; }

        bool Load(out string errorText);
        void Save();
    }
}