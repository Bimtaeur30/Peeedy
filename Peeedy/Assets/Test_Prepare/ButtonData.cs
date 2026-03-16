using UnityEngine.UIElements;
using UnityEngine;

[UxmlElement]
public partial class ButtonData : Button
{
    [UxmlAttribute]
    public int ButtonIndex { get; set; }
}