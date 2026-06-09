using UnityEngine;

public class Gun : MonoBehaviour
{
    public Rigidbody2D rocket;
    public float speed = 15;
    [SerializeField] public PlayerControl playerCtrl;

    void Update()
    {
        if (playerCtrl != null)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (playerCtrl.bFaceRight)
                {
                    Rigidbody2D rocketInstance = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                    // 使用 transform.right 代替 Vector2(speed, 0)
                    rocketInstance.velocity = transform.right * speed;
                }
                else
                {
                    Rigidbody2D rocketInstance = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                    // 使用 -transform.right 代替 Vector2(-speed, 0)
                    rocketInstance.velocity = -transform.right * speed;
                }
            }
        }
    }
}