using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonesBlend : MonoBehaviour
{
    [SerializeField] GameObject beetLeaf;
    [SerializeField] Vector3 beetLeafScale = new Vector3(1, 1, 1);

    // control the spawing ratio, remember to check also Invoke() functions
    private float spawnDelay = 3f;
    private float nextSpawnTime = 0f;

    // initialization
    private static GameObject plant;
    private static Vector3 zeroPos = new Vector3(0f, 1f, 0f);
    private static Vector3 zeroRot = new Vector3(0f, 0f, 0f);
    private static Vector3 add = new Vector3(0f, 10f, 0f);
    private static Quaternion zeroRotQ = Quaternion.Euler(zeroRot);

    // bones
    public  int bonesNumber = 9;
    public static string bone_init = "Armature/Bone";
    public static List<Transform> bones = new List<Transform>();

    public int time = 0;

    // Start is called before the first frame update
    void Start()
    {
        plant = Instantiate(beetLeaf, zeroPos, zeroRotQ);
        plant.transform.localScale = beetLeafScale;
        bones.Add(plant.transform.Find(bone_init));

        for (int i = 1; i < bonesNumber; i++)
        {
            bone_init += "/Bone_00"+i.ToString();
            bones.Add(plant.transform.Find(bone_init));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ShouldSpawn())
        {

            for (int i = 0; i < bonesNumber; i++)
            {
                bones[i].rotation = Quaternion.Euler(zeroRot + add);
                add[2] += time * i;
            }

            time += 1;
            nextSpawnTime += spawnDelay;
        }
    }

    private bool ShouldSpawn()
    {
        return Time.time >= nextSpawnTime;
    }

    GameObject BendLeaf()
    {
        GameObject createdLeaf = new GameObject();
        return createdLeaf;
    }
}
