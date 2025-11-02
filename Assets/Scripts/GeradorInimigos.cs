using UnityEngine;
using System.Collections;

public class GeradorInimigos : MonoBehaviour
{
    // Prefabs de inimigos
    public GameObject inimigoChaoPrefab;
    public GameObject inimigoArPrefab;

    //Posições de spawn e destruição
    public float posXSpawn = 10f;        
    public float posXDestruicao = -12f;  
    public float yChao = -3.5f;
    public float yAr = 1.5f;

    //configurações de dificuldade
    public float intervaloInicial = 3f;
    public float intervaloMinimo = 0.7f;
    public float reducaoIntervaloPorSegundo = 0.02f;

    public float velocidadeInicial = 3f;
    public float velocidadeMaxima = 10f;
    public float aumentoVelocidadePorSegundo = 0.2f;

    private float proximoSpawn = 0f;

    void Update()
    {
        if (GameManager.instance == null) return;

        float tempo = GameManager.instance.GetTempoSobrevivencia();

        float intervaloAtual = Mathf.Max(intervaloInicial - tempo * reducaoIntervaloPorSegundo, intervaloMinimo);

        if (Time.time >= proximoSpawn)
        {
            GerarInimigo(tempo);
            proximoSpawn = Time.time + intervaloAtual;
        }
    }
       
        


    void GerarInimigo(float tempo)
    {
        
        bool gerarNoChao = Random.value > 0.5f;
        GameObject prefab = gerarNoChao ? inimigoChaoPrefab : inimigoArPrefab;

        if (prefab == null) return;

        float y = gerarNoChao ? yChao : yAr;
        Vector2 posicaoSpawn = new Vector2(posXSpawn, y);

        GameObject inimigo = Instantiate(prefab, posicaoSpawn, Quaternion.identity);

       
        float velocidade = Mathf.Min(velocidadeInicial + tempo * aumentoVelocidadePorSegundo, velocidadeMaxima);

        
        StartCoroutine(MoverInimigo(inimigo, velocidade));
    }

    IEnumerator MoverInimigo(GameObject inimigo, float velocidade)
    {
        while (inimigo != null)
        {
            inimigo.transform.Translate(Vector2.left * velocidade * Time.deltaTime);

            if (inimigo.transform.position.x < posXDestruicao)
            {
                Destroy(inimigo);
                yield break;
            }

            yield return null;
        }
    }
}
