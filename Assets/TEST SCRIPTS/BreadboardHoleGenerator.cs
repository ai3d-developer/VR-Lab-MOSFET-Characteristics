using UnityEngine;

public class BreadboardGenerator : MonoBehaviour
{
    [Header("TOP SECTION - First Column")]
    public Transform A1;
    public Transform B1;
    public Transform C1;
    public Transform D1;
    public Transform E1;

    [Header("BOTTOM SECTION - First Column")]
    public Transform F1;
    public Transform G1;
    public Transform H1;
    public Transform I1;
    public Transform J1;

    [Header("POSITIVE RAIL - First Column")]
    public Transform Positive1;
    public Transform Positive2;

    [Header("NEGATIVE RAIL - First Column")]
    public Transform Negative1;
    public Transform Negative2;

    [Header("Generation")]
    public int totalColumns = 30;

    [Tooltip("Distance between columns in X direction")]
    public float xSpacing = 0.00254f;

    [ContextMenu("Generate Breadboard")]
    public void GenerateBreadboard()
    {
        if (!CheckReferences())
            return;

        ClearGeneratedObjects();

        GenerateTopSection();
        GenerateBottomSection();
        GeneratePositiveRails();
        GenerateNegativeRails();

        Debug.Log("Breadboard generated successfully!");
    }

    // =========================================================
    // CHECK REFERENCES
    // =========================================================

    bool CheckReferences()
    {
        if (A1 == null || B1 == null || C1 == null ||
            D1 == null || E1 == null)
        {
            Debug.LogError("Assign A1, B1, C1, D1, E1.");
            return false;
        }

        if (F1 == null || G1 == null || H1 == null ||
            I1 == null || J1 == null)
        {
            Debug.LogError("Assign F1, G1, H1, I1, J1.");
            return false;
        }

        if (Positive1 == null || Positive2 == null)
        {
            Debug.LogError("Assign Positive1 and Positive2.");
            return false;
        }

        if (Negative1 == null || Negative2 == null)
        {
            Debug.LogError("Assign Negative1 and Negative2.");
            return false;
        }

        return true;
    }

    // =========================================================
    // TOP A-E
    // =========================================================

    void GenerateTopSection()
    {
        Transform[] references =
        {
            A1, B1, C1, D1, E1
        };

        string[] names =
        {
            "A", "B", "C", "D", "E"
        };

        for (int column = 2; column <= totalColumns; column++)
        {
            for (int row = 0; row < 5; row++)
            {
                Transform reference = references[row];

                GameObject hole =
                    CreateHole("Hole_" + names[row] + column);

                Vector3 position =
                    reference.localPosition;

                // X direction only
                position.x +=
                    xSpacing * (column - 1);

                hole.transform.localPosition = position;

                hole.transform.localRotation =
                    reference.localRotation;

                hole.transform.localScale =
                    reference.localScale;

                BreadboardHole data =
                    hole.GetComponent<BreadboardHole>();

                data.holeID =
                    names[row] + column;

                // A-E of same column
                // are electrically connected
                data.nodeID = column;
            }
        }
    }

    // =========================================================
    // BOTTOM F-J
    // =========================================================

    void GenerateBottomSection()
    {
        Transform[] references =
        {
            F1, G1, H1, I1, J1
        };

        string[] names =
        {
            "F", "G", "H", "I", "J"
        };

        for (int column = 2; column <= totalColumns; column++)
        {
            for (int row = 0; row < 5; row++)
            {
                Transform reference = references[row];

                GameObject hole =
                    CreateHole("Hole_" + names[row] + column);

                Vector3 position =
                    reference.localPosition;

                // X direction only
                position.x +=
                    xSpacing * (column - 1);

                hole.transform.localPosition = position;

                hole.transform.localRotation =
                    reference.localRotation;

                hole.transform.localScale =
                    reference.localScale;

                BreadboardHole data =
                    hole.GetComponent<BreadboardHole>();

                data.holeID =
                    names[row] + column;

                // F-J of same column
                // are electrically connected
                data.nodeID = 100 + column;
            }
        }
    }

    // =========================================================
    // POSITIVE RAILS
    // =========================================================

    void GeneratePositiveRails()
    {
        Transform[] references =
        {
            Positive1,
            Positive2
        };

        for (int column = 2; column <= totalColumns; column++)
        {
            for (int rail = 0; rail < 2; rail++)
            {
                Transform reference =
                    references[rail];

                GameObject railPoint =
                    CreateHole(
                        "Positive_" +
                        (rail + 1) +
                        "_" +
                        column
                    );

                Vector3 position =
                    reference.localPosition;

                // X direction only
                position.x +=
                    xSpacing * (column - 1);

                railPoint.transform.localPosition =
                    position;

                railPoint.transform.localRotation =
                    reference.localRotation;

                railPoint.transform.localScale =
                    reference.localScale;

                BreadboardHole data =
                    railPoint.GetComponent<BreadboardHole>();

                data.holeID =
                    "+_" +
                    (rail + 1) +
                    "_" +
                    column;

                // Both positive rows
                // are same electrical node
                data.nodeID = 1000;
            }
        }
    }

    // =========================================================
    // NEGATIVE RAILS
    // =========================================================

    void GenerateNegativeRails()
    {
        Transform[] references =
        {
            Negative1,
            Negative2
        };

        for (int column = 2; column <= totalColumns; column++)
        {
            for (int rail = 0; rail < 2; rail++)
            {
                Transform reference =
                    references[rail];

                GameObject railPoint =
                    CreateHole(
                        "Negative_" +
                        (rail + 1) +
                        "_" +
                        column
                    );

                Vector3 position =
                    reference.localPosition;

                // X direction only
                position.x +=
                    xSpacing * (column - 1);

                railPoint.transform.localPosition =
                    position;

                railPoint.transform.localRotation =
                    reference.localRotation;

                railPoint.transform.localScale =
                    reference.localScale;

                BreadboardHole data =
                    railPoint.GetComponent<BreadboardHole>();

                data.holeID =
                    "-_" +
                    (rail + 1) +
                    "_" +
                    column;

                // Both negative rows
                // are same electrical node
                data.nodeID = 1001;
            }
        }
    }

    // =========================================================
    // CREATE GAMEOBJECT
    // =========================================================

    GameObject CreateHole(string objectName)
    {
        GameObject hole =
            new GameObject(objectName);

        hole.transform.SetParent(transform);

        hole.AddComponent<BreadboardHole>();

        return hole;
    }

    // =========================================================
    // CLEAR GENERATED OBJECTS
    // =========================================================

    void ClearGeneratedObjects()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child =
                transform.GetChild(i);

            // Keep manually created references
            if (child == A1 ||
                child == B1 ||
                child == C1 ||
                child == D1 ||
                child == E1 ||

                child == F1 ||
                child == G1 ||
                child == H1 ||
                child == I1 ||
                child == J1 ||

                child == Positive1 ||
                child == Positive2 ||

                child == Negative1 ||
                child == Negative2)
            {
                continue;
            }

            DestroyImmediate(child.gameObject);
        }
    }
}