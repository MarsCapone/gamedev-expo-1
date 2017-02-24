using UnityEngine;

[System.Serializable]
public abstract class AnimalBehaviour : MonoBehaviour
{
    public float startTime;
    public bool enabled = true;

    public abstract void perform(Animal animal);

    public void activate(Animal animal)
    {
        if (enabled) perform(animal);
    }
}