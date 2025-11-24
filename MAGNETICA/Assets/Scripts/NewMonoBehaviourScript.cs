using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float scrollAmount;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Vector3 moveDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        if (transform.position.x <= -scrollAmount)
        {
            transform.position = target.position - moveDirection * scrollAmount;
        }
    }
}
