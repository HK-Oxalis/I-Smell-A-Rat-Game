using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEditor.UIElements;
using System.Linq;

public class Dialogue_Editor : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;
    private Conversation active_Conversation;
    private List<Dialogue_Line> lines;
    private int current_Line = 0;

    private ListView list;

    private ObjectField conversation_Select;
    private IntegerField speaker_Count;
    private Button add_Line_Button;
    private Button save_Button;

    private TextField line_Edit;
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
        conversation_Select.objectType = typeof(Conversation);
        root.Add(conversation_Select);
        conversation_Select.RegisterCallback<ChangeEvent<Object>>((evt) => { Load_Conversation(evt.newValue); });


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


        line_Edit = new TextField();
        rightPane.Add(line_Edit);
        line_Edit.RegisterCallback<ChangeEvent<string>>((evt) => {Update_Text(); evt.StopPropagation(); }); 

        speaker_Select = new DropdownField();
        rightPane.Add(speaker_Select);
        speaker_Select.RegisterCallback<ChangeEvent<string>>((evt) => { Update_Speaker_Select(); evt.StopPropagation(); });


        Change_Line(0);
    }

    private void Change_Line(int new_Line)
    {
        line_Edit.value = lines[new_Line].text;
        speaker_Select.index = lines[new_Line].source_Index;


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
        lines[current_Line].source_Index = speaker_Select.index;
    }


    private void Load_Conversation(Object convo_Obj)
    {
        Debug.Log(convo_Obj.GetType());
        Conversation convo = (Conversation)convo_Obj;
        Debug.Log(convo);

        this.lines.Clear();
        this.lines.AddRange(convo.lines);
        this.speaker_Count.value = convo.source_Count;

        Update_Speaker_Choice();
        Change_Line(0);

        list.RefreshItems();
    }

    private void Save_Conversation() 
    {
        string path = "Assets/Objects/Conversations/" + this.lines[0].text + ".asset";
        
        if(AssetDatabase.AssetPathExists(path)) { 
            active_Conversation = AssetDatabase.LoadAssetAtPath(path, typeof(Conversation)) as Conversation; 
        }
        else { active_Conversation = new Conversation(); }

            active_Conversation.lines = this.lines.ToArray();
        active_Conversation.source_Count = this.speaker_Count.value;

        

        Debug.Log(active_Conversation.lines.Length);

        if (!AssetDatabase.AssetPathExists(path)) { AssetDatabase.CreateAsset(active_Conversation, path); }
        else { 
            EditorUtility.SetDirty(AssetDatabase.LoadAssetAtPath(path, typeof(Object)));  
            AssetDatabase.SaveAssets(); 
        }

    }



}
