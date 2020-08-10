using System;
using UnityEngine;
using UnityEditor;


//
// Custom editor for editing property binder list
//
sealed class PropertyBinderEditor
{
    #region Public methods

    public PropertyBinderEditor(SerializedProperty binders,string datatype)
    {
        _binders = binders;
        _datatype = datatype; 
        ComponentSelector.InvalidateCache();
    }

    public void ShowGUI()
    {
        EditorGUILayout.Space();

        for (var i = 0; i < _binders.arraySize; i++)
        {
            Action<int> showPropertyBinderEditor = ShowPropertyBinderEditor;
            showPropertyBinderEditor(i);
        }

        CoreEditorUtils.DrawSplitter();
        EditorGUILayout.Space();

        // "Add Property Binder" button
        var rect = EditorGUILayout.GetControlRect();
        rect.x += (rect.width - 200) / 2;
        rect.width = 200;

        if (GUI.Button(rect, "Add " + _datatype + "Property Binder"))
        {
           //  CreateNewPropertyBinderMenu(_datatype).DropDown(rect);
             //OnAddNewPropertyBinder<FloatValuePropertyBinder>();
             CreateNewProprtyBinder(_datatype);
            
        }

       
            
    }

    #endregion

    #region Private members

    SerializedProperty _binders;
    private String _datatype; 

    static class Styles
    {
        // public static Label Value0 = "Value at 0";
        // public static Label Value1 = "Value at 1";
        public static Label MoveUp = "Move Up";
        public static Label MoveDown = "Move Down";
        public static Label Remove = "Remove";
    }

    #endregion

    #region "Add Property Binder" button

    GenericMenu CreateNewPropertyBinderMenu(string dataType)
    {
        var menu = new GenericMenu();
        if (dataType == "bool")
        {
            AddNewPropertyBinderItem<BooleanPropertyBinder>(menu);   
        }

        if (dataType == "float")
        {
            AddNewPropertyBinderItem<FloatValuePropertyBinder>(menu);
        }

        // AddNewPropertyBinderItem<FloatPropertyBinder>(menu);
        // AddNewPropertyBinderItem<Vector3PropertyBinder>(menu);
        // AddNewPropertyBinderItem<EulerRotationPropertyBinder>(menu);
        // AddNewPropertyBinderItem<ColorPropertyBinder>(menu);
       
        return menu;
    }

    public void AddNewPropertyBinderItem<T>(GenericMenu menu) where T : new()
      => menu.AddItem(PropertyBinderTypeLabel<T>.Content,
                      false, OnAddNewPropertyBinder<T>);

    void CreateNewProprtyBinder(string dataType)
    {
        if (dataType == "bool")
        {
            OnAddNewPropertyBinder<BooleanPropertyBinder>();   
        }

        if (dataType == "float")
        {
            OnAddNewPropertyBinder<FloatValuePropertyBinder>();
        }
    }

    void OnAddNewPropertyBinder<T>() where T : new()
    {
        _binders.serializedObject.Update();

        var i = _binders.arraySize;
        _binders.InsertArrayElementAtIndex(i);
        _binders.GetArrayElementAtIndex(i).managedReferenceValue = new T();

        _binders.serializedObject.ApplyModifiedProperties();
    }

    #endregion

    #region PropertyBinder editor

    void ShowPropertyBinderEditor(int index)
    {
        var prop = _binders.GetArrayElementAtIndex(index);
        var finder = new RelativePropertyFinder(prop);

        // Header
        CoreEditorUtils.DrawSplitter();

        var toggle = CoreEditorUtils.DrawHeaderToggle
          (PropertyBinderNameUtil.Shorten(prop),
           prop, finder["Enabled"],
           pos => CreateHeaderContextMenu(index)
                  .DropDown(new Rect(pos, Vector2.zero)));

        if (!toggle) return;

        _binders.serializedObject.Update();
        EditorGUILayout.Space();

        // Properties
        var target = finder["Target"];
        EditorGUILayout.PropertyField(target);

        if (ComponentSelector.GetInstance(target).ShowGUI(target) &&
            PropertySelector.GetInstance(target, finder["_propertyType"])
            .ShowGUI(finder["PropertyName"]))
        {
            // EditorGUILayout.PropertyField(finder["Value0"], Styles.Value0);
            // EditorGUILayout.PropertyField(finder["Value1"], Styles.Value1);
        }

        _binders.serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Space();
    }

    #endregion

    #region ProeprtyBinder editor context menu

    GenericMenu CreateHeaderContextMenu(int index)
    {
        var menu = new GenericMenu();

        // Move up
        if (index == 0)
            menu.AddDisabledItem(Styles.MoveUp);
        else
            menu.AddItem(Styles.MoveUp, false,
                         () => OnMoveControl(index, index - 1));

        // Move down
        if (index == _binders.arraySize - 1)
            menu.AddDisabledItem(Styles.MoveDown);
        else
            menu.AddItem(Styles.MoveDown, false,
                         () => OnMoveControl(index, index + 1));

        menu.AddSeparator(string.Empty);

        // Remove
        menu.AddItem(Styles.Remove, false, () => OnRemoveControl(index));

        return menu;
    }

    void OnMoveControl(int src, int dst)
    {
        _binders.serializedObject.Update();
        _binders.MoveArrayElement(src, dst);
        _binders.serializedObject.ApplyModifiedProperties();
    }

    void OnRemoveControl(int index)
    {
        _binders.serializedObject.Update();
        _binders.DeleteArrayElementAtIndex(index);
        _binders.serializedObject.ApplyModifiedProperties();
    }

    #endregion
}

