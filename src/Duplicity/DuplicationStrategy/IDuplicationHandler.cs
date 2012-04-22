namespace Duplicity.DuplicationStrategy
{
    internal interface IDuplicationHandler
    {
        void Handle(string fileOrDirectory);
    }
}
