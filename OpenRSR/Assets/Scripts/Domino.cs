using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using OpenRSR.Animation;

public enum DominoType
{
    ManuallyTriggered,
    AutomaticallyTriggered
}

public enum DominoSubtype
{
    Subtype1,
    Subtype2,
    Subtype3
}

public class Domino : MonoBehaviour
{
    public DominoType dominoType;
    public DominoSubtype dominoSubtype;
    public bool hasMoved = false;
    public bool isGroupLeader = false;
    public Vector3 direction;
    public List<Domino> dominoGroup = new List<Domino>();
    public bool shouldGroup = true;
    public SphereMovement sphm;

    void Start() {
        sphm = GameObject.Find("Balus").GetComponent<SphereMovement>();
    }

    void Update()
    {
        if (!GameManager.instance.levelEdit.isInEditor && gameObject.activeSelf) {
            UpdateDominoGrouping();
        }
        if (isGroupLeader && direction == Vector3.zero) {
            SetDirection();
            foreach (var domino in dominoGroup) {
                domino.direction = direction;
            }
        }
        if (this.direction == Vector3.zero) {
            foreach (var domino in dominoGroup) {
                if (domino.isGroupLeader && domino.direction != Vector3.zero) {
                    this.direction = domino.direction;
                    break;
                }
            }
        }
    }

    private void UpdateDominoGrouping()
    {
        var hitColliders = Physics.OverlapSphere(transform.position, 1f);
        bool isAdjacent = hitColliders.Any(c =>
        {
            var adjacentDomino = c.GetComponent<Domino>();
            return adjacentDomino != null &&
                   !dominoGroup.Contains(adjacentDomino) &&
                   adjacentDomino.dominoType == dominoType &&
                   adjacentDomino.dominoSubtype == dominoSubtype;
        });

        shouldGroup = isAdjacent;
        if (shouldGroup) {
            GroupAdjacentDominos();
        }
    }

    public void SetDirection()
    {
        List<GameObject> arrows = new List<GameObject>();
        string tag = dominoType == DominoType.ManuallyTriggered ? "MoverArrowCollision" : "MoverAutoArrowCollision";
        arrows.AddRange(GameObject.FindGameObjectsWithTag(tag));

        foreach (var arrow in arrows)
        {
            if (arrow.transform.position.x == transform.position.x && arrow.transform.position.z == transform.position.z)
            {
                var arrowNormal = arrow.transform.parent.GetChild(1).gameObject;
                float rotationY = arrowNormal.transform.rotation.eulerAngles.y;
                direction = rotationY switch
                {
                    0f => Vector3.forward,
                    90f => Vector3.right,
                    180f => Vector3.back,
                    270f => Vector3.left,
                    _ => direction
                };
                break;
            }
        }
    }

    public void TriggerManualDomino()
    {
        if (dominoType == DominoType.ManuallyTriggered && !hasMoved)
        {
            TriggerDominoGroup();
            shouldGroup = false;
        }
    }

    public void TriggerAutoDomino()
    {
        if (dominoType == DominoType.AutomaticallyTriggered && !hasMoved)
        {
            TriggerDominoGroup();
            shouldGroup = false;
        }
    }

    private void TriggerDominoGroup()
    {
        foreach (var domino in dominoGroup)
        {
            if (!domino.hasMoved && domino.direction != Vector3.zero)
            {
                StartCoroutine(domino.MoveDomino(direction));
                domino.hasMoved = true;
                domino.shouldGroup = false;
            }
        }
    }

