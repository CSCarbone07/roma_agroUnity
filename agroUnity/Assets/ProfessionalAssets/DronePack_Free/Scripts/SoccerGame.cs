using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PA_DronePack_Free
{
    public class SoccerGame : MonoBehaviour
    {
        public int score = 0;
        public GameObject goalie;
        public Text scoreboard;
        public GameObject ball;
        public float maxDistance = 100;
        public Vector3 spawn;
        bool resettingBall;

        void Awake()
        {
            ball = GameObject.Find("SoccerBall");
            goalie = GameObject.Find("SoccerGoalie");
            scoreboard = GameObject.Find("ScoreText").GetComponent<Text>();
            spawn = ball.transform.position;
            StartCoroutine(CheckDistance());
        }

        IEnumerator CheckDistance()
        {
            while (true)
            {
                if (Vector3.Distance(transform.position, ball.transform.position) > maxDistance && !resettingBall)
                {
                    StartCoroutine(ResetBall());
                }
                yield return new WaitForSeconds(1f);
            }
        }

        IEnumerator ResetBall()
        {
            resettingBall = true;
            yield return new WaitForSeconds(3f);
            ball.GetComponent<ParticleSystem>().Play();
            while (ball.GetComponent<MeshRenderer>().material.color.a > 0.01f)
            {
                Color cColor = ball.GetComponent<MeshRenderer>().material.color;
                Color nColor = Color.Lerp(cColor, new Vector4(1, 1, 1, 0), Time.deltaTime * 2f);
                ball.GetComponent<MeshRenderer>().material.color = nColor;
                ball.GetComponent<MeshRenderer>().material.SetFloat("_Glossiness", ball.GetComponent<MeshRenderer>().material.color.a);
                yield return null;
            }
            ball.GetComponent<ParticleSystem>().Stop();
            ball.GetComponent<MeshRenderer>().material.color = new Vector4(1, 1, 1, 1);
            ball.GetComponent<MeshRenderer>().material.SetFloat("_Glossiness", 0.6f);
            ball.transform.position = spawn + new Vector3(0, 7, 0);
            ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            resettingBall = false;
        }

        void OnTriggerEnter(Collider col)
        {
            if (col == ball.GetComponent<Collider>() && !resettingBall)
            {
                score += 1;
                goalie.GetComponent<Animation>()[goalie.GetComponent<Animation>().clip.name].speed += 1;
                scoreboard.text = score.ToString();
                StartCoroutine(ResetBall());
            }
        }
    }
}
