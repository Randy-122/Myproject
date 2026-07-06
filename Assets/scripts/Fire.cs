using UnityEngine;
using UnityEngine.EventSystems;

public class Gun : MonoBehaviour
{
    // 1. 将类型改为 Rigidbody2D，这样你可以直接拖拽预制体上的刚体组件
    public Rigidbody2D rocket;

    public float speed = 15;
    [SerializeField] public PlayerControl playerCtrl;

    AudioSource audioSource;
    private Animator animator;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = transform.parent.GetComponent<Animator>();
    }

    void Update()
    {
        // 2. 安全检查
        if (playerCtrl != null && rocket != null)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                if (Time.timeScale == 0) 
                {
                    return; // 游戏暂停时不发射火箭
                }

                if (audioSource != null) audioSource.Play();
                animator.SetTrigger("shoot");
                float zRotation = playerCtrl.bFaceRight ? 0f : 180f;

                // 3. 实例化 Rigidbody2D 组件
                Rigidbody2D rocketInstance = Instantiate(rocket, transform.position, Quaternion.Euler(0, 0, zRotation));

                // 4. 直接给实例化出的刚体赋值速度
                rocketInstance.velocity = (playerCtrl.bFaceRight ? transform.right : -transform.right) * speed;
            }
        }
    }
}