using UnityEngine;

[System.Serializable]
public abstract class AnimalBehaviour : MonoBehaviour
{
    public float startTime;

    public abstract void perform(Animal animal);
}