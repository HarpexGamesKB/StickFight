using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class SettingChanger : EditorWindow
{
    private List<ColorStruct> colorStructs = new List<ColorStruct>();
    private SavedSettings savedColorStruct;

    private bool _useToon;

    private Vector2 scrollPosition;

    private float LH = EditorGUIUtility.singleLineHeight;
    private const float SP = 1.9f;
    private float leftXOffset;
    private float rightXOffset;
    private Color highlightColor;
    private Color changeColor;

    private GUIStyle textStyle_1;
    private GUIStyle textStyle_2;
    private GUIStyle buttonStyle_1;

    private int choosedSettingIndex = 0;
    private bool activated = false;

    private void DefineStyles()
    {
        changeColor = new Color(255, 255, 255, 0.5f);
        highlightColor = new Color(255, 255, 255, 0.1f);
        LH = EditorGUIUtility.singleLineHeight;
        leftXOffset = 4;
        rightXOffset = 8;
        textStyle_1 = new GUIStyle(EditorStyles.boldLabel);
        textStyle_1.alignment = TextAnchor.MiddleCenter;
        textStyle_1.fontSize = 14;
        textStyle_2 = new GUIStyle(EditorStyles.boldLabel);
        textStyle_2.alignment = TextAnchor.MiddleCenter;
        textStyle_2.fontSize = 24;

        buttonStyle_1 = new GUIStyle(EditorStyles.miniButtonMid);
        buttonStyle_1.fontStyle = FontStyle.Bold;
    }

    private void Save()
    {
        savedColorStruct.colorStructs = new List<ColorStruct>();
        for (int i = 0; i < colorStructs.Count; i++)
        {
            savedColorStruct.colorStructs.Add(new ColorStruct());
            savedColorStruct.colorStructs[i].material = colorStructs[i].material;
            savedColorStruct.colorStructs[i].colors = new List<Color>(colorStructs[i].colors);
            savedColorStruct.colorStructs[i].colorsToonCel = new List<Color>(colorStructs[i].colorsToonCel);
        }
        savedColorStruct.useToon = _useToon;
        EditorUtility.SetDirty(savedColorStruct);

        AssetDatabase.SaveAssets();
    }

    private void CreateDatabase()
    {
        savedColorStruct = CreateInstance<SavedSettings>();
        savedColorStruct.colorStructs = new List<ColorStruct>();
        if (!Directory.Exists("Assets/SettingChanger/SavedData"))
        {
            Directory.CreateDirectory("Assets/SettingChanger/SavedData");
        }
        AssetDatabase.CreateAsset(savedColorStruct, "Assets/SettingChanger/SavedData/SavedSettings.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void LoadDatabase()
    {
        savedColorStruct = AssetDatabase.LoadAssetAtPath<SavedSettings>("Assets/SettingChanger/SavedData/SavedSettings.asset");
        if (savedColorStruct == null)
            CreateDatabase();
        colorStructs = new List<ColorStruct>();
        for (int i = 0; i < savedColorStruct.colorStructs.Count; i++)
        {
            colorStructs.Add(new ColorStruct());
            colorStructs[i].material = savedColorStruct.colorStructs[i].material;
            colorStructs[i].colors = new List<Color>(savedColorStruct.colorStructs[i].colors);

            if (savedColorStruct.colorStructs[i].colorsToonCel.Count < savedColorStruct.colorStructs[i].colors.Count)
            {
                for (int c = savedColorStruct.colorStructs[i].colorsToonCel.Count; c < savedColorStruct.colorStructs[i].colors.Count; c++)
                {
                    savedColorStruct.colorStructs[i].colorsToonCel.Add(new Color(0, 0, 0, 1));
                }
            }

            colorStructs[i].colorsToonCel = new List<Color>(savedColorStruct.colorStructs[i].colorsToonCel);
        }
        _useToon = savedColorStruct.useToon;
    }

    [MenuItem("Tools/Setting Changer")]
    public static void ShowWindow()
    {
        GetWindow(typeof(SettingChanger));
    }

    private void OnEnable()
    {
        if (savedColorStruct == null)
            LoadDatabase();
    }

    private void StartBox(float YPosition, float height, bool boxStyled = false)
    {
        if (boxStyled)
        {
            GUILayout.BeginArea(new Rect(leftXOffset, YPosition, position.width - rightXOffset, height), EditorStyles.helpBox);
        }
        else
        {
            GUILayout.BeginArea(new Rect(leftXOffset, YPosition, position.width - rightXOffset, height));
        }
    }

    private void StartBox(float YPosition, float height, float leftOffset, float rightOffset, bool boxStyled = false)
    {
        if (boxStyled)
        {
            GUILayout.BeginArea(new Rect(leftOffset, YPosition, position.width - rightOffset, height), EditorStyles.helpBox);
        }
        else
        {
            GUILayout.BeginArea(new Rect(leftOffset, YPosition, position.width - rightOffset, height));
        }
    }

    private void StartBox(float YPosition, float height, float width, float leftOffset, float rightOffset, bool boxStyled = false)
    {
        if (boxStyled)
        {
            GUILayout.BeginArea(new Rect(leftOffset, YPosition, width, height), EditorStyles.helpBox);
        }
        else
        {
            GUILayout.BeginArea(new Rect(leftOffset, YPosition, width, height));
        }
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    private void Space(float height)
    {
        GUILayout.Space(height);
    }

    private void HS()
    {
        GUILayout.BeginHorizontal();
    }

    private void HSH(Color color)
    {
        GUIStyle style = new GUIStyle(EditorStyles.helpBox);
        style.padding = new RectOffset();
        style.normal.background = MakeTex(2, 2, color);
        GUILayout.BeginHorizontal(style);
    }

    private void HE()
    {
        GUILayout.EndHorizontal();
    }

    private void VS()
    {
        GUILayout.BeginVertical();
    }

    private void VSH(Color color)
    {
        GUIStyle style = new GUIStyle(EditorStyles.helpBox);
        style.padding = new RectOffset();
        style.normal.background = MakeTex(2, 2, color);
        GUILayout.BeginVertical(style);
    }

    private void VE()
    {
        GUILayout.EndVertical();
    }

    private void EndBox()
    {
        GUILayout.EndArea();
    }

    private void DrawHeader()
    {
        VS();
        {
            GUILayout.Space(LH);
            GUILayout.Label("Setting Changer", textStyle_2);
        }
        VE();
    }

    private void DrawAddingButtons()
    {
        VS();
        {
            Space(LH);
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Label("Color", textStyle_1);
            Space(LH / 2);
            HS();
            if (GUILayout.Button("+"))
            {
                foreach (ColorStruct colorStruct in colorStructs)
                {
                    if (colorStruct.colors.Count > 0)
                    {
                        colorStruct.colors.Add(colorStruct.colors[colorStruct.colors.Count - 1]);
                        colorStruct.colorsToonCel.Add(colorStruct.colorsToonCel[colorStruct.colorsToonCel.Count - 1]);
                    }
                    else
                    {
                        colorStruct.colors.Add(new Color(0, 0, 0, 1));
                        colorStruct.colorsToonCel.Add(new Color(0, 0, 0, 1));
                    }
                }
            }
            if (GUILayout.Button("-"))
            {
                if (colorStructs.Count != 0)
                {
                    int toRemove = choosedSettingIndex;
                    foreach (ColorStruct colorStruct in colorStructs)
                    {
                        if (colorStruct.colors.Count > toRemove)
                        {
                            colorStruct.colors.RemoveAt(toRemove);
                        }
                        if (colorStruct.colorsToonCel.Count > toRemove)
                        {
                            colorStruct.colorsToonCel.RemoveAt(toRemove);
                        }
                    }
                    if (toRemove > 0 && toRemove == colorStructs[0].colors.Count)
                    {
                        SwitchLeft();
                    }
                }
            }
            HE();
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Material", textStyle_1);
            Space(LH / 2);
            HS();
            if (GUILayout.Button("+"))
            {
                if (colorStructs.Count > 0)
                {
                    colorStructs.Add(new ColorStruct(colorStructs[colorStructs.Count - 1]));
                }
                else
                {
                    colorStructs.Add(new ColorStruct());
                }
            }
            if (GUILayout.Button("-"))
            {
                if (colorStructs.Count > 0)
                {
                    colorStructs.RemoveAt(colorStructs.Count - 1);
                }
            }
            HE();
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Clear", textStyle_1);
            Space(LH / 2);
            if (GUILayout.Button("x"))
            {
                choosedSettingIndex = 0;
                colorStructs.Clear();
            }
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }
        VE();
    }

    private int GetOveralColorsCount()
    {
        int count = 0;
        foreach (ColorStruct colorStruct in colorStructs)
        {
            count += colorStruct.colors.Count;
        }
        return count;
    }

    private void DrawList()
    {
        GUIStyle labelStyle = new GUIStyle(EditorStyles.boldLabel);
        labelStyle.alignment = TextAnchor.MiddleCenter;
        int CSC = colorStructs.Count;
        GUIStyle labelStyleHL = new GUIStyle(labelStyle);
        labelStyleHL.normal.textColor = new Color(1f, .4f, .4f, 1f);
        if (CSC <= 0)
        {
            return;
        }
        int CC = GetOveralColorsCount();

        float colorsSize = CC * (LH + SP) + (CC > 0 ? CSC * LH / 4 : 0);

        float materialsSize = CSC * (LH + SP) - SP;

        float rectInSize = LH * 1.5f + materialsSize + colorsSize + ((CSC - 1) * LH) + LH / 2;

        float rectOutSize = LH * 3;
        float fullSize = rectInSize + rectOutSize;

        VS();
        {
            Space(LH);
            GUILayout.Label("Settings", textStyle_1);
            Space(LH);

            scrollPosition = GUILayout.BeginScrollView(
                scrollPosition,
                GUILayout.Width(position.width),
                GUILayout.Height(position.height - LH * 13)
                );

            VSH(highlightColor);
            {
                Space(LH);
                for (int setting = 0; setting < colorStructs.Count; setting++)
                {
                    HS();
                    {
                        Space(LH);
                        GUILayout.Label(colorStructs[setting].material == null ? "Material" : colorStructs[setting].material.name, labelStyle, GUILayout.Width(position.width / 4));
                        colorStructs[setting].material = (Material)EditorGUILayout.ObjectField(colorStructs[setting].material, typeof(Material), true);
                        Space(LH);
                    }
                    HE();
                    for (int color = 0; color < colorStructs[setting].colors.Count; color++)
                    {
                        if (choosedSettingIndex == color)
                        {
                            HSH(highlightColor);
                        }
                        else
                        {
                            HS();
                        }
                        {
                            Space(LH);
                            GUILayout.Label("Base Color " + (color + 1), (choosedSettingIndex == color) ? labelStyleHL : labelStyle, GUILayout.Width((choosedSettingIndex == color) ? (position.width / 4 - SP * 2) : position.width / 4));
                            colorStructs[setting].colors[color] = (Color)EditorGUILayout.ColorField(colorStructs[setting].colors[color]);
                            if (_useToon)
                            {
                                GUILayout.Label("Cel Color " + (color + 1), (choosedSettingIndex == color) ? labelStyleHL : labelStyle, GUILayout.Width((choosedSettingIndex == color) ? (position.width / 4 - SP * 2) : position.width / 4));
                                colorStructs[setting].colorsToonCel[color] = (Color)EditorGUILayout.ColorField(colorStructs[setting].colorsToonCel[color]);
                            }
                            Space(LH);
                        }
                        HE();
                    }
                    if (colorStructs.Count != setting + 1)
                    {
                        Space(LH);
                    }
                }
                Space(LH);
            }
            VE();
            GUILayout.EndScrollView();
        }
        VE();
    }

    private void SwitchLeft()
    {
        choosedSettingIndex--;
        if (choosedSettingIndex < 0)
        {
            choosedSettingIndex = 0;
        }
    }

    private void SwitchRight()
    {
        choosedSettingIndex++;
        if (choosedSettingIndex > (GetOveralColorsCount() / colorStructs.Count) - 1)
        {
            choosedSettingIndex = (GetOveralColorsCount() / colorStructs.Count) - 1;
        }
    }

    private void SwitchButtons()
    {
        VS();
        {
            Space(SP);
            HS();
            {
                if (GUILayout.Button("<"))
                {
                    SwitchLeft();
                }
                if (GUILayout.Button(">"))
                {
                    SwitchRight();
                }
            }
            HE();
        }
        VE();
    }

    private void ChangeColorButton()
    {
        var style = new GUIStyle(GUI.skin.button);
        style.fontStyle = FontStyle.Bold;
        style.fontSize = 14;
        VS();
        {
            Space(SP);
            if (GUILayout.Button("Change Color", style, GUILayout.Height(LH * 2)))
            {
                ChangeColors();
            }
        }
        VE();
    }

    private void SaveButton()
    {
        StartBox(LH, LH * 2, position.width - LH * 4 - SP, SP);
        {
            Space(SP);
            if (GUILayout.Button("Save", GUILayout.Height(LH * 1.5f), GUILayout.Width(LH * 4)))
            {
                Save();
            }
        }
        EndBox();
    }

    private void LoadButton()
    {
        StartBox(LH, LH * 2, position.width - LH * 8 - SP, SP);
        {
            Space(SP);
            if (GUILayout.Button("Load", GUILayout.Height(LH * 1.5f), GUILayout.Width(LH * 4)))
            {
                LoadDatabase();
            }
        }
        EndBox();
    }

    private void ChangeColors()
    {
        if (GetOveralColorsCount() > 0)
        {
            foreach (ColorStruct colorStruct in colorStructs)
            {
                colorStruct.material.color = colorStruct.colors[choosedSettingIndex];
                if (_useToon)
                {
                    colorStruct.material.SetColor("_ColorDim", colorStruct.colorsToonCel[choosedSettingIndex]);
                }
            }
        }
    }

    private void ToonToggle()
    {
        StartBox(LH * 2.5f + SP * 2, LH * 2, position.width - LH * 8 - SP / 2, SP);
        {
            _useToon = GUILayout.Toggle(_useToon, "Toon");
        }
        EndBox();
    }

    private void OnGUI()
    {
        DefineStyles();

        DrawHeader();

        DrawAddingButtons();

        ToonToggle();

        LoadButton();

        SaveButton();

        SwitchButtons();

        ChangeColorButton();

        DrawList();
    }

    public override void SaveChanges()
    {
        base.SaveChanges();
    }
}