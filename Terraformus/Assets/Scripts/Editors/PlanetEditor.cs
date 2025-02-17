using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CubePlanet))]
public class PlanetEditor : Editor
{
    CubePlanet planet;

    // Editors 
    Editor shapeEditor;
    Editor colorEditor;

    public void OnEnable() {
        planet = (CubePlanet)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        ShowSettings(planet.colorSettings, planet.OnColorChanged, ref colorEditor, ref planet.colorFoldout);
        ShowSettings(planet.shapeSettings, planet.OnShapeChanged, ref shapeEditor, ref planet.shapeFoldout);
    }

    /// <summary>
    /// Used to draw inspector menus for any settings objects.
    /// </summary>
    /// <param name="settings">The scriptableOject</param>
    /// <param name="onChange">A funciton to call when settings change</param>
    /// <param name="e">The editor to draw on</param>
    /// <param name="foldout">Whether the settings are folded out or not</param>
    private void ShowSettings(Object settings, 
                                System.Action onChange, 
                                ref Editor e, ref bool foldout) {
        // No settings to show
        if (settings == null) return; 

        // Draws the titleBar
        foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);    

        // Checks for changes in the editor and updates the shape
        using var check = new EditorGUI.ChangeCheckScope();
        
        // Draws the settings editor
        if(foldout) 
        {
            CreateCachedEditor(settings, null, ref e);
            e.OnInspectorGUI();
        }
        
        // Checks if there was a change
        if (!check.changed) return; // returns early if no changes in editor
        // Logic to call onChange
        if (onChange != null) onChange();
    }

}