using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float[] Levels;

    [Header("Props")]
    [SerializeField] private float Duration;

    [SerializeField] private Ease Ease = Ease.InOutSine;
    public int LevelCount => Levels.Length;
    public int CurrentLevel { get; private set; }

    private void Start()
    {
        //MoveToLevel(_currentLevel);
    }

    [ContextMenu("Zoom Out")]
    public void ZoomOut()
    {
        if (CurrentLevel + 1 < Levels.Length)
        {
            CurrentLevel++;
            MoveToLevel(CurrentLevel);
        }
    }

    public void ZoomIn()
    {
        if (CurrentLevel - 1 >= 0)
        {
            CurrentLevel--;
            MoveToLevel(CurrentLevel);
        }
    }

    public void ResetZoom()
    {
        CurrentLevel = 0;
        MoveToLevel(CurrentLevel);
    }

    private void MoveToLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= Levels.Length) return;

        transform.DOLocalMove(transform.forward * -Levels[levelIndex] * 1.06417790926f - Vector3.forward * Levels[levelIndex] / 15, Duration).SetEase(Ease);
    }
}