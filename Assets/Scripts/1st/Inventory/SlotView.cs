using TMPro;
using UnityEngine.UI;
public class SlotView
{
    private readonly Image source;
    private readonly TextMeshProUGUI sourceText;

    public SlotView(Image source, TextMeshProUGUI sourceText)
    {
        this.source = source;
        this.sourceText = sourceText;
    }

    public void ViewSlot(ConsumableConfig config = null, int count = 0)
    {
        source.sprite = config != null ? config.Icon : null;
        sourceText.text = count > 0 ? count.ToString() : "";
    }
}
