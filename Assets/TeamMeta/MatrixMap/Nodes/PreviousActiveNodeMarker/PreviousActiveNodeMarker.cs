using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.TeamMeta.MatrixMap
{
    public class PreviousActiveNodeMarker : MonoBehaviour
    {
        [SerializeField] Transform model;
        [SerializeField] Node node;
        [SerializeField] float distanceFromNode = 1f;

        Vector3 nodeLastPosition;
        bool hasBeenInitiated = false;

        [Header("Hover")]
        [SerializeField] Vector3 hoverSpeed = new Vector3(0.1f, 0.1f, 0);
        [SerializeField] Vector3 baseHoverMax = new Vector3(1, 1, 0);
        Vector3 hoverDirection = new Vector3(1, 1, 0);
        Vector3 hoverMax = new Vector3(1, 1, 0);
        private Vector3 offset;

        [Header("Appear")]
        [SerializeField] ColorHdr appearColorHdr;
        [SerializeField] ColorHdr disappearColorHdr;

        ColorHdr baseColorHdr;
        private Material modelMaterial;

        private void Awake()
        {
            Init();
        }
        // Start is called before the first frame update
        void Start()
        {
            if(node)
                MarkNode(node, Vector3.zero);



        }
        public void Init()
        {
            InitOnce();

            hoverMax.x = Random.Range(0f, baseHoverMax.x);
            hoverMax.y = Random.Range(0f, baseHoverMax.y);

            //ExecuteDisappear(1);

            hasBeenInitiated = true;

        }
        void InitOnce()
        {
            if (hasBeenInitiated)
                return;
            modelMaterial = model.GetComponent<Renderer>().material;

            baseColorHdr.color = modelMaterial.GetColor("_Color2");
            baseColorHdr.intensity = modelMaterial.GetFloat("_ColorIntensity2");

        }
        // Update is called once per frame
        void Update()
        {
            if (node!=null)
            {
                if (node.transform.position != nodeLastPosition)
                {
                    UpdatePosition(Vector3.zero);
                }
                Hover();
                RotateModelToLookAtNode(node.transform.position);

            }
        }
        public void UnmarkNode()
        {
            node = null;
        }
        public void MarkNode(Node node, Vector3 mapCenter)
        {
            this.node = node;

            UpdatePosition(mapCenter);

            RotateModelToLookAtNode(node.transform.position);
        }
        void UpdatePosition(Vector3 mapCenter)
        {
            Vector3 position = node.transform.position;
            nodeLastPosition = node.transform.position;

            Vector3 targetDirection = (mapCenter - node.transform.position);
            targetDirection.Normalize();

            position -= targetDirection * distanceFromNode;

            transform.position = position;
        }
        public void RotateModelToLookAtNode(Vector3 nodePosition)
        {
            Vector3 direction = nodePosition - model.position;
            var v1 = -model.transform.up;
            var v2 = direction;

            var degrees = Mathf.Atan2(v2.y, v2.x) - Mathf.Atan2(v1.y, v1.x);
            degrees *= Mathf.Rad2Deg;
            model.transform.Rotate(0, 0, degrees);
        }
        public void Appear(float duration, float delay)
        {
            StartCoroutine(AppearRoutine(duration,delay));
        }
        IEnumerator AppearRoutine(float duration, float delay)
        {
            yield return new WaitForSeconds(delay);
            float t = 0;

            while (t<1)
            {
                ExecuteAppear(t);

                t += Time.deltaTime/ duration;
                yield return null;
            }
            ExecuteAppear(1);

            yield break;
        }
        void ExecuteAppear(float t)
        {
            var appearColor = Color.Lerp(appearColorHdr.color, baseColorHdr.color, t);
            var appearIntensity = Mathf.Lerp(appearColorHdr.intensity, baseColorHdr.intensity, t);
            modelMaterial.SetColor("_Color2", appearColor);
            modelMaterial.SetFloat("_ColorIntensity2", appearIntensity);


            modelMaterial.SetFloat("_AppearProgress",t);
        }
        void ExecuteDisappear(float t)
        {
            modelMaterial.SetFloat("_AppearProgress",1-t);
            var disappearColor = Color.Lerp(baseColorHdr.color, disappearColorHdr.color, Mathf.Clamp01(t * 2));
            var disappearIntensity = Mathf.Lerp(baseColorHdr.intensity, disappearColorHdr.intensity,Mathf.Clamp01( t*2));
            modelMaterial.SetColor("_Color2", disappearColor);
            modelMaterial.SetFloat("_ColorIntensity2", disappearIntensity);
        }
        public void Disappear(float duration, float delay)
        {
            
            StartCoroutine(DisappearRoutine(duration, delay));
        }
        IEnumerator DisappearRoutine(float duration, float delay)
        {
            if(delay > 0)
                yield return new WaitForSeconds(delay);
            if(duration == 0)
            {
                ExecuteDisappear(1);
                yield break;
            }
            float t = 1-modelMaterial.GetFloat("_AppearProgress");

            while (t < 1)
            {
                ExecuteDisappear(t);

                t += Time.deltaTime/ duration;
                yield return null;
            }
            ExecuteDisappear(1);

            yield break;
        }
        //Hover code
        private void Hover()
        {
            HoverX();
            HoverY();

            model.transform.localPosition = offset.x * model.transform.right;
            model.transform.localPosition += offset.y * model.transform.up;
        }
        private void HoverX()
        {
            offset.x += CalculateHoverDelta(hoverDirection.x, hoverSpeed.x);


            if (offset.x > hoverMax.x && hoverDirection.x == 1)
            {
                hoverDirection.x = -Mathf.Abs(hoverDirection.x);
                offset.x = hoverMax.x;
                hoverMax.x = Random.Range(0f, baseHoverMax.x);
            }
            if (offset.x < -hoverMax.x && hoverDirection.x == -1)
            {
                hoverDirection.x = Mathf.Abs(hoverDirection.x);
                offset.x = -hoverMax.x;
                hoverMax.x = Random.Range(0f, baseHoverMax.x);

            }
            // Set the position to the BasePosition plus the offset
        }
        private void HoverY()
        {
            offset.y += CalculateHoverDelta(hoverDirection.y, hoverSpeed.y);

            if (offset.y > hoverMax.y && hoverDirection.y == 1)
            {
                hoverDirection.y = -Mathf.Abs(hoverDirection.y);
                offset.y = hoverMax.y;
                hoverMax.y = Random.Range(0f, baseHoverMax.y);
            }
            if (offset.y < -hoverMax.y && hoverDirection.y == -1)
            {
                hoverDirection.y = Mathf.Abs(hoverDirection.y);
                offset.y = -hoverMax.y;
                hoverMax.y = Random.Range(0f, baseHoverMax.y);
            }
        }
        private float CalculateHoverDelta(float direction, float speed)
        {
            return Mathf.PerlinNoise(Time.time, 0) * direction * speed * Time.deltaTime;
        }
    }
}
