using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
    public int WallDamage = 1;
    public int PointsPerFood = 10;
    public int PointsPerSoda = 20;
    public float RestartLevelDelay = 1f;
    public Text FoodText;

    private Animator _animator;
    private int _food;

    // Start is called before the first frame update
    protected override void Start()
    {
        _animator = GetComponent<Animator>();
        _food = GameManager.Instance.PlayerFoodPoints;
        FoodText.text = "Food: " + _food;

        base.Start();
    }

    private void OnDisable()
    {
        GameManager.Instance.PlayerFoodPoints = _food;
    }

    private void CheckIfGameOver()
    {
        if (_food <= 0)
            GameManager.Instance.GameOver();
    }

    protected override void AttemptMove<T>(float xDir, float yDir)
    {
        // _food--;
        FoodText.text = "Food " + _food;

        base.AttemptMove<T>(xDir, yDir);

        CheckIfGameOver();

        // GameManager.Instance.PlayersTurn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.PlayersTurn)
            return;

        float horizontal = 0;
        float vertical = 0;

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
            vertical = 0;

        if (horizontal != 0 || vertical != 0)
            AttemptMove<Wall>(horizontal, vertical);
    }

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(WallDamage);
        _animator.SetTrigger("PlayerChop");
    }

    private void Restart()
    {
        // Application.LoadLevel(Application.loadedLevel);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoseFood(int loss)
    {
        _animator.SetTrigger("PlayerHit");
        _food -= loss;
        FoodText.text = "-" + loss + " Food: " + _food;
        CheckIfGameOver();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", RestartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            _food += PointsPerFood;
            FoodText.text = "+" + PointsPerFood + " Food:" + _food;
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            _food += PointsPerSoda;
            FoodText.text = "+" + PointsPerSoda + " Food:" + _food;

            other.gameObject.SetActive(false);
        }
    }
}
