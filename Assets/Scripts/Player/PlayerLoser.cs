using System.Collections.Generic;
using UnityEngine;

public class PlayerLoser : MonoBehaviour
{

    [SerializeField]
    private float minVelocityThreshold = 1;

    [SerializeField]
    private float catchProgessIncrementFactor = 1;
    [SerializeField]
    private float catchProgessDecrementFactor = 1;
    
    private bool locked = false;

    private readonly HashSet<Collider2D> enemiesInRange = new();

    private ObservableVariable<float> catchProgess = new(0);

    private Rigidbody2D carBody;

    void Start()
    {
        carBody = GetComponentInParent<Rigidbody2D>();
    }

    void Update()
    {
        if (locked) return;

        if (carBody.linearVelocity.magnitude < minVelocityThreshold && enemiesInRange.Count > 0) {
            catchProgess.Value = Mathf.MoveTowards(catchProgess.Value, 1, catchProgessIncrementFactor * Time.deltaTime);
        } else {
            catchProgess.Value = Mathf.MoveTowards(catchProgess.Value, 0, catchProgessDecrementFactor * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Enemy)) {
            enemiesInRange.Add(collision);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        enemiesInRange.Remove(collision);
    }

    public void Lock() {
        locked = true;
        catchProgess.Value = 1;
    }

    public float CatchProgess => catchProgess.Value;

    public event ObservableVariable<float>.ValueChangedDelegate CatchProgessChanged
    {
        add => catchProgess.ValueChanged += value;
        remove => catchProgess.ValueChanged -= value;
    }
}
