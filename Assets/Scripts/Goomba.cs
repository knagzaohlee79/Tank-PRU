using UnityEngine;

public class Goomba : MonoBehaviour
{
    public Sprite flatSprite;

    private void OnCollisionEnter2D(Collision2D collision) // hàm xử lý va chạm của Goomba
    {
        if (collision.gameObject.CompareTag("Player")) // nếu va chạm với Player
        {
            Player player = collision.gameObject.GetComponent<Player>(); // lấy component Player 

            if (player.starpower) // nếu player đang trong trạng thái starpower
            {
                Hit(); // gọi hàm Hit() để xử lý Goomba bị đạp
            } else if (collision.transform.DotTest(transform, Vector2.down)) // nếu player đang ở trên Goomba và va chạm với Goomba từ phía trên
            {
                Flatten(); // gọi hàm Flatten() để xử lý Goomba bị đạp
            } else {
                player.Hit(); // gọi hàm Hit() của Player để xử lý Player bị thua
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Shell")) {
            Hit();
        }
    }

    private void Flatten()
    {
        GetComponent<Collider2D>().enabled = false; // tắt Collider2D
        GetComponent<EntityMovement>().enabled = false; // tắt EntityMovement
        GetComponent<AnimatedSprite>().enabled = false; // tắt AnimatedSprite
        GetComponent<SpriteRenderer>().sprite = flatSprite; // đổi sprite của Goomba thành sprite flatSprite
        Destroy(gameObject, 0.5f); // sau 0.5 giây hủy gameObject
    }

    private void Hit()
    {
        GetComponent<AnimatedSprite>().enabled = false; // tắt AnimatedSprite
        GetComponent<DeathAnimation>().enabled = true; // bật DeathAnimation
        Destroy(gameObject, 3f); // sau 3 giây hủy gameObject
    }

}
