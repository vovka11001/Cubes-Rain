using TMPro;
using UnityEngine;

public class SpawnerStatsView : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;
    [SerializeField] private string _title;

    private ISpawnerStats _stats;

    private void Awake()
    {
        _stats = GetComponentInParent<ISpawnerStats>();
        _label.color = Color.black;
    }

    private void Update()
    {
        _label.text = $"{_title}\n" +
                      $"Создано: {_stats.CreatedCount}\n" +
                      $"Заспавнено: {_stats.TotalSpawnedCount}\n" +
                      $"Активно: {_stats.ActiveCount}";
    }
}