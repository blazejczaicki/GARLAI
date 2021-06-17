using System.Collections;
using System.Collections.Generic;
using TopShooter;
using UnityEngine;

public enum KindOfArea
{
    Square,
    Circle
}

public class AreaController : MonoBehaviour
{
    [SerializeField] private KindOfArea kindOfArea;
    private List<Enemy> enemies;

    public KindOfArea KindOfArea { get => kindOfArea; set => kindOfArea = value; }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy!=null)
        {
            enemies.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemies.Remove(enemy);
        }
    }
}
