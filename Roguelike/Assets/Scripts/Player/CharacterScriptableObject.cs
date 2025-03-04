using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "ScriptableObjects/Character")]
public class CharacterScriptableObject : ScriptableObject
{
    // Значок персонажа
    [SerializeField] Sprite icon;
    public Sprite Icon { get => icon; private set => icon = value; }

    // Имя персонажа
    [SerializeField] new string name;
    public string Name { get => name; private set => name = value; }

    // Начальное оружие
    [SerializeField] GameObject startingWeapon;
    public GameObject StartingWeapon { get => startingWeapon; private set => startingWeapon = value; }

    // Максимальное здоровье
    [SerializeField] float maxHealth;
    public float MaxHealth { get => maxHealth; private set => maxHealth = value; }

    // Регенерация
    [SerializeField] float recovery;
    public float Recovery { get => recovery; private set => recovery = value; }

    // Скорость передвижения
    [SerializeField] float moveSpeed;
    public float MoveSpeed { get => moveSpeed; private set => moveSpeed = value; }

    // Сила атаки
    [SerializeField] float might;
    public float Might { get => might; private set => might = value; }

    // Скорость снарядов
    [SerializeField] float projectileSpeed;
    public float ProjectileSpeed { get => projectileSpeed; private set => projectileSpeed = value; }

    // Сила магнита
    [SerializeField] float magnet;
    public float Magnet { get => magnet; private set => magnet = value; }
}
