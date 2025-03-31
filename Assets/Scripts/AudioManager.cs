using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [Header("Музыка меню")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private float musicVolume = 0.5f;
    [SerializeField] private float fadeDuration = 1.0f;
    
    private AudioSource musicSource;
    private bool isFading = false;
    private float fadeTimer = 0f;
    private float startVolume = 0f;
    private float targetVolume = 0f;
    
    private void Awake()
    {
        Debug.Log("AudioManager: Awake");
        // Singleton реализация
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Создаем AudioSource, если его нет
            musicSource = GetComponent<AudioSource>();
            if (musicSource == null)
            {
                musicSource = gameObject.AddComponent<AudioSource>();
                Debug.Log("AudioManager: AudioSource добавлен");
            }
            
            // Базовые настройки
            musicSource.loop = true;
            musicSource.volume = musicVolume;
            musicSource.playOnAwake = false;
            
            // Подписываемся на событие загрузки сцены
            SceneManager.sceneLoaded += OnSceneLoaded;
            
            // Проверяем наличие музыкального клипа
            if (menuMusic == null)
            {
                Debug.LogError("AudioManager: menuMusic не задан в инспекторе!");
            }
        }
        else if (Instance != this)
        {
            Debug.Log("AudioManager: Уничтожен дублирующий экземпляр");
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        // Запускаем музыку для текущей сцены
        Scene currentScene = SceneManager.GetActiveScene();
        CheckSceneForMusic(currentScene);
        Debug.Log("AudioManager: Start - проверка начальной сцены: " + currentScene.name);
    }
    
    private void OnDestroy()
    {
        // Отписываемся от события при удалении объекта
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Debug.Log("AudioManager: OnDestroy");
    }
    
    private void Update()
    {
        // Обработка плавного изменения громкости
        if (isFading)
        {
            ProcessFading();
        }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("AudioManager: Загружена сцена: " + scene.name);
        CheckSceneForMusic(scene);
    }
    
    private void CheckSceneForMusic(Scene scene)
    {
        // Проверяем, какая сцена загружена
        if (scene.name == "MainMenuScene")
        {
            // Воспроизводим музыку меню
            PlayMenuMusic();
        }
        else
        {
            // На всех других сценах останавливаем музыку
            StopMusic();
            Debug.Log("AudioManager: Музыка остановлена, так как текущая сцена не MainMenuScene");
        }
    }
    
    public void PlayMenuMusic()
    {
        Debug.Log("AudioManager: PlayMenuMusic вызван");
        if (musicSource != null && menuMusic != null)
        {
            // Если сейчас играет другой трек или не играет совсем
            if (musicSource.clip != menuMusic || !musicSource.isPlaying)
            {
                Debug.Log("AudioManager: Начинаем воспроизведение музыки меню");
                musicSource.clip = menuMusic;
                
                // Если источник не играет, начинаем с нулевой громкости
                if (!musicSource.isPlaying)
                {
                    musicSource.volume = 0f;
                    musicSource.Play();
                    StartFade(0f, musicVolume);
                    Debug.Log("AudioManager: Музыка запущена с начала");
                }
                // Если играет другой трек, делаем кроссфейд
                else
                {
                    float currentVolume = musicSource.volume;
                    musicSource.Stop();
                    musicSource.volume = 0f;
                    musicSource.Play();
                    StartFade(0f, musicVolume);
                    Debug.Log("AudioManager: Смена трека с кроссфейдом");
                }
            }
            else
            {
                Debug.Log("AudioManager: Музыка меню уже играет, громкость: " + musicSource.volume);
            }
        }
        else
        {
            if (musicSource == null)
                Debug.LogError("AudioManager: musicSource не существует!");
            if (menuMusic == null)
                Debug.LogError("AudioManager: menuMusic не задан!");
        }
    }
    
    public void StopMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            StartFade(musicSource.volume, 0f);
            Debug.Log("AudioManager: Остановка музыки");
        }
    }
    
    // Запуск плавного изменения громкости
    private void StartFade(float from, float to)
    {
        startVolume = from;
        targetVolume = to;
        fadeTimer = 0f;
        isFading = true;
        Debug.Log($"AudioManager: Начат фейд от {from} до {to}");
    }
    
    // Обработка плавного изменения громкости
    private void ProcessFading()
    {
        if (musicSource != null)
        {
            fadeTimer += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(fadeTimer / fadeDuration);
            
            musicSource.volume = Mathf.Lerp(startVolume, targetVolume, t);
            
            if (t >= 1f)
            {
                isFading = false;
                Debug.Log("AudioManager: Фейд завершен, громкость: " + musicSource.volume);
                
                // Если громкость стала нулевой - останавливаем воспроизведение
                if (targetVolume <= 0f && musicSource.isPlaying)
                {
                    musicSource.Stop();
                    Debug.Log("AudioManager: Музыка остановлена");
                }
            }
        }
        else
        {
            isFading = false;
            Debug.LogError("AudioManager: Потерян musicSource во время фейда");
        }
    }
    
    // Метод для установки громкости музыки
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null && !isFading)
        {
            musicSource.volume = musicVolume;
            Debug.Log("AudioManager: Громкость установлена на " + musicVolume);
        }
    }
}