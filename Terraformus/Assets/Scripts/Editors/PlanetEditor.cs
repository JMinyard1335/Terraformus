using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    Planet planet;

    // Editors 
    Editor shapeEditor;
    Editor colorEditor;
    Editor[] terrainEditor;

    public void OnEnable()
    {
        planet = (Planet)target;
        if (planet.terrainFoldouts == null || planet.terrainFoldouts.Length != planet.terrainSettings.Length)
        {
            planet.terrainFoldouts = new bool[planet.terrainSettings.Length];
        }

        terrainEditor = new Editor[planet.terrainSettings.Length];
    }

    /// <summary>
    /// Overridding this allows you to display a custom inspector
    /// </summary>
    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                planet.GeneratePlanet();
            }
        }

        ShowSettings(planet.colorSettings, planet.OnColorChanged, ref colorEditor, ref planet.colorFoldout);
        ShowSettings(planet.shapeSettings, planet.OnShapeChanged, ref shapeEditor, ref planet.shapeFoldout);

        Debug.Assert(planet.terrainSettings != null, "PlanetEditor/OnInspectorGUI: TerrainSettings was null");
        for (int i = 0; i < planet.terrainSettings.Length; i++)
        {
            Debug.Assert(i < planet.terrainFoldouts.Length, "PlanetEditor/OnInspectorGUI: TerrainFoldouts was null");
            Debug.Assert(i < planet.terrainSettings.Length, "PlanetEditor/OnInspectorGUI: TerrainSettings was null");
            ShowSettings(planet.terrainSettings[i], planet.OnShapeChanged, ref terrainEditor[i], ref planet.terrainFoldouts[i]);
        }
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
                                ref Editor e, ref bool foldout)
    {
        // No settings to show
        if (settings == null) return;

        // Draws the titleBar
        foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);

        // Checks for changes in the editor and updates the shape
        using var check = new EditorGUI.ChangeCheckScope();

        // Draws the settings editor
        if (foldout)
        {
            CreateCachedEditor(settings, null, ref e);
            e.OnInspectorGUI();
        }

        // Checks if there was a change
        if (!check.changed) return; // returns early if no changes in editor
        // Logic to call onChange
        onChange?.Invoke();
    }

}