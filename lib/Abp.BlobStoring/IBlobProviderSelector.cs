namespace Abp.BlobStoring;

public interface IBlobProviderSelector
{
    IBlobProvider Get(string containerName);
}
