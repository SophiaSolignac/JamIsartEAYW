using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;




public class EnemySpawner : MonoBehaviour
{
    #region StateMachine

    delegate void RusherMovementDelegate();
    RusherMovementDelegate _Action;
    public void SetModeVoid()
    {
        StopAllCoroutines();
        _Action = DoVoid;
    }
    private void DoVoid() { }
    public void SetModeSpawn(bool resetAction = true)
    {
        if (resetAction) { _Action = null; }
        _Action -= DoSpawn;
        _Action += DoSpawn;
        StartCoroutine(DelaySpawnEnemy(1f / _MinEnemyPerSec, true, -1));
    }


    private Vector3 _Direction;
    Transform _Target;
    private void DoSpawn()
    {
    }

    #endregion
    [Header("Objects"),Space(5)]
    [SerializeField]
    Enemy _EnemyPrefab;
    List<Enemy> _AllEnemies = new List<Enemy>(); //A REGLER : SUPPRIMER ENNEMIS QUAND DETRUIT PAR PLAYER !!!!!!!!!!!!!!!!
    [SerializeField]
    GameObject _ScoreLabel; 
    [SerializeField]
    GameObject _GameOverLabel;
    [SerializeField]
    GameObject _Hud;
    [SerializeField]
    GameObject _GameOverHud;
    [SerializeField]
    Player _Player;

    [Header("Difficulty"), Space(5)]
    [SerializeField]
    AnimationCurve _DifficultyCurve = default;
    [SerializeField, Tooltip("The time at which the difficulty is maxed in seconds")]
    float _MaxDifficultyTime = 60;
    [SerializeField]
    float _MinEnemyPerSec = 1f;
    float _EnemyPerSec = 1f;
    [SerializeField]
    float _MaxEnemyPerSec = 5f;


    private float _CurrentDifficulty = 0f;
    float _Score = 0; 
    private bool _IsGameOver;
    private int offset = 2;


    // Start is called before the first frame update
    void Start()
    {
        _Player._HitPlayer.AddListener(GameOver);
        SetScore(0);
        SetModeSpawn();
    }

    private IEnumerator DelaySpawnEnemy(float time = 1f, bool repeat = false, int nbLoop = 1)
    {

        yield return new WaitForSeconds(time); 
        Vector2 randPositivePos = Camera.main.ScreenToWorldPoint(new Vector3(UnityEngine.Random.value *( Screen.width + offset), UnityEngine.Random.value * (Screen.height + offset)));
        Vector2 position = randPositivePos * new Vector2(UnityEngine.Random.Range(-1,1) == 0 ? 1 : -1, UnityEngine.Random.Range(-1, 1) == 0 ? 1 : -1);
        _AllEnemies.Add(CreateEnemy(position));
        if (repeat) 
        {
            if (nbLoop > 0) nbLoop--;
            if (nbLoop == 1) repeat = false;
            StartCoroutine(DelaySpawnEnemy(1f / _EnemyPerSec, repeat,nbLoop));
        }
    }

    // Update is called once per frame
    void Update()
    {
        _Action();
    }

    private void UpdateDifficulty()
    {
        _CurrentDifficulty = _DifficultyCurve.Evaluate(Time.timeSinceLevelLoad / _MaxDifficultyTime);
        _EnemyPerSec =  Mathf.Lerp(_MinEnemyPerSec,_MaxEnemyPerSec,_CurrentDifficulty);
    }

    private Enemy CreateEnemy(Vector2 position,float rotationAngle = 0f)
    {
        Enemy enemy = Instantiate(_EnemyPrefab);
        enemy.transform.parent = transform; 
        enemy.transform.position = position;
        enemy.transform.Rotate(Vector3.forward, rotationAngle);
        enemy._HitSword.AddListener(KilledEnemy);
        return enemy;

    }
    private void ApplyScoreToLabel(float score)
    {
        _ScoreLabel.GetComponent<TextMeshProUGUI>().text = score.ToString();
        _GameOverLabel.GetComponent<TextMeshProUGUI>().text = "Final score : " + score.ToString();
    }
    private void SetScore(float score)
    {
        _Score = score;
        ApplyScoreToLabel(_Score);
    }

    private void AddScore(float score = 500f) 
    {
        SetScore(_Score + score);
    }
    private void KilledEnemy()
    {
        AddScore(Enemy._ScoreValue);
        _Player._Grow();
    }

    private void UpdateVisibleHud()
    {
        _GameOverHud.SetActive(_IsGameOver);
        _Hud.SetActive(!_IsGameOver);
    }
    private void GameOver()
    {
        _IsGameOver = true;
        SetModeVoid();
        UpdateVisibleHud();
        Time.timeScale = 0f;
    }
    public void Restart()
    {
        _IsGameOver = false;
        for (int i = _AllEnemies.Count - 1; i >= 0; i--)
        {
            Enemy enemy = _AllEnemies[i];
            print(enemy);
            if (enemy == null) 
            { 
                _AllEnemies.Remove(enemy);
                continue; 
            }
            Destroy(enemy.gameObject);
        }
        SetScore(0);
        UpdateVisibleHud();
        SetModeSpawn();
        _Player.SetSize(1f);
        Time.timeScale = 1f;


    }
}
