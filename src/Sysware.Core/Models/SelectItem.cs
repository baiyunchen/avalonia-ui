namespace Sysware.Core.Models;

public class SelectItem<TValue,TLabel>
{
    public SelectItem(TValue value, TLabel label)
    {
        Value = value;
        Label = label;
    }
    
    public TValue Value { get; set; }
    
    public TLabel Label { get; set; }
}