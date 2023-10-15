using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamingConvention : MonoBehaviour
{
    /// <summary>
    /// Must follow the following rules for naming convention
    /// 1. A public variable / Method should always start with a Capital Letter
    /// 2. A private variable / Method should always start with a small letter
    /// 3. A serializefield variable should start with m_ followed by Variable name starting with a Capital letter
    /// 4. An internal variable within the method shouls start with _ followed by a variable name starting with small letter
    /// 5. Avoid using internal variables As much as possible for performance reasons.
    /// 6. Always write the code in clean redable pattern
    /// 7. Design your logic in a way that we can squeeze maximum performance out of c#.
    /// </summary>
    public float MyPublicVariable;
    private float myPrivateVariable;
    [SerializeField] private string m_MyName;
    // Start is called before the first frame update
    void Start()
    {
        float _myInternalVariable = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Its a good practice to name your methods properly, because its a collaborative environment, other people should be able to understand it,
    // Please write the summary of the method in easily understandable manner so other developers should be easily able to understand what this method does.
    /// <summary>
    /// This method calculates the bullet Belastics.
    /// </summary>
    public void ProcessBulletBelastics()
    {

    }
    /// <summary>
    /// As the name suggests this method will destroy the bullet.
    /// </summary>
    private void destroyBullet()
    {

    }
}
