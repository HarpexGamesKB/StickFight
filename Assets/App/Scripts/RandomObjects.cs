using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjects : Singleton<RandomObjects>
{
    [System.Serializable]
    public class ColorSet
    {
        public string name;
        public Color background;
        public Color cubeBase;
        public Color cubeCel;
        public Color groundBase;
        public Color groundCel;
        public Mesh backgroundMesh;
    }

    [SerializeField] private Material Material;
    [SerializeField] private Material GroundMaterial;
    [SerializeField] private Material SceneObjectsMaterial;
    [SerializeField] private int XCount;
    [SerializeField] private int ZCount;
    [SerializeField] private float MinScale;
    [SerializeField] private Vector3 MaxScale;
    [SerializeField] private Vector3 Offset;
    [SerializeField] private ColorSet[] Colors;
    [SerializeField] private int[] UseIndexes;
    private List<MeshFilter> Objects = new List<MeshFilter>();

    private void Start()
    {
        R2();
        OSLevelManager.Instance.OnLevelLoaded += ChangeByLevel;
        // StartCoroutine(Animate());
    }

    private void ChangeByLevel(int levelIndex, OSLevelManager.Level level)
    {
        Change(UseIndexes[levelIndex]);
    }

    private void Change(int index)
    {
        if (UseIndexes[index] >= Colors.Length) return;

        Camera.main.backgroundColor = Colors[UseIndexes[index]].background;
        //RenderSettings.fog = true;
        RenderSettings.fogColor = Colors[UseIndexes[index]].background;

        Material.color = Colors[UseIndexes[index]].cubeBase;
        Material.SetColor("_ColorDim", Colors[UseIndexes[index]].cubeCel);
        Material.SetColor("_ColorGradient", Colors[UseIndexes[index]].background);
        if (GroundMaterial != null)
        {
            GroundMaterial.color = Colors[UseIndexes[index]].groundBase;
            GroundMaterial.SetColor("_ColorDim", Colors[UseIndexes[index]].groundCel);
        }

        SceneObjectsMaterial.color = Colors[UseIndexes[index]].background;
        SceneObjectsMaterial.SetColor("_ColorDim", Colors[UseIndexes[index]].background / 1.3f);
        SceneObjectsMaterial.SetColor("_ColorGradient", Colors[UseIndexes[index]].groundBase / 1.3f);
        for (int i = 0; i < Objects.Count; i++)
        {
            Objects[i].sharedMesh = Colors[UseIndexes[index]].backgroundMesh;
        }
    }

    private void R2()
    {
        for (int x = 0; x < XCount; x++)
        {
            if (x == 1 || x == 2) continue; // Timed
            for (int z = 0; z < ZCount; z++)
            {
                Vector3 scale = MaxScale;
                Transform Object = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
                Objects.Add(Object.GetComponent<MeshFilter>());
                Object.GetComponent<Renderer>().material = Material;
                Object.hideFlags = HideFlags.HideInHierarchy;
                Object.parent = transform;
                Vector3 position = transform.position + Vector3.right * x * (scale.x + Offset.x) + Vector3.forward * z * (scale.z + Offset.z);
                Vector3 localScale = scale;

                Object.position = position;
                Object.localScale = localScale;
                Object.eulerAngles = Vector3.up * Random.Range(0, 360);
            }
        }
    }

    private void R1()
    {/*
        for (int x = 0; x < XCount; x++)
        {
            for (int z = 0; z < ZCount; z++)
            {
                float scale = (MaxScale - MinScale) / 2;
                Transform Object = GameObject.CreatePrimitive(Random.Range(0, 2) == 0 ? PrimitiveType.Cube : PrimitiveType.Sphere).transform;
                Object.GetComponent<Renderer>().material = Material;
                Quaternion rotation = new Quaternion(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                Vector3 position = transform.position + Vector3.right * x * (scale + Offset.x) + Vector3.forward * z * (scale + Offset.z) + Vector3.up * Random.Range(-Offset.y, Offset.y);
                Vector3 localScale = Vector3.one * Random.Range(0, 9f);

                Object.position = position;
                Object.rotation = rotation;
                Object.localScale = localScale;

                //Objects.Add(Object.gameObject);
                Object.gameObject.AddComponent<Translate>().Activate(Vector3.up * Random.Range(-1.5f, 1.5f), Random.Range(3f, 5f), true);
                Object.gameObject.AddComponent<Rotate>().Activate(new Vector3(360 * Random.Range(0, 3), 360 * Random.Range(0, 3), 360 * Random.Range(0, 3)), Random.Range(20f, 45f), true);
            }
        }*/
    }

    /*private IEnumerator Animate()
    {
        WaitForSeconds delay = new WaitForSeconds(.05f);
        for (int i = 0; i < Objects.Count; i++)
        {
            Objects[i].AddComponent<Translate>().Activate(Vector3.up * 2, 3, true);
            yield return delay;
        }
    }*/

    private void OnDrawGizmosSelected()
    {
        //float scale = (MaxScale - MinScale) / 2;
        Vector3 scale = MaxScale;
        int counter = 0;
        for (int x = 0; x < XCount; x++)
        {
            for (int z = 0; z < ZCount; z++)
            {
                Gizmos.DrawCube(transform.position + Vector3.right * x * (scale.x + Offset.x) + Vector3.forward * z * (scale.z + Offset.z) + Vector3.up * (counter % 2 == 0 ? -Offset.y : Offset.y), scale);
                counter++;
            }
        }
    }
}