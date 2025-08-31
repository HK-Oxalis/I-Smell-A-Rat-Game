using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEditor.UIElements;
using System.IO;

public class Dialogue_Editor : EditorWindow
{
    private Conversation active_Conversation;
    private List<Dialogue_Line> lines;
    private int current_Line = 0;

    private ListView list;

    private ObjectField conversation_Select;
    private TextField name_Edit;
    private IntegerField speaker_Count;
    private Button add_Line_Button;
    private Button save_Button;

    private TextField line_Edit;
    private TextField keyphrase_Edit;
    private FloatField volume_Edit;
    private FloatField length_Edit;
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

        lines = new List<Dialogue_Line>();
        lines.Add(new Dialogue_Line());

        conversation_Select = new ObjectField();
        conversation_Select.objectType = typeof(TextAsset);
        root.Add(conversation_Select);
        conversation_Select.RegisterCallback<ChangeEvent<Object>>((evt) => { Load_Conversation(evt.newValue); });

        Label name_Label = new Label("Conversation name");
        root.Add(name_Label);
        name_Edit = new TextField();
        root.Add(name_Edit);


        Label speaker_Label = new Label("How many people are speaking in this conversation?");
        root.Add(speaker_Label);
        speaker_Count = new IntegerField();
        root.Add(speaker_Count);
        speaker_Count.RegisterCallback<ChangeEvent<int>>((evt) => { Update_Speakers(evt.newValue); evt.StopPropagation(); });

        add_Line_Button = new Button();
        add_Line_Button.text = "Add line to conversation";
        root.Add(add_Line_Button);
        add_Line_Button.RegisterCallback<ClickEvent>((evt) => Add_Line());

        save_Button = new Button();
        save_Button.text = "Save conversation to file";
        root.Add(save_Button);
        save_Button.RegisterCallback<ClickEvent>((evt) => Save_Conversation());

        var splitView = new TwoPaneSplitView(0, 50, TwoPaneSplitViewOrientation.Horizontal);


        root.Add(splitView);


        list = new ListView();
        splitView.Add(list);

        list.makeItem = () => new Label();
        list.bindItem = (item, index) => { (item as Label).text = index.ToString() + ": " + lines[index].text; };
        list.itemsSource = lines;

        list.selectedIndicesChanged += (IEnumerable<int> selected) => { Change_Line(list.selectedIndex); };

        var rightPane = new VisualElement();
        splitView.Add(rightPane);

        rightPane.Add(new Label("Dialogue"));
        line_Edit = new TextField();
        rightPane.Add(line_Edit);
        line_Edit.RegisterCallback<ChangeEvent<string>>((evt) => {Update_Text(); evt.StopPropagation(); });

        rightPane.Add(new Label("Key phrase (if any)"));
        keyphrase_Edit = new TextField();
        rightPane.Add(keyphrase_Edit);
        keyphrase_Edit.RegisterCallback<ChangeEvent<string>>((evt) => { lines[current_Line].keyphrase = evt.newValue; });

        rightPane.Add(new Label("Volume (from 1 - 10)"));
        volume_Edit = new FloatField();
        rightPane.Add(volume_Edit);
        volume_Edit.RegisterCallback<ChangeEvent<float>>((evt) => { lines[current_Line].volume = evt.newValue; });

        rightPane.Add(new Label("Length (in seconds)"));
        length_Edit = new FloatField();
        rightPane.Add(length_Edit);
        length_Edit.RegisterCallback<ChangeEvent<float>>((evt) => { lines[current_Line].length_Seconds = evt.newValue; });

        rightPane.Add(new Label("Speaker number"));
        speaker_Select = new DropdownField();
        rightPane.Add(speaker_Select);
        speaker_Select.RegisterCallback<ChangeEvent<string>>((evt) => { Update_Speaker_Select(); evt.StopPropagation(); });


        list.SetSelection(0);
    }

    private void Change_Line(int new_Line)
    {
        line_Edit.value = lines[new_Line].text;
        keyphrase_Edit.value = lines[new_Line].keyphrase;
        volume_Edit.value = lines[new_Line].volume;
        length_Edit.value = lines[new_Line].length_Seconds;
        speaker_Select.index = lines[new_Line].speaker_Number;


        current_Line = new_Line;
    }

    private void Add_Line()
    {
        lines.Add(new Dialogue_Line());

        Change_Line(lines.Count - 1);
        list.RefreshItems();
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
        lines[current_Line].speaker_Number = speaker_Select.index;
    }


    private void Load_Conversation(Object convo_Obj)
    {
        TextAsset convo_Text = (TextAsset)convo_Obj;
        Conversation convo = ScriptableObject.CreateInstance<Conversation>();
        convo.load_JSON(convo_Text.text);

        active_Conversation = convo;

        this.lines.Clear();
        this.lines.AddRange(convo.lines);
        this.speaker_Count.value = convo.source_Count;
        this.name_Edit.value = convo.file_Name;

        Update_Speaker_Choice();

        list.RefreshItems();
        list.SetSelection(0);

    }

    private void Save_Conversation() 
    {
        if (active_Conversation == null) { active_Conversation = ScriptableObject.CreateInstance<Conversation>(); }

        active_Conversation.lines = this.lines.ToArray();
        active_Conversation.source_Count = this.speaker_Count.value;
        active_Conversation.file_Name = this.name_Edit.value;


        string path = "Assets/Resources/Conversations/" + active_Conversation.file_Name + ".txt";

        File.WriteAllText(path, active_Conversation.Save_JSON());

        AssetDatabase.ImportAsset(path);
    }



}
