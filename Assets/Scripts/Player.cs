using System.Collections;
using UnityEngine;

// Lớp Player xử lý trạng thái và hoạt ảnh của nhân vật chơi.
public class Player : MonoBehaviour
{
    // Tham chiếu đến 2 renderer kích cỡ của nhân vật nhỏ và lớn.
    public PlayerSpriteRenderer smallRenderer;
    public PlayerSpriteRenderer bigRenderer;
    // Renderer sprite đang hoạt động.
    private PlayerSpriteRenderer activeRenderer;


    // collider hình dạng cho nhân vật.
    public CapsuleCollider2D capsuleCollider { get; private set; }
    // Thành phần xử lý hoạt ảnh của nhân vật chết.
    public DeathAnimation deathAnimation { get; private set; }

    // kiểm tra xem nhân vật có ở trạng thái to hay không.
    public bool big => bigRenderer.enabled;
    // kiểm tra xem nhân vật có đang ở trạng thái chết hay không 
    public bool dead => deathAnimation.enabled;
    // kiểm tra xem nhân vật có đang ở dạng starpower hay không.
    public bool starpower { get; private set; }

    // khởi tạo các thành phần.
    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>(); // lấy collider hình dạng của nhân vật.
        deathAnimation = GetComponent<DeathAnimation>(); // lấy thành phần xử lý hoạt ảnh chết của nhân vật.
        activeRenderer = smallRenderer; // mặc định renderer đang hoạt động là renderer nhân vật nhỏ.
    }
    // hàm xử lý khi nhân vật chạm vào vật thể.
    public void Hit()
    {// nếu nhân vật đang ở trạng thái starpower thì không xử lý.
        if (!dead && !starpower)
        {
            // nếu nhân vật đang ở trạng thái to thì chuyển về trạng thái nhỏ.
            if (big) {
                Shrink();
            } // nếu nhân vật đang ở trạng thái nhỏ thì chuyển về trạng thái chết.
            else {
                Death();
            }
        }
    }
    // hàm xử lý nhân vật chết.
    public void Death()
    {
        
        smallRenderer.enabled = false; // tắt renderer nhân vật nhỏ.
        bigRenderer.enabled = false; // tắt renderer nhân vật lớn.
        deathAnimation.enabled = true; // bật hoạt ảnh chết.

        GameManager.Instance.ResetLevel(3f); // reset lại level sau 3s.
    }

    // hàm xử lý nhân vật ăn nấm và biến to.
    public void Grow()
    {
        smallRenderer.enabled = false; // tắt renderer nhân vật nhỏ.
        bigRenderer.enabled = true; // bật renderer nhân vật lớn.
        activeRenderer = bigRenderer; // renderer đang hoạt động là renderer nhân vật lớn.

        capsuleCollider.size = new Vector2(1f, 2f); // thay đổi kích thước collider.
        capsuleCollider.offset = new Vector2(0f, 0.5f); // thay đổi vị trí collider.

        StartCoroutine(ScaleAnimation()); // bắt đầu hoạt ảnh chuyển đổi kích thước.
    }
    // hàm xử lý nhân vật biến nhỏ
    public void Shrink()
    {
        smallRenderer.enabled = true; // vào trạng thái nhỏ.
        bigRenderer.enabled = false;
        activeRenderer = smallRenderer;

        capsuleCollider.size = new Vector2(1f, 1f);
        capsuleCollider.offset = new Vector2(0f, 0f);

        StartCoroutine(ScaleAnimation());
    }
    // hàm xử lý hoạt ảnh chuyển đổi kích thước.
    private IEnumerator ScaleAnimation()
    {
        float elapsed = 0f; // thời gian đã trôi qua.
        float duration = 0.5f; // thời gian hoạt ảnh.

        while (elapsed < duration) // nếu thời gian trôi qua nhỏ hơn thời gian hoạt ảnh
        {
            elapsed += Time.deltaTime; // cộng thời gian trôi qua với thời gian thực.

            if (Time.frameCount % 4 == 0) // nếu frame hiện tại chia hết cho 4 thì đổi renderer. chia 4 là vì mỗi giây có 60 frame và mỗi frame thực hiện 4 lần.
            {
                smallRenderer.enabled = !smallRenderer.enabled; // đổi renderer nhỏ.
                bigRenderer.enabled = !smallRenderer.enabled; // đổi renderer lớn.
            }

            yield return null; // chờ 1 frame.
        }

        smallRenderer.enabled = false; // tắt renderer nhỏ.
        bigRenderer.enabled = false; // tắt renderer lớn.
        activeRenderer.enabled = true; // bật renderer đang hoạt động.
    }

    public void Starpower()
    {
        StartCoroutine(StarpowerAnimation()); // bắt đầu hoạt ảnh starpower.
    }

    private IEnumerator StarpowerAnimation()
    {
        starpower = true; // đang ở trạng thái starpower.

        float elapsed = 0f; // thời gian trôi qua.
        float duration = 10f; // thời gian starpower = 10s.

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime; // cộng thời gian trôi qua với thời gian thực.

            if (Time.frameCount % 4 == 0)  // nếu frame hiện tại chia hết cho 4 thì đổi renderer. chia 4 là vì mỗi giây có 60 frame và mỗi frame thực hiện 4 lần.
            {
                activeRenderer.spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f); // đổi màu renderer. 
            }

            yield return null;
        }

        activeRenderer.spriteRenderer.color = Color.white; // đổi màu renderer về màu trắng.
        starpower = false; // kết thúc starpower.
    }

}
