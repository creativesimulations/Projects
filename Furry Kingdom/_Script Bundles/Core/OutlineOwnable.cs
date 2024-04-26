using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Furry
{


public class OutlineOwnable : MonoBehaviour
    {
        private Outline _outline;
        private void Awake()
        {
            _outline = GetComponent<Outline>();
            _outline.OutlineMode = Outline.Mode.OutlineAll;
            _outline.OutlineWidth = 5f;
        }
        // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        }
        private void SetOutlineColor()
        {
            Debug.Log("SetOutlineColor" + _outline.OutlineColor);
            _outline.OutlineColor = Color.yellow;
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("collision is   " + collision.gameObject.name);
            Player player;
            collision.gameObject.TryGetComponent<Player>(out player);
            if (player != null)
            {
                SetOutlineColor();
            }
        }
        
    }

}