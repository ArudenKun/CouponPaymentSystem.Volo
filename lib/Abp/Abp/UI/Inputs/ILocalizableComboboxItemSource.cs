namespace Abp.UI.Inputs;

public interface ILocalizableComboboxItemSource
{
    ICollection<ILocalizableComboboxItem> Items { get; }
}