    private IEnumerator MoveDomino(Vector3 moveDirection)
    {
        float moveDuration = 0.015f / (sphm.speed / 6f);
        float elapsedTime = 0f;
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = initialPosition + moveDirection;

        GameObject parent = transform.parent.gameObject;
        GameObject m_riser = FindRiserAtPosition(initialPosition);

        Vector3 riserPosition = m_riser?.transform.position ?? Vector3.zero;
        Vector3 targetRiserPosition = riserPosition + moveDirection;

        var arrows = FindArrows();
        ActivateArrow(arrows, initialPosition);

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            parent.transform.position = Vector3.Lerp(initialPosition, targetPosition, t);

            if (m_riser != null)
            {
                m_riser.transform.position = Vector3.Lerp(riserPosition, targetRiserPosition, t);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        parent.transform.position = targetPosition;

        if (Mathf.Abs(GameObject.Find("Balus").transform.position.z - targetPosition.z) < 1f)
        {
            GameObject spawnedDomino = Instantiate(transform.gameObject, initialPosition, Quaternion.identity, parent.transform);

            // Destroy riser in spawned domino if it exists
            for (int i = spawnedDomino.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = spawnedDomino.transform.GetChild(i);
                if (child.CompareTag("Riser"))
                {
                    Destroy(child.gameObject);
                }
            }
        }

        if (m_riser != null)
        {
            m_riser.transform.position = targetRiserPosition;
        }

        UpdateRiserFrames(m_riser, targetPosition);
        TriggerAdjacentDominos();
    }


    private GameObject FindRiserAtPosition(Vector3 position)
    {
        return GameObject.FindGameObjectsWithTag("Riser")
                         .FirstOrDefault(riser => riser.transform.position.x == position.x &&
                                                  riser.transform.position.z == position.z);
    }

    private List<GameObject> FindArrows()
    {
        string tag = dominoType == DominoType.ManuallyTriggered ? "MoverArrowCollision" : "MoverAutoArrowCollision";
        return new List<GameObject>(GameObject.FindGameObjectsWithTag(tag));
    }

    private void ActivateArrow(List<GameObject> arrows, Vector3 position)
    {
        foreach (var arrow in arrows)
        {
            if (arrow.transform.position.x == position.x && arrow.transform.position.z == position.z)
            {
                var arrowParent = arrow.transform.parent.gameObject;
                arrowParent.transform.GetChild(1).gameObject.SetActive(false);
                arrowParent.transform.GetChild(0).gameObject.SetActive(true);
                break;
            }
        }
    }

    private void UpdateRiserFrames(GameObject m_riser, Vector3 targetPosition)
    {
        if (m_riser != null && m_riser.TryGetComponent<BaseAnim>(out BaseAnim baseAnim))
        {
            foreach (var animator in baseAnim.animators)
            {
                foreach (var frame in animator.frames)
                {
                    frame.position = new Vector3(targetPosition.x, frame.position.y, targetPosition.z);
                }
            }
        }
    }

    private void GroupAdjacentDominos()
    {
        HashSet<Domino> visitedDominos = new HashSet<Domino>();
        Stack<Domino> dominosToProcess = new Stack<Domino>();

        dominosToProcess.Push(this);

        while (dominosToProcess.Count > 0)
        {
            Domino currentDomino = dominosToProcess.Pop();

            if (visitedDominos.Add(currentDomino))
            {
                //visitedDominos.Add(currentDomino);
                var hitColliders = Physics.OverlapSphere(currentDomino.transform.position, 1f);

                foreach (var hitCollider in hitColliders)
                {
                    Domino adjacentDomino = hitCollider.GetComponent<Domino>();
                    if (adjacentDomino != null &&
                        adjacentDomino.dominoType == currentDomino.dominoType &&
                        adjacentDomino.dominoSubtype == currentDomino.dominoSubtype &&
                        !visitedDominos.Contains(adjacentDomino))
                    {
                        adjacentDomino.direction = currentDomino.direction;
                        dominoGroup.Add(adjacentDomino);
                        dominosToProcess.Push(adjacentDomino);
                    }
                }
            }
        }

        if (!dominoGroup.Contains(this))
        {
            dominoGroup.Add(this);
        }
    }

    private void TriggerAdjacentDominos()
    {
        foreach (var domino in dominoGroup)
        {
            if (!domino.hasMoved)
            {
                if (domino.dominoType == DominoType.ManuallyTriggered)
                {
                    domino.TriggerManualDomino();
                }
                else if (domino.dominoType == DominoType.AutomaticallyTriggered)
                {
                    domino.TriggerAutoDomino();
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var otherDomino = collision.gameObject.GetComponent<Domino>();
        if (otherDomino != null && (this.hasMoved || otherDomino.hasMoved))
        {
            Vector3 offset = otherDomino.transform.position - transform.position;
            if (Mathf.Abs(offset.x) + Mathf.Abs(offset.z) == 1f && otherDomino.dominoType == DominoType.AutomaticallyTriggered) {
                otherDomino.TriggerAutoDomino();
            }
        }
    }
}
