using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using JetBrains.Annotations;

public class Dialogue_Editor : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;
    private List<Dialogue_Line> lines;
    private int current_Line = 0;

    private ListView list;

    private TextField line_Edit;
    private IntegerField speaker_Count;
    private DropdownField speaker_Select;

    [MenuItem("Window/UI Toolkit/Dialogue_Editor")]
    public static void ShowExample()
    {
        Dialogue_Editor wnd = GetWindow<Dialogue_Editor>();
        wnd.titleContent = new GUIContent("Dialogue_Editor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;
        
        speaker_Count = new IntegerField();
        root.Add(speaker_Count);
        speaker_Count.RegisterCallback<ChangeEvent<int>>((evt) => { Update_Speakers(evt.newValue); evt.StopPropagation(); });

        // Create a two-pane view with the left pane being fixed.
        var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);

        // Add the view to the visual tree by adding it as a child to the root element.
        root.Add(splitView);

        // A TwoPaneSplitView needs exactly two child elements.
        list = new ListView();
        splitView.Add(list);

        list.makeItem = () => new Label();
        list.bindItem = (item, index) => { (item as Label).text = lines[index].text; };
        list.itemsSource = lines;


        var rightPane = new VisualElement();
        splitView.Add(rightPane);


        line_Edit = new TextField();
        rightPane.Add(line_Edit);
        line_Edit.RegisterCallback<ChangeEvent<string>>((evt) => {Update_Text(); evt.StopPropagation(); }); 

        speaker_Select = new DropdownField();
        rightPane.Add(speaker_Select);
        speaker_Select.RegisterCallback<ChangeEvent<string>>((evt) => { Update_Speaker_Select(); evt.StopPropagation(); });


        lines = new List<Dialogue_Line>();
        lines.Add(new Dialogue_Line());

        Change_Line(0);
    }

    private void Change_Line(int new_Line)
    {
        line_Edit.value = lines[new_Line].text;
        speaker_Select.index = lines[new_Line].source_Index;


        current_Line = new_Line;
    }

    private void Update_Text()
    {
        lines[current_Line].text = line_Edit.value;
        list.RefreshItems();
    }

    private void Update_Speakers(int speakers)
    {
        Update_Speaker_Choice();
    }

    private void Update_Speaker_Choice()
    {
        List<string> number_List = new List<string>();

        for (int i = 0; i < speaker_Count.value; i++) { number_List.Add((i + 1).ToString()); }

        speaker_Select.choices = number_List;

    }

    private void Update_Speaker_Select()
    {
        lines[current_Line].source_Index = speaker_Select.index;
    }
}
