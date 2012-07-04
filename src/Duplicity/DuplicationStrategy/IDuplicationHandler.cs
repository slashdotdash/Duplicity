namespace Duplicity.DuplicationStrategy
{
    public interface IDuplicationHandler
    {
        void Handle(string fileOrDirectory);
    }
}
